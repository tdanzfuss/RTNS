using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Threading;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Linq;


namespace TMB.Reuters
{
    public class RTNSConnection
    {
        private string IP;
        private int Port;
        private int SubscriberPort;
        private string Username;
        private string Pwd;
        private int Keepalive;
        private int RetryInterval;
        private bool IsConnected;
        private TcpClient clientConnection;
        private TcpClient subscriptionConnection;
        private NetworkStream ns;
        private NetworkStream subStream;
        private Dictionary<string,string> commandTable;
        // private List<string> subscribtionCommands;
        private ConcurrentQueue<string> subscribtionCommands;
        private Timer RtnsTimer;
        private int RtnsTimeout;
        private RTNSConnectionState state;
        private string message;
        private Data.TMBDataContext context;

        public RTNSConnection(string ip, int port, int subscribtionPort, string username, string pwd, int keepalive, string connectionString, string version, int rtnsTimeout, int rtnsRetryInterval)
        {
            this.IP = ip;
            this.Port = port;
            this.Username = username;
            this.Pwd = pwd;
            this.Keepalive = keepalive;
            this.RetryInterval = rtnsRetryInterval;
            this.clientConnection = new TcpClient();
            this.subscriptionConnection = new TcpClient();
            this.SubscriberPort = subscribtionPort;
            commandTable = InitializeCommands(connectionString, version);
            subscribtionCommands = new ConcurrentQueue<string>();

            this.RtnsTimeout = rtnsTimeout;
            this.context = new Data.TMBDataContext();
        }

        public RTNSConnectionState ConnectionState
        {
            get { return state; }
            private set
            {
                if (state != value)
                {
                    state = value; OnConnectionStatusChange(EventArgs.Empty);
                }
            }
        }

        public string RTNSConnectionMessage
        {
            get { return message; }
            private set
            {
                if (message != value)
                {
                    message = value; OnConnectionStatusChange(EventArgs.Empty);
                }
            }
        }

        private Dictionary<string, string> InitializeCommands(string connectionString, string version)
        {
            TMB.Data.TMBDataContext context = new Data.TMBDataContext(connectionString);
            return context
                .IntegrationTemplates
                .Where(w => w.Version == Convert.ToInt32(version))
                .ToDictionary(x => x.Name, x => x.Command);
        }

        #region CONNECTION MANAGEMENT
        public void Connect()
        {
            state = RTNSConnectionState.Warning;
            try
            {
                this.RTNSConnectionMessage = "Initializing Connection to RTNS";
                // connect to publisher and subscriber ports
                clientConnection.Connect(IP, Port);
                subscriptionConnection.Connect(IP, SubscriberPort);

                // Get both streams 
                ns = clientConnection.GetStream();
                subStream = subscriptionConnection.GetStream();

                subscribeToUpdates();
                // Yip we're connected OK
                IsConnected = true;
                // state = RTNSConnectionState.OK;
                // this.RTNSConnectionMessage = "Connected to RTNS";            


                // Start the timer that watches for timeouts on RTNS
                RtnsTimer = new Timer(new TimerCallback(RTNSTimeout), null, RtnsTimeout, RtnsTimeout);

                // Now keep the connection alive and respond to subscribtion messages from the publisher
                ThreadPool.QueueUserWorkItem(new WaitCallback(keepAliveWorker));
                ThreadPool.QueueUserWorkItem(new WaitCallback(subscriptionWorker));
                ThreadPool.QueueUserWorkItem(new WaitCallback(subscribtionCommandWorker));
            }
            catch (Exception ex) 
            {
                this.RTNSConnectionMessage = "Could not connect to RTNS";
                ConnectionState = RTNSConnectionState.Error;
            }
        }

        public void Disconnect()
        {
            IsConnected = false;
            state = RTNSConnectionState.Error;
            this.RTNSConnectionMessage = "Disconnected from RTNS";   

            if (RtnsTimer != null)
                RtnsTimer.Dispose();

            if (ns != null)
            {
                ns.Flush();
                ns.Dispose();
                ns = null;
            }
            if (subStream != null)
            {
                subStream.Flush();
                subStream.Dispose();
                subStream = null;
            }
            clientConnection.Close();
            subscriptionConnection.Close();
        }

        private void subscriptionWorker(object data)
        {
            while (IsConnected)
            {
                Thread.Sleep(Keepalive);
                List<string> commands = readCommandFromStream(subStream,0,true);
                foreach (string completeCommand in commands)
                    subscribtionCommands.Enqueue(completeCommand);                
            }
            /* while (IsConnected)
             {
                 Thread.Sleep(Keepalive);

                 byte [] readBuffer = new byte[1024];
                 // StringBuilder responseString = new StringBuilder();
                 string buffer = string.Empty;
                 int numBytesRead = 0;
                 try
                 {
                     if ((subStream.CanRead && subStream.DataAvailable))
                     {
                         while (subStream.DataAvailable)
                         {
                             numBytesRead = subStream.Read(readBuffer, 0, readBuffer.Length);
                             buffer += Encoding.ASCII.GetString(readBuffer, 0, numBytesRead);
                             // responseString.Append(Encoding.ASCII.GetString(readBuffer, 0, numBytesRead));
                             // string[] commands = responseString.ToString().Split(new string[]{"</RWP_1>"},StringSplitOptions.RemoveEmptyEntries);
                             int endIdx = buffer.IndexOf("</RWP_1>");
                             while (endIdx > 0)
                             {
                                 string command = buffer.Substring(0, endIdx + 8);
                                 subscribtionCommands.Enqueue(command);
                                 buffer = buffer.Remove(0, endIdx + 8);
                                 endIdx = buffer.IndexOf("</RWP_1>");
                             }
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     OnConnectionError(EventArgs.Empty);
                 }
             }    */
        }

        private List<string> readCommandFromStream(NetworkStream ns, int retryCount, bool dump)
        {
            List<string> completecommands = new List<string>();

            byte[] readBuffer = new byte[1024];
            string buffer = string.Empty;
            int numBytesRead = 0;
            try
            {
                if ((ns.CanRead && ns.DataAvailable))
                {
                    while (ns.DataAvailable)
                    {
                        numBytesRead = ns.Read(readBuffer, 0, readBuffer.Length);
                        string bufferstring = Encoding.ASCII.GetString(readBuffer, 0, numBytesRead);                       
                        buffer = buffer+bufferstring;                       
                        int endIdx = buffer.IndexOf("</RWP_1>");
                        while (endIdx > 0)
                        {
                            string command = buffer.Substring(0, endIdx + 8);
                            // completecommand = command;
                            completecommands.Add(command);
                            buffer = buffer.Remove(0, endIdx + 8);
                            endIdx = buffer.IndexOf("</RWP_1>");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nothing to Read - Retry Count :" + retryCount);
                }

                // If we require to wait for a response then call this method again until we get something back...
                if ((retryCount > 0) && (completecommands.Count() == 0))
                {
                    Console.WriteLine("No response, retrying in " + RetryInterval + "ms. Retries left: " + retryCount);
                    Thread.Sleep(RetryInterval);
                    completecommands.AddRange(readCommandFromStream(ns, --retryCount, dump));
                }
            }
            catch (Exception ex)
            {
                OnConnectionError(EventArgs.Empty);
            }
            
            return completecommands;
        }

        private void keepAliveWorker(object data)
        {
            while (IsConnected)
            {
                // Suspend the thread
                Thread.Sleep(Keepalive);
                // Invoke a KeepAlive call to the publisher adapter, if error then raise event
                try
                {
                    //using (NetworkStream ns = clientConnection.GetStream())
                    //{
                    byte[] arr = Encoding.ASCII.GetBytes(commandTable["KEEP_ALIVE"]);
                    ns.Write(arr, 0, arr.Length);
                    // ns.Flush();
                    //}
                }
                catch (Exception ex)
                {
                    OnConnectionError(EventArgs.Empty);
                }
            }
        }
        private void subscribeToUpdates()
        {
            try
            {
                byte[] arr = Encoding.ASCII.GetBytes(commandTable["HEARTBEAT"]);
                subStream.Write(arr, 0, arr.Length);
            }
            catch (Exception ex)
            {
                OnConnectionError(EventArgs.Empty);
            }
        }
        #endregion

        #region SUBSCRIBTION MANAGEMENT
        private void subscribtionCommandWorker(object data)
        {
            while (IsConnected || (subscribtionCommands.Count > 0))
            {
                Thread.Sleep(Keepalive);

                string commandString = string.Empty;
                while (subscribtionCommands.TryDequeue(out commandString))
                {                    
                    // This is a Heartbeat command, reset the heartbeat timer and start timing again.
                    // We do not raise events for the heartbeat response
                    if (commandString.Contains("<HEARTBEAT></HEARTBEAT>"))
                    {
                        if (state != RTNSConnectionState.OK)
                        {
                            state = RTNSConnectionState.OK;
                            RTNSConnectionMessage = "Connected to RTNS";
                        }
                        RtnsTimer.Change(RtnsTimeout, RtnsTimeout);
                    }
                    else
                    {
                        // First lets log the subscribtion Event
                        int logID = LogSubscribtionMessage(commandString);

                        // Now process it
                        ProcessSubscribtionMessage(logID, commandString);

                        // then raise subscribtion event for all other subscribtion events
                        
                        SubscribtionEventArgs args = new SubscribtionEventArgs(commandString);
                        OnSubscribtionEvent(args);
                    }
                }
            }
        }
        private void RTNSTimeout(object data)
        {
            // Timer timed out, meening we havent received a HEARTBEAT from RTNS in the allowed time period.
            state = RTNSConnectionState.Warning;
            this.RTNSConnectionMessage = "Connection to RTNS Timeout";
        }
        #endregion

        #region CONNECTION EVENTS
        protected virtual void OnConnectionError(EventArgs e)
        {
            EventHandler handler = ConnectionError;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnSubscribtionEvent(SubscribtionEventArgs e)
        {
            EventHandler handler = SubscribtionEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnConnectionStatusChange(EventArgs e)
        {
            EventHandler handler = ConnectionStatusChange;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        public event EventHandler ConnectionError;
        public event EventHandler SubscribtionEvent;
        public event EventHandler ConnectionStatusChange;
        #endregion

        #region DEALS
        public RTNSResponseMessage ConfirmFXSpot
            (string brokerGroupName, string brokerUserName, TMB.Data.Transaction deal)
        {
            RTNSResponseMessage responseMsg = null;

            XDocument doc = CreateDealXml("FX_SPOT_NOTIFY"
                , brokerGroupName
                , brokerUserName
                , deal);

            responseMsg = SubmitDealToRTNS(deal.ID, doc);

            return responseMsg;
        }

        public RTNSResponseMessage ConfirmFXForward(string brokerGroupName, string brokerUserName, TMB.Data.Transaction deal)
        {//FX_FWD_NOTIFY
            RTNSResponseMessage responseMsg = null;

            XDocument doc = CreateDealXml("FX_FWD_NOTIFY"
                , brokerGroupName
                , brokerUserName
                , deal);

            responseMsg = SubmitDealToRTNS(deal.ID, doc);

            return responseMsg;
        }

        public RTNSResponseMessage ConfirmFXSwap(string brokerGroupName, string brokerUserName, TMB.Data.Transaction deal)
        {
            RTNSResponseMessage responseMsg = null;

            XDocument doc = CreateDealXml("FX_SWAP_NOTIFY"
                , brokerGroupName
                , brokerUserName
                , deal);

            responseMsg = SubmitDealToRTNS(deal.ID, doc);

            return responseMsg;
        }

        public RTNSResponseMessage ConfirmFXNDF(string brokerGroupName, string brokerUserName, TMB.Data.Transaction deal)
        {//FX_NDF_NOTIFY
            RTNSResponseMessage responseMsg = null;

            XDocument doc = CreateDealXml("FX_NDF_NOTIFY"
                , brokerGroupName
                , brokerUserName
                , deal);

            responseMsg = SubmitDealToRTNS(deal.ID, doc);

            return responseMsg;
        }

        public RTNSResponseMessage ConfirmMMDeposit(string brokerGroupName, string brokerUserName, TMB.Data.Transaction deal)
        {// MM_DEPO_NOTIFY
            RTNSResponseMessage responseMsg = null;

            XDocument doc = CreateDealXml("MM_DEPO_NOTIFY"
                , brokerGroupName
                , brokerUserName
                , deal);

            responseMsg = SubmitDealToRTNS(deal.ID, doc);

            return responseMsg;
        }

        public RTNSResponseMessage ConfirmDeal(string brokerGroupName, string brokerUserName, TMB.Data.Transaction deal)
        {
            switch (deal.ProductType)
            {
                case 1:
                    return ConfirmFXSpot(brokerGroupName, brokerUserName, deal);
                case 2:
                    return ConfirmFXForward(brokerGroupName, brokerUserName, deal);
                case 3:
                    return ConfirmFXSwap(brokerGroupName, brokerUserName, deal);
                case 4:
                    return ConfirmFXNDF(brokerGroupName, brokerUserName, deal);
                case 5:
                    return ConfirmMMDeposit(brokerGroupName, brokerUserName, deal);
                default:
                    throw new Exception("Unsupported Product Type");
            }
        }

        public RTNSResponseMessage CloseDeal(TMB.Data.Transaction deal)
        {
            RTNSResponseMessage responseMsg = null;

            XDocument doc = CreateCloseDealXml(deal);

            responseMsg = SubmitDealToRTNS(deal.ID, doc);

            return responseMsg;
        }

        public RTNSResponseMessage UpdateDeal(string brokerGroupName, string brokerUserName, TMB.Data.Transaction deal)
        {
            // Updates must be done seperatly. i.e. a message must be created and sent to the Maker&Taker
            RTNSResponseMessage responseMsg = null;
            XDocument dealDocument;
            XDocument makerDocument;
            XDocument takerDocument;

            switch (deal.ProductType)
            {
                case 1:
                    dealDocument = CreateDealXml("FX_SPOT_NOTIFY", brokerGroupName, brokerUserName, deal);
                    break;
                case 2:
                    dealDocument = CreateDealXml("FX_FWD_NOTIFY", brokerGroupName, brokerUserName, deal);
                    break;                
                case 3:
                    dealDocument = CreateDealXml("FX_SWAP_NOTIFY", brokerGroupName, brokerUserName, deal);
                    break;
                case 4:
                    dealDocument = CreateDealXml("FX_NDF_NOTIFY", brokerGroupName, brokerUserName, deal);
                    break;               
                default:
                    throw new Exception("Unsupported Product Type");
            }

            // create copies of the XML
            makerDocument = new XDocument(dealDocument);
            takerDocument = new XDocument(dealDocument);

            makerDocument.Element("RWP_1").Element("COMMAND").Element("SIDE").SetValue("MAKER");
            takerDocument.Element("RWP_1").Element("COMMAND").Element("SIDE").SetValue("TAKER");

            makerDocument.Element("RWP_1").Element("COMMAND").Element("REFERENCE").SetValue("MAKER:" + deal.TransactionReference);
            takerDocument.Element("RWP_1").Element("COMMAND").Element("REFERENCE").SetValue("TAKER:" + deal.TransactionReference);            

            // IF MAKER IS FDC then change command type to MODIFY_FDC
            if (deal.TransactionLegs[0].BankUser1.ElectronicType.HasValue && deal.TransactionLegs[0].BankUser1.ElectronicType.Value == 2)
                makerDocument.Element("RWP_1").Element("COMMAND").Element("COMMAND_ID").SetValue("MODIFY_FDC");

            // IF TAKER IS FDC then change command type to MODIFY_FDC
            if (deal.TransactionLegs[0].BankUser.ElectronicType.HasValue && deal.TransactionLegs[0].BankUser.ElectronicType.Value == 2)
                takerDocument.Element("RWP_1").Element("COMMAND").Element("COMMAND_ID").SetValue("MODIFY_FDC");

            RTNSResponseMessage makerResponseMsg = SubmitDealToRTNS(deal.ID, makerDocument);
            RTNSResponseMessage takerResponseMsg = SubmitDealToRTNS(deal.ID, takerDocument);

            responseMsg = new RTNSResponseMessage(
                makerResponseMsg.Success && takerResponseMsg.Success,
                "Maker: " + makerResponseMsg.ErrorName + " Taker: " + takerResponseMsg.ErrorName,
                "Maker: " + makerResponseMsg.ErrorDescription + " Taker: " + takerResponseMsg.ErrorDescription);
                
            return responseMsg;
        }

        public RTNSResponseMessage CancelDeal(TMB.Data.Transaction deal)
        {
            RTNSResponseMessage responseMsg = null;
            XDocument dealDocument;
            XDocument makerDocument;
            XDocument takerDocument;

            dealDocument = CreateCancelDealXml(deal);
            // create copies of the XML
            makerDocument = new XDocument(dealDocument);
            takerDocument = new XDocument(dealDocument);

            makerDocument.Element("RWP_1").Element("COMMAND").Element("SIDE").SetValue("MAKER");
            takerDocument.Element("RWP_1").Element("COMMAND").Element("SIDE").SetValue("TAKER");

            makerDocument.Element("RWP_1").Element("COMMAND").Element("REFERENCE").SetValue("CANCEL MAKER:" + deal.TransactionReference);
            takerDocument.Element("RWP_1").Element("COMMAND").Element("REFERENCE").SetValue("CANCEL TAKER:" + deal.TransactionReference);

            // IF MAKER IS FDC then change command type to MODIFY_FDC
            //if (deal.TransactionLegs[0].BankUser1.ElectronicType.HasValue && deal.TransactionLegs[0].BankUser1.ElectronicType.Value == 2)
            //    makerDocument.Element("RWP_1").Element("COMMAND").Element("COMMAND_ID").SetValue("CANCEL_FDC");

            // IF TAKER IS FDC then change command type to MODIFY_FDC
            //if (deal.TransactionLegs[0].BankUser.ElectronicType.HasValue && deal.TransactionLegs[0].BankUser.ElectronicType.Value == 2)
            //    takerDocument.Element("RWP_1").Element("COMMAND").Element("COMMAND_ID").SetValue("CANCEL_FDC");

            RTNSResponseMessage makerResponseMsg = SubmitDealToRTNS(deal.ID, makerDocument);
            RTNSResponseMessage takerResponseMsg = SubmitDealToRTNS(deal.ID, takerDocument);

            if (!makerResponseMsg.Success)
            {
                makerDocument.Element("RWP_1").Element("COMMAND").Element("COMMAND_ID").SetValue("CANCEL_FDC");
                makerResponseMsg = SubmitDealToRTNS(deal.ID, makerDocument);
            }

            if (!takerResponseMsg.Success)
            {
                takerDocument.Element("RWP_1").Element("COMMAND").Element("COMMAND_ID").SetValue("CANCEL_FDC");
                takerResponseMsg = SubmitDealToRTNS(deal.ID, takerDocument);
            }

            responseMsg = new RTNSResponseMessage(
                makerResponseMsg.Success && takerResponseMsg.Success,
                "Maker: " + makerResponseMsg.ErrorName + " Taker: " + takerResponseMsg.ErrorName,
                "Maker: " + makerResponseMsg.ErrorDescription + " Taker: " + takerResponseMsg.ErrorDescription);

            return responseMsg;
        }

        private XDocument CreateDealXml(string commandType,
            string brokerGroupName,
            string brokerUserName,
            TMB.Data.Transaction trxn)
        {
            XDocument doc = XDocument.Parse(commandTable[commandType]);
            var commandPortion =
                    doc.Element("RWP_1")
                    .Element("COMMAND");

            var deal =
                    commandPortion    
                    .Element("DEAL");

            PopulateCommandPortion(brokerGroupName, brokerUserName, trxn, commandPortion);
            PopulateDealPortion(brokerGroupName, brokerUserName, trxn, deal);
            switch (commandType)
            {
                case "FX_SPOT_NOTIFY":
                    PopulateSpotPortion(trxn, deal);
                    break;
                case "FX_SWAP_NOTIFY":
                    PopulateSwapPortion(trxn, deal);
                    break;
                case "FX_FWD_NOTIFY":
                    PopulateForwardPortion(trxn, deal);
                    break;
                case "FX_NDF_NOTIFY":
                    PopulateNDFPortion(trxn, deal);
                    break;
                case "MM_DEPO_NOTIFY":
                    PopulateMMDepoPortion(trxn, deal);
                    break;
            }
                       
            return doc;
        }

        private XDocument CreateCloseDealXml(TMB.Data.Transaction trxn) 
        {
            XDocument doc = XDocument.Parse(commandTable["CLOSE"]);
            var cmd =
                    doc.Element("RWP_1")
                    .Element("COMMAND");
            cmd.Element("REFERENCE").SetValue("CLOSE:"+trxn.TransactionReference);
            cmd.Element("DEAL").Element("ID").SetValue(trxn.TransactionReference);

            return doc;
        }

        private XDocument CreateCancelDealXml(TMB.Data.Transaction trxn)
        {
            XDocument doc = XDocument.Parse(commandTable["FX_CANCEL_NOTIFY"]);
            var cmd =
                    doc.Element("RWP_1")
                    .Element("COMMAND");
            cmd.Element("REFERENCE").SetValue("CANCEL:" + trxn.TransactionReference);
            cmd.Element("DEAL").Element("ID").SetValue(trxn.TransactionReference);
            cmd.Element("DEAL").Element("TRADE_DATE").SetValue(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

            return doc;
        }

        private XDocument CreateSwapDealXml(string commandType,
            string brokerGroupName,
            string brokerUserName,
            TMB.Data.Transaction trxn)
        {
            XDocument doc = XDocument.Parse(commandTable[commandType]);
            var deal =
                    doc.Element("RWP_1")
                    .Element("COMMAND")
                    .Element("DEAL");

            
            PopulateDealPortion(brokerGroupName, brokerUserName, trxn, deal);
            PopulateSwapPortion(trxn, deal);

            return doc;
        }

        private void PopulateCommandPortion(string brokerGroupName, string brokerUserName, TMB.Data.Transaction trxn, XElement cmd)
        {
            cmd.Element("REFERENCE").SetValue("BOTH:" + trxn.TransactionReference);
        }

        private void PopulateDealPortion(string brokerGroupName, string brokerUserName, TMB.Data.Transaction trxn, XElement deal)
        {
            deal.Element("ID").SetValue(trxn.TransactionReference);
            deal.Element("MAKER_PROXY_GROUP_INTERNAL_ID").SetValue(brokerGroupName);
            deal.Element("MAKER_PROXY_INTERNAL_ID").SetValue(brokerUserName);
            deal.Element("TAKER_PROXY_GROUP_INTERNAL_ID").SetValue(brokerGroupName);
            deal.Element("TAKER_PROXY_INTERNAL_ID").SetValue(brokerUserName);
            
            // Taker == buyer
            deal.Element("TAKER_GROUP_INTERNAL_ID").SetValue(trxn.TransactionLegs[0].Bank.InternalID);
            deal.Element("TAKER_INTERNAL_ID").SetValue((trxn.TransactionLegs[0].BankUser == null) ? string.Empty : trxn.TransactionLegs[0].BankUser.InternalID);
            deal.Element("TAKER_FULL_NAME").SetValue(trxn.TransactionLegs[0].BankUser.FullName);
            deal.Element("TAKER_GROUP_FULL_NAME").SetValue(trxn.TransactionLegs[0].Bank.Name);

            // Maker == Seller
            deal.Element("MAKER_GROUP_INTERNAL_ID").SetValue(trxn.TransactionLegs[0].Bank1.InternalID);
            deal.Element("MAKER_INTERNAL_ID").SetValue((trxn.TransactionLegs[0].BankUser1 == null) ? string.Empty : trxn.TransactionLegs[0].BankUser1.InternalID);
            deal.Element("MAKER_FULL_NAME").SetValue(trxn.TransactionLegs[0].BankUser1.FullName);
            deal.Element("MAKER_GROUP_FULL_NAME").SetValue(trxn.TransactionLegs[0].Bank1.Name);

            
            deal.Element("DEAL_DATE").SetValue(trxn.TransactionDate.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"));
            deal.Element("TRADE_DATE").SetValue(trxn.TransactionDate.Value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"));
        }
        private void PopulateSwapPortion(TMB.Data.Transaction trxn, XElement deal)
        {
            var swap = deal.Element("FX_SWAP_DETAILS");

            string[] currencyArr = new string[] { trxn.TransactionLegs[0].Currency.Code, trxn.TransactionLegs[0].Currency1.Code };
            int baseCurrencyIdx = (trxn.BaseCurrency.HasValue) ? trxn.BaseCurrency.Value : 0;
            int dealtCurrencyIdx = (trxn.DealtCurrency.HasValue && trxn.DealtCurrency.Value > 0) ? 1 - baseCurrencyIdx : baseCurrencyIdx;
            swap.Element("BASE_CURRENCY").SetValue(currencyArr[baseCurrencyIdx]);
            swap.Element("TERM_CURRENCY").SetValue(currencyArr[1 - baseCurrencyIdx]);
            //swap.Element("TAKER_BUYS_BASE").SetValue((baseCurrencyIdx == 0) ? "TRUE" : "FALSE");
            // Deat currency is the same as Base currency
            swap.Element("DEALT_CURRENCY").SetValue(currencyArr[dealtCurrencyIdx]);
            // swap.Element("DEALT_CURRENCY").SetValue(trxn.TransactionLegs[0].Currency1.Code);
            if (trxn.Scale.HasValue)
                swap.Element("FORWARD_SCALE_DECIMAL_PLACES").SetValue(trxn.Scale.Value);

            var farLeg = swap.Element("FAR_LEG");
            var nearLeg = swap.Element("NEAR_LEG");

            // Send through the base rate, because RTNS automatically adds the Points
            farLeg.Element("RATE").SetValue(trxn.TransactionLegs[0].Rate.Value);
            farLeg.Element("TAKER_BUYS_BASE").SetValue((baseCurrencyIdx == 0) ? "FALSE" : "TRUE");
            farLeg.Element("AMOUNT").SetValue(context.GetTransactionAmount(trxn));
            farLeg.Element("VALUE_DATE").SetValue(trxn.TransactionLegs[1].ActionDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            // farLeg.Element("SPOT_DATE").SetValue(trxn.TransactionLegs[1].ActionDate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            farLeg.Element("SPOT_DATE").SetValue(trxn.spotdate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            farLeg.Element("FORWARD_POINTS").SetValue(trxn.TransactionLegs[1].ForwardPoints.HasValue ? trxn.TransactionLegs[1].ForwardPoints.Value : 0);

            nearLeg.Element("RATE").SetValue(trxn.TransactionLegs[0].Rate.Value);
            nearLeg.Element("TAKER_BUYS_BASE").SetValue((baseCurrencyIdx == 0) ? "TRUE" : "FALSE");
            nearLeg.Element("AMOUNT").SetValue(context.GetTransactionAmount(trxn));
            nearLeg.Element("VALUE_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            // nearLeg.Element("SPOT_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            nearLeg.Element("SPOT_DATE").SetValue(trxn.spotdate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            nearLeg.Element("FORWARD_POINTS").SetValue(trxn.TransactionLegs[0].ForwardPoints.HasValue ? trxn.TransactionLegs[0].ForwardPoints.Value : 0);
        }
        private void PopulateSpotPortion(TMB.Data.Transaction trxn, XElement deal)
        {
            var spot = deal.Element("FX_SPOT_DETAILS");
            string[] currencyArr = new string[] { trxn.TransactionLegs[0].Currency.Code, trxn.TransactionLegs[0].Currency1.Code };
            int baseCurrencyIdx = (trxn.BaseCurrency.HasValue) ? trxn.BaseCurrency.Value : 0;
            int dealtCurrencyIdx = (trxn.DealtCurrency.HasValue && trxn.DealtCurrency.Value > 0) ? 1 - baseCurrencyIdx : baseCurrencyIdx;
            spot.Element("BASE_CURRENCY").SetValue(currencyArr[baseCurrencyIdx]);
            spot.Element("TERM_CURRENCY").SetValue(currencyArr[1 - baseCurrencyIdx]);
            // Check whether the TAKER BUYS BASE OR TERM Currency
            // TAKER is ALWAYS the BUYER and MAKER Always the SELLER            
            spot.Element("TAKER_BUYS_BASE").SetValue((baseCurrencyIdx == 0) ? "TRUE" : "FALSE");
            

            // Base Currency is always equal to my Dealt Currency.
            // This implies that the Exchange rate and the amount you type in is always in the same currency
            // spot.Element("DEALT_CURRENCY").SetValue(trxn.TransactionLegs[0].Currency1.Code);
            spot.Element("DEALT_CURRENCY").SetValue(currencyArr[dealtCurrencyIdx]);
            spot.Element("RATE").SetValue(trxn.TransactionLegs[0].Rate.Value);
            spot.Element("AMOUNT").SetValue(context.GetTransactionAmount(trxn));
            spot.Element("VALUE_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            spot.Element("SPOT_DATE").SetValue(trxn.spotdate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            // spot.Element("SPOT_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));

        }
        private void PopulateForwardPortion(TMB.Data.Transaction trxn, XElement deal)
        {
            var fwd = deal.Element("FX_FORWARD_DETAILS");
            string[] currencyArr = new string[] { trxn.TransactionLegs[0].Currency.Code, trxn.TransactionLegs[0].Currency1.Code };
            int baseCurrencyIdx = (trxn.BaseCurrency.HasValue) ? trxn.BaseCurrency.Value : 0;
            int dealtCurrencyIdx = (trxn.DealtCurrency.HasValue && trxn.DealtCurrency.Value > 0) ? 1 - baseCurrencyIdx : baseCurrencyIdx;
            fwd.Element("BASE_CURRENCY").SetValue(currencyArr[baseCurrencyIdx]);
            fwd.Element("TERM_CURRENCY").SetValue(currencyArr[1 - baseCurrencyIdx]);
            fwd.Element("TAKER_BUYS_BASE").SetValue((baseCurrencyIdx == 0) ? "TRUE" : "FALSE");
            // Dealt currency is the same as Base currency
            fwd.Element("DEALT_CURRENCY").SetValue(currencyArr[dealtCurrencyIdx]);            
            // fwd.Element("DEALT_CURRENCY").SetValue(trxn.TransactionLegs[0].Currency1.Code);
            fwd.Element("RATE").SetValue(trxn.TransactionLegs[0].Rate.Value);
            fwd.Element("AMOUNT").SetValue(context.GetTransactionAmount(trxn));
            fwd.Element("VALUE_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            // fwd.Element("SPOT_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            fwd.Element("SPOT_DATE").SetValue(trxn.spotdate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            if (trxn.TransactionLegs[0].ForwardPoints.HasValue)
            {
                fwd.Element("FORWARD_POINTS").SetValue(trxn.TransactionLegs[0].ForwardPoints.Value);
                fwd.Element("FORWARD_SCALE_DECIMAL_PLACES").SetValue(trxn.Scale.HasValue ? trxn.Scale.Value : 4);
            }
        }

        private void PopulateNDFPortion(TMB.Data.Transaction trxn, XElement deal)
        {
            var ndf = deal.Element("FX_NDF_DETAILS");
            string[] currencyArr = new string[] { trxn.TransactionLegs[0].Currency.Code, trxn.TransactionLegs[0].Currency1.Code };
            int baseCurrencyIdx = (trxn.BaseCurrency.HasValue) ? trxn.BaseCurrency.Value : 0;
            int dealtCurrencyIdx = (trxn.DealtCurrency.HasValue && trxn.DealtCurrency.Value > 0) ? 1 - baseCurrencyIdx : baseCurrencyIdx;
            ndf.Element("AMOUNT").SetValue(context.GetTransactionAmount(trxn));
            ndf.Element("BASE_CURRENCY").SetValue(currencyArr[baseCurrencyIdx]);
            // Dealt currency is the same as Base currency
            ndf.Element("DEALT_CURRENCY").SetValue(currencyArr[dealtCurrencyIdx]);
            ndf.Element("FIXING_CENTRE").SetValue(trxn.TransactionFixes.FixingCentre.Code);
            ndf.Element("FIXING_DATE").SetValue(trxn.TransactionFixes.FixingDate.Date.ToString("yyyy-MM-dd HH:mm:ss"));
            ndf.Element("FIXING_DAYS").SetValue(trxn.TransactionFixes.FixingDays);
            ndf.Element("FIXING_SETTLEMENT_CURRENCY").SetValue(trxn.TransactionFixes.Currency.Code);
            ndf.Element("FIXING_TEXT").SetValue(trxn.Message);

            ndf.Element("FORWARD_POINTS").SetValue(trxn.TransactionLegs[0].ForwardPoints.Value);
            ndf.Element("FORWARD_SCALE_DECIMAL_PLACES").SetValue(trxn.Scale.HasValue ? trxn.Scale.Value : 4);

            ndf.Element("RATE").SetValue(trxn.TransactionLegs[0].Rate.Value);
            ndf.Element("SPOT_DATE").SetValue(trxn.spotdate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            ndf.Element("TERM_CURRENCY").SetValue(currencyArr[1 - baseCurrencyIdx]);
            ndf.Element("TAKER_BUYS_BASE").SetValue((baseCurrencyIdx == 0) ? "TRUE" : "FALSE");
            ndf.Element("VALUE_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void PopulateMMDepoPortion(TMB.Data.Transaction trxn, XElement deal)
        {
            var mm = deal.Element("MM_DEPO_DETAILS");
            mm.Element("AMOUNT").SetValue(context.GetTransactionAmount(trxn));
            mm.Element("BASIS").SetValue(trxn.TransactionMMs.DaysPerYear.Value); // Days/Year Basis
            mm.Element("CURRENCY").SetValue(trxn.TransactionLegs[0].Currency.Code);
            mm.Element("INTEREST_AMOUNT").SetValue(trxn.TransactionMMs.InterestAmount.Value);
            mm.Element("MATURITY_DATE").SetValue(trxn.spotdate.Value.ToUniversalTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
            mm.Element("RATE").SetValue(trxn.TransactionLegs[0].Rate.Value);
            mm.Element("VALUE_DATE").SetValue(trxn.TransactionLegs[0].ActionDate.Value.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        }


        private RTNSResponseMessage SubmitDealToRTNS(int trxnID, 
            XDocument doc)
        {
            int logRequestID = 0;
            string rawresponse = string.Empty;
            RTNSResponseMessage responseMsg = null;

            try
            {
                string s = doc.ToString(SaveOptions.DisableFormatting);
                logRequestID = LogRequest(trxnID, s);
                byte[] arr = Encoding.ASCII.GetBytes(s);
                ns.Write(arr, 0, arr.Length);

                
                List<string> commandList = readCommandFromStream(ns, 5,true);
                foreach (string cmd in commandList)
                    rawresponse += cmd;
                //rawresponse = commandList[0];
                responseMsg = new RTNSResponseMessage(rawresponse);
                LogResponse(trxnID, logRequestID, responseMsg.RTNSResponseStatus, rawresponse, responseMsg.ErrorName);
            }
            catch (Exception ex)
            {
                string errorReason =
                    ((rawresponse == null) || (rawresponse.Trim() == string.Empty))
                    ? "NO RESPONSE FROM RTNS"
                    : "NON RTNS INTERNAL ERROR";
                responseMsg = new RTNSResponseMessage(false, errorReason, ex.ToString());
                LogResponse(trxnID, logRequestID, responseMsg.RTNSResponseStatus, rawresponse, ex.ToString());
            }
            return responseMsg;
        }

        List<string> confirmedStatuses = new List<string>() { "UNKNOWN", "CONFIRMED" };
        private void ProcessSubscribtionMessage(int logID, string commandString)
        {
            int newStatus = 0;
            string subStatus = string.Empty;
            // Find the transaction
            XDocument doc = XDocument.Parse(commandString);
            var msgRoot = (doc.Element("RWP_1").Element("UPDATED_DEALS") != null) ?
                    doc.Element("RWP_1").Element("UPDATED_DEALS") :
                    doc.Element("RWP_1").Element("CACHED_DEALS");

            // Handle the status update 
            foreach (var deal in msgRoot.Elements("DEAL"))
            {
                try
                {
                    string id = deal.Element("ID").Value;
                    string makerStatus = deal.Element("MAKER_STATUS").Value;
                    string takerStatus = deal.Element("TAKER_STATUS").Value;
                    bool closeDeal = false;

                    // PENDING, 
                    // if (makerStatus.Equals("CONFIRMED") && takerStatus.Equals("CONFIRMED"))
                    if ((confirmedStatuses.IndexOf(makerStatus) >= 0) && (confirmedStatuses.IndexOf(takerStatus) >= 0))
                    {
                        newStatus = 10;
                        closeDeal = true;
                    }
                    else if (makerStatus.Equals("PENDING") || takerStatus.Equals("PENDING"))
                    {
                        newStatus = 8;
                        closeDeal = false;
                    }
                    else if (makerStatus.Equals("CANCELLED") || takerStatus.Equals("CANCELLED"))
                    {
                        newStatus = 9;
                        closeDeal = true;
                    }
                    else if (makerStatus.Contains("REJECTED") || takerStatus.Contains("REJECTED"))
                    {
                        newStatus = 11;
                        subStatus = string.Empty;
                        closeDeal = false;
                        // subStatus = makerStatus.Replace("REJECTED:", string.Empty);
                        //Add rejection reasons as received from maker&taker. Remember Maker == Seller, Taker == Buyer
                        if (makerStatus.Contains("REJECTED"))
                            subStatus += "Seller: " + makerStatus.Replace("REJECTED:", string.Empty) + ". ";
                        if (takerStatus.Contains("REJECTED"))
                            subStatus += "Buyer: " + takerStatus.Replace("REJECTED:", string.Empty);
                    }

                    if (newStatus > 0)
                    {
                        var trxn = context
                            .Transactions
                            .Where(t => t.TransactionReference == id)
                            .FirstOrDefault();
                        
                        context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, trxn);

                        trxn.Status1 = context.Status.Where(s => s.ID == newStatus).FirstOrDefault();
                        trxn.SubStatus = subStatus;

                        if (closeDeal)
                        {
                            CloseDeal(trxn);
                        }

                        context.SubmitChanges();
                    }
                }
                catch (Exception ex)
                {
                    // Error occured processing subscribtion response.
                }
            }
            // Save
            
            // var deal = msgRoot.Element("DEAL");

            
        }
        #endregion

        #region REFERENCE DATA
        public void GetReferenceData()
        {
            try
            {
                int numBytesRead;
                string buffer = string.Empty;
                
                byte[] arr = Encoding.ASCII.GetBytes(commandTable["GET_REF_DATA"]);
                ns.Write(arr, 0, arr.Length);

                byte[] readBuffer = new byte[1024];
                if ((ns.CanRead && ns.DataAvailable))
                {                    
                    while (ns.DataAvailable)
                    {
                        numBytesRead = ns.Read(readBuffer, 0, readBuffer.Length);
                        buffer += Encoding.ASCII.GetString(readBuffer, 0, numBytesRead);
                    }

                    XDocument doc = XDocument.Parse(buffer);
                    if (doc.Elements("RWP_1").Elements("RESULT").Elements("STATUS").FirstOrDefault().Value == "SUCCESS")
                    {

                    }

                    // Load the document from the response stream.                                        
                }
            }
            catch (Exception ex)
            {
                OnConnectionError(EventArgs.Empty);
            }
        }
        #endregion

        #region LOGGING
        public int LogRequest(int trxnID, string message)
        {
            int logID = 0;
            Data.RequestLog logEntry = new Data.RequestLog();
            logEntry.TransactionID = trxnID;
            logEntry.RequestDate = DateTime.Now;
            logEntry.RequestMessage = message;
            context.RequestLogs.InsertOnSubmit(logEntry);
            context.SubmitChanges();

            logID = logEntry.ID;
            return logID;
        }

        public void LogResponse(int trxnID, int requestID, string responsestatus, string message, string errorMessage)
        {
            Data.ResponseLog logEntry = new Data.ResponseLog();
            logEntry.TransactionID = trxnID;
            logEntry.RequestID = requestID;
            logEntry.ResponseDate = DateTime.Now;
            logEntry.ResponseMessage = message;
            logEntry.ResponseStatus = responsestatus;
            logEntry.ErrorMessage = errorMessage;
            context.ResponseLogs.InsertOnSubmit(logEntry);
            context.SubmitChanges();
        }

        public int LogSubscribtionMessage(string message)
        {
            Data.SubscribtionLog logEntry = new Data.SubscribtionLog();
            logEntry.DateReceived = DateTime.Now;
            logEntry.Message = message;
            logEntry.Status = "RCVD";

            context.SubscribtionLogs.InsertOnSubmit(logEntry);
            context.SubmitChanges();

            return logEntry.ID;
        }
        #endregion
    }

    public enum RTNSConnectionState
    {
        OK,
        Warning,
        Error
    }

    public class SubscribtionEventArgs : EventArgs 
    {
        protected string responseString;

        public SubscribtionEventArgs(string responseString)
            :base()
        {
            this.responseString = responseString;
        }

        public string ResponseString
        {
            get { return responseString; }
        }
    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;


namespace TMB.Reuters
{
    public class RTNSReferenceFile
    {
        private string docfilePath;
        private string adminUrl;
        private string adminUsername;
        private string adminPwd;
        private XDocument document;
        private XElement root;
        private XElement receivers;
        private Data.TMBDataContext context;

        public RTNSReferenceFile(string filePath, string adminUrl, string username, string pwd, Data.TMBDataContext datacontext)
        {
            this.context = datacontext;

            if (File.Exists(filePath))
            {
                docfilePath = filePath;
                document = XDocument.Load(filePath);
                root = document.Element("REFERENCE_DATA");
                receivers = root.Element("RECEIVERS");
            }
            else
                throw new Exception(string.Format("File {0} does not exist", filePath));

            this.adminUrl = adminUrl;
            this.adminUsername = username;
            this.adminPwd = pwd;
        }

        public bool AddReceiver(Data.Bank receiver)
        {
            if (receivers != null)
            {

            }

            return true;
        }

        public bool UpdateReceiver(Data.Bank receiver)
        {
            if (receivers != null)
            {
                var q = from r in receivers.Descendants("RECEIVER")
                        where r.Element("GROUP_INTERNAL_ID").Value == receiver.InternalID
                        select r;

                if ((q != null) && (q.Count() > 0))
                {
                    // Populate existing receiver(s)
                    foreach (var r in q)
                        PopulateReceiver(r, receiver);
                }
                else
                {
                    // Create a new receiver
                    XElement newReceiver = GetNewReceiver();
                    receivers.Add(newReceiver);
                    PopulateReceiver(newReceiver, receiver);
                }
            }
            
            return Save();
        }

        public bool RemoveReceiver(Data.Bank receiver)
        {
            bool bSuccess = false;
            var q = from r in receivers.Descendants("RECEIVER")
                    where r.Element("GROUP_INTERNAL_ID").Value == receiver.InternalID
                    select r;

            if ((q != null) && (q.Count() > 0))            
                q.Remove();

            // RefreshAdminUrl();

            return Save();
        }

        private bool Save()
        {
            bool bSuccess = false;
            if (File.Exists(docfilePath))
            {
                try
                {
                    File.Copy(docfilePath, docfilePath + ".BACKUP_" + DateTime.Now.ToString("yyyyMMddHHmmss"), true);
                    document.Save(docfilePath);

                    // docfilePath
                    // DO AN HTTP POST to the URL to reload the configuration                    
                    RefreshAdminUrl();

                    bSuccess = true;
                }
                catch (Exception ex)
                {
                    File.WriteAllLines("error.log", new string[] { ex.ToString() });
                    bSuccess = false;
                }
            }
            return bSuccess;
        }

        private void RefreshAdminUrl()
        {
            string URI = adminUrl + "/referenceController.jsp?action=reload";
            WebClient webClient = new WebClient();
            webClient.Credentials = new NetworkCredential(adminUsername, adminPwd);
            StreamReader reader = new StreamReader(webClient.OpenRead(URI));
            String request = reader.ReadToEnd();
        }

        private XElement GetNewReceiver()
        {
            string receiver = @"<RECEIVER>
	                            <GROUP_INTERNAL_ID>NEW</GROUP_INTERNAL_ID>
	                            <GROUP_FULL_NAME>NEW</GROUP_FULL_NAME>
	                            <SFN_GROUP_NAME>NEW</SFN_GROUP_NAME>
	                            <FDC_GROUP_NAME>NEW</FDC_GROUP_NAME>
	                            <INSTRUMENT_ROUTING_CODES>
	                            <INSTRUMENT_ROUTING_CODE>
		                            <INSTRUMENT>DEFAULT</INSTRUMENT>
		                            <FDC_ROUTING_CODE>NEW</FDC_ROUTING_CODE>
		                            <SFN_ROUTING_CODE>NEWSFN</SFN_ROUTING_CODE>
	                            </INSTRUMENT_ROUTING_CODE>
	                            </INSTRUMENT_ROUTING_CODES>
	                            <USERS />                
                            </RECEIVER>";
            return XElement.Parse(receiver);
        }
        private XElement GetNewUser()
        {
            string user = @"<USER>
	                            <INTERNAL_ID>CITI_NOSTP</INTERNAL_ID>
                                    <FULL_NAME>CITI_NOSTP</FULL_NAME>
	                                <NAME>citi@nostp.co.za</NAME>
                                    <ID>CITI_NOSTP</ID>
                                    <MANUAL_TYPE>NOSTP</MANUAL_TYPE>
                                    <ELECTRONIC_TYPE>NOSTP</ELECTRONIC_TYPE>
                            </USER>";

            return XElement.Parse(user);
        }

        private void PopulateReceiver(XElement r, Data.Bank receiver) 
        {
            r.Element("GROUP_INTERNAL_ID").SetValue(receiver.InternalID);
            r.Element("GROUP_FULL_NAME").SetValue(receiver.Name);
            r.Element("SFN_GROUP_NAME").SetValue(receiver.SFNName);
            r.Element("FDC_GROUP_NAME").SetValue(receiver.FDCName);
            r.Element("INSTRUMENT_ROUTING_CODES").Element("INSTRUMENT_ROUTING_CODE").Element("INSTRUMENT").SetValue("DEFAULT");
            r.Element("INSTRUMENT_ROUTING_CODES").Element("INSTRUMENT_ROUTING_CODE").Element("FDC_ROUTING_CODE").SetValue(receiver.FDCName);
            r.Element("INSTRUMENT_ROUTING_CODES").Element("INSTRUMENT_ROUTING_CODE").Element("SFN_ROUTING_CODE").SetValue(receiver.SFNName);

            var receiverUsers = r.Element("USERS");
            foreach (var bankUser in receiver.BankUsers)
            {
                var receiverUser = from ru in receiverUsers.Descendants("USER")
                                   where ru.Element("INTERNAL_ID").Value == bankUser.InternalID
                                   select ru;

                if ((receiverUser != null) && (receiverUser.Count() > 0))
                {
                    // Populate existing users
                    foreach (var ru in receiverUser)
                        PopulateUser(ru, bankUser);
                }
                else
                {
                    // create new user
                    XElement newUser = GetNewUser();
                    receiverUsers.Add(newUser);
                    PopulateUser(newUser,bankUser);
                }
            }
        }

        private void PopulateUser(XElement ru, Data.BankUser bankUser)
        {
            ru.Element("INTERNAL_ID").SetValue(bankUser.InternalID);
            ru.Element("FULL_NAME").SetValue(bankUser.FullName);
            ru.Element("NAME").SetValue(bankUser.Name);
            ru.Element("ID").SetValue(bankUser.InternalID);
            ru.Element("MANUAL_TYPE").SetValue(GetRTNSNotificationType(bankUser.ManualType));
            ru.Element("ELECTRONIC_TYPE").SetValue(GetRTNSNotificationType(bankUser.ElectronicType));
        }

        private string GetRTNSNotificationType(int? notificationTypeId)
        {
            string response = "NOSTP";
            if (notificationTypeId.HasValue)
            {
                var notificationType = context.RTNSNotificationTypes
                    .Where(w => w.ID == notificationTypeId.Value);

                response = notificationType.FirstOrDefault().Name;
            }
            return response;
        }
    }
}

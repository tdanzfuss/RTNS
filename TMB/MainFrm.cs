using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using TMB.Reuters;

namespace TMB
{
    public partial class frmMain : Form
    {
        private string UserInternalID;
        public string UserDisplayName;
        public string BrokerName;
        private Reuters.RTNSConnection connection;
        private Controls.Admin.ReceiversListControl receiversList;
        private Controls.Admin.CurrencyListControl currencyList;
        private Controls.Admin.UserListControl userList;
        private Controls.Admin.TermDateControl termDateControl;
        private Controls.Admin.CurrencyScale currencyScaleControl;
        private Controls.Admin.FixingCentersListControl fixingCenterListControl;
        private Controls.ReportListControl reportList;
        public static string StaticUserDisplayName;

        public frmMain()
        {
            InitializeComponent();
            receiversList = new TMB.Controls.Admin.ReceiversListControl();
            currencyList = new TMB.Controls.Admin.CurrencyListControl();
            userList = new TMB.Controls.Admin.UserListControl();
            reportList = new TMB.Controls.ReportListControl();
            termDateControl = new TMB.Controls.Admin.TermDateControl();
            currencyScaleControl = new TMB.Controls.Admin.CurrencyScale();
            fixingCenterListControl = new TMB.Controls.Admin.FixingCentersListControl();

            SetConnectionState(RTNSConnectionState.Error, "Not Connected");            
        }

        public frmMain(string userInternalID, string userDisplayName)
            : this()
        {
            UserInternalID = userInternalID;
            UserDisplayName = userDisplayName;
            StaticUserDisplayName = userDisplayName;
            BrokerName = ConfigurationManager.AppSettings["broker"];
        }
         
        private void CreateNewDeal(int productID)
        {
            using (PopupForm frm = new PopupForm())
            {
                TMB.Controls.DealDetailControl ctrl = new TMB.Controls.DealDetailControl(productID);
                ctrl.Connection = this.connection;
                ctrl.ParentControl = this;
                // frm.Controls.Add(new TMB.Controls.DealDetailControl());
                frm.AddControl(ctrl, true);                
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    dealListControl1.RefreshList();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Exit ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }

        private void lblDeals_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Display the Deal List Controls
            TMB.Controls.DealListControl ctrl = new TMB.Controls.DealListControl();
            ctrl.ParentControl = this;
            ctrl.Connection = connection;
            ctrl.RefreshList();
            //this.pictureBox1.BackgroundImage = ;
            SwitchControl(ctrl, global::TMB.Properties.Resources.coins, "Deal Listing");
        }

        private void SwitchControl(UserControl ctrl, Bitmap headerImage, string headerText)
        {
            // splitContainer1.Panel2.Controls.Clear();
            // splitContainer1.Panel2.Controls.Add(ctrl);
            this.pictureBox1.BackgroundImage = headerImage;
            this.lblHeaderText.Text = headerText;
            
            splitContainer2.Panel2.Controls.Clear();
            splitContainer2.Panel2.Controls.Add(ctrl);

            ctrl.Dock = DockStyle.Fill;
        }

        public void SetConnectionState(RTNSConnectionState state, string message)
        {
            // Threadsafe update
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    SetUI(state, message);
                }));
            }
            else
                SetUI(state, message);
        }

        private void SetUI(RTNSConnectionState state, string message)
        {
            // View nothing
            imgOK.Visible = imgWarning.Visible = imgError.Visible = false;

            // Depending on state view one...
            switch (state)
            {
                case RTNSConnectionState.OK:
                    imgOK.Visible = true;
                    break;
                case RTNSConnectionState.Warning:
                    imgWarning.Visible = true;
                    break;
                case RTNSConnectionState.Error:
                    imgError.Visible = true;
                    break;
            }

            if (message != null)
                lblConnectionStatus.Text = message;
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            connection = new Reuters.RTNSConnection(
                ConfigurationManager.AppSettings["IP"],
                Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                Convert.ToInt32(ConfigurationManager.AppSettings["SubscribtionPort"]),
                ConfigurationManager.AppSettings["username"],
                ConfigurationManager.AppSettings["password"],
                Convert.ToInt32(ConfigurationManager.AppSettings["PollInterval"]),
                ConfigurationManager.ConnectionStrings["TMB.Properties.Settings.TMBConnectionString"].ConnectionString,
                ConfigurationManager.AppSettings["Version"],
                Convert.ToInt32(ConfigurationManager.AppSettings["RTNSTimeout"]),
                Convert.ToInt32(ConfigurationManager.AppSettings["RTNSRetryInternal"]));

            connection.ConnectionStatusChange += new EventHandler(connection_ConnectionStatusChange);
            connection.SubscribtionEvent += new EventHandler(connection_SubscribtionEvent);
            connection.Connect();

            // Set the connection to the control.
            dealListControl1.Connection = connection;
            dealListControl1.ParentControl = this;            

        }

        void connection_SubscribtionEvent(object sender, EventArgs e)
        {
            SubscribtionEventArgs args = (SubscribtionEventArgs)e;
            // If we get a subscribtion message, update the view
            if ( dealListControl1!=null )
                dealListControl1.RefreshList();
            //MessageBox.Show(args.ResponseString);
            // throw new NotImplementedException();
        }

        void connection_ConnectionStatusChange(object sender, EventArgs e)
        {
            SetConnectionState(
                ((RTNSConnection)sender).ConnectionState,
                ((RTNSConnection)sender).RTNSConnectionMessage);
        }

        private void lblAdministration_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Lets get the Administration data from the publisher adapter
            connection.GetReferenceData();
        }

        private void fXSpotToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // New FX Spot deal
            CreateNewDeal(1);
        }

        private void fXSwapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // New FX Forward deal
            CreateNewDeal(3);
        }

        private void fXForwardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // New FX Swap deal
            CreateNewDeal(2);
        }

        private void fxNDFToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // New NDF deal
            CreateNewDeal(4);
        }

        private void mmDepoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // New MM Deposit deal
            CreateNewDeal(5);
        }

        private void showSubMenuItems(int menuTypeID)
        {
            panel1.Visible = false;
            pnlAdministrationLinks.Visible = false;

            if (menuTypeID == 1)
            {
                panel1.Visible = true;

                pnlAdministrationLinks.Dock = DockStyle.Fill;
                pnlAdministrationLinks.Visible = true;
            }
        }

        #region Menu item click events
        private void btnDeals_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            SwitchControl(this.dealListControl1, global::TMB.Properties.Resources.coins, "Deal Listing");
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            reportList.RefreshList();
            SwitchControl(reportList, global::TMB.Properties.Resources.invoice, "Reports");
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            showSubMenuItems(1);
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            SwitchControl(userList, global::TMB.Properties.Resources.User, "Manage Users");
        }

        private void btnCurrencies_Click(object sender, EventArgs e)
        {
            SwitchControl(currencyList, global::TMB.Properties.Resources.dollar, "Manage Currencies");
        }

        private void btnFixingCenters_Click(object sender, EventArgs e)
        {
            SwitchControl(fixingCenterListControl, global::TMB.Properties.Resources.dollar, "Manage Fixing Centers");
        }
        private void btnReceivers_Click(object sender, EventArgs e)
        {
            SwitchControl(receiversList, global::TMB.Properties.Resources.Receivers, "Manage Receivers");
        }

        private void btnTermDates_Click(object sender, EventArgs e)
        {
            SwitchControl(termDateControl, global::TMB.Properties.Resources.Settings, "Configure Term Dates");
        }
        private void btnCurrencyScales_Click(object sender, EventArgs e)
        {
            SwitchControl(currencyScaleControl, global::TMB.Properties.Resources.money, "Configure Currency Scales");
        }
        #endregion        

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Backup the database...

        }
      

    }
}

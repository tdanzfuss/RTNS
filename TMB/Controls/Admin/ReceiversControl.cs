using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace TMB.Controls.Admin
{
    public partial class ReceiversControl : UserControl, IPopupFormControl
    {
        private Data.TMBDataContext context;
        private int? receiverID;
        private Data.Bank receiver;
        private Reuters.RTNSReferenceFile rtnsconfig;

        public ReceiversControl()
        {
            InitializeComponent();
            context = new Data.TMBDataContext();
            
            try
            {
                rtnsconfig = new Reuters.RTNSReferenceFile(ConfigurationManager.AppSettings["publisherConfigFile"],
                    ConfigurationManager.AppSettings["publisherAdminUrl"],
                    ConfigurationManager.AppSettings["publisherUsername"],
                    ConfigurationManager.AppSettings["publisherPassword"],
                    context);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cant find RTNS config file","Configuration Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void ReceiversControl_Load(object sender, EventArgs e)
        {
            // This is a new receiver
            if (!receiverID.HasValue)
            {
                receiver = new Data.Bank();
                receiver.Status = 1;
                context.Banks.InsertOnSubmit(receiver);
            }

            bankBindingSource.DataSource = receiver;
            gvBankUsers.DataSource = receiver.BankUsers;
            rTNSNotificationTypeBindingSource.DataSource = context.RTNSNotificationTypes.ToList();
            rTNSNotificationTypeBindingSource1.DataSource = context.RTNSNotificationTypes.ToList();
        }

        public int? ReceiverID
        {
            get
            {
                return receiverID.Value;
            }
            set
            {
                receiverID = value;
                receiver = context
                    .Banks
                    .Where(b => b.ID == value)
                    .FirstOrDefault();
            }
        }



        public bool SaveControl()
        {
            bool bSuccess = true;
            try
            {
                context.SubmitChanges();
                
                // Update the Reference File
                rtnsconfig.UpdateReceiver(receiver);
            }
            catch
            {
                bSuccess = false;
            }
            return bSuccess;
        }

        public void CancelControl()
        {
            
        }

        public string HeadingText
        {
            get { return "Manage Receivers"; }
        }

        private void txtShortCode_Leave(object sender, EventArgs e)
        {
            string txt = txtShortCode.Text;
            // Automatically populate the Reuters fields
            txtInternalID.Text = txt;
            txtSFNName.Text = txt + "SFN";
            txtFDCName.Text = txt;
        }
    }
}

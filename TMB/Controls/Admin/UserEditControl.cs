using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TMB.Controls.Admin
{
    public partial class UserEditControl : UserControl, IPopupFormControl
    {
        private Data.TMBDataContext context;
        private int? userID;
        private Data.User user;

        public UserEditControl()
        {
            InitializeComponent();
            context = new Data.TMBDataContext();
            // rtnsconfig = new Reuters.RTNSReferenceFile(ConfigurationManager.AppSettings["publisherConfigFile"]);
        }

        public int? UserID
        {
            get { return userID; }
            set
            {
                userID = value;
                user = context
                    .Users
                    .Where(u => u.ID == value)
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
                // rtnsconfig.UpdateReceiver(receiver);
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
            get { return "Manage Users"; }
        }

        private void UserEditControl_Load(object sender, EventArgs e)
        {
            // This is a new receiver
            if (!userID.HasValue)
            {
                user = new Data.User();
                user.Status = 1;
                context.Users.InsertOnSubmit(user);
            }

            userBindingSource.DataSource = user;
            txtPasswordRetype.Text = txtPassword.Text;
        }
    }
}

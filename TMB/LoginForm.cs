using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TMB
{
    public partial class LoginForm : Form
    {

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Login())
            {
                lblErrorMessage.Visible = false;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                lblErrorMessage.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        public int UserID
        {
            get;
            set;
        }

        public string UserInternalID
        {
            get;
            set;
        }

        public string UserDisplayName
        {
            get;
            set;
        }

        private bool Login()
        {
            TMB.Data.TMBDataContext context = new Data.TMBDataContext();
            var usr = context.Users
                       .Where(t => t.Username == txtUserName.Text && t.Password == txtPassword.Text)
                       .Select(t => t);
            if (usr.Count() > 0)
            {
                UserID = usr.First().ID;
                UserInternalID = usr.First().InternalID;
                UserDisplayName = usr.First().Username;
                return true;
            }

            return false;
        }
    }
}

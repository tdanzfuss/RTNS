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
    public partial class UserListControl : UserControl
    {
        private Data.TMBDataContext context;
        private UserEditControl editCtrl;
        private PopupForm popup;

        public UserListControl()
        {
            InitializeComponent();

            context = new Data.TMBDataContext();
            editCtrl = new UserEditControl();
            popup = new PopupForm();

            popup.AddControl(editCtrl, false);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //
            if (gvUsers.SelectedRows.Count > 0)
            {
                lnkEdit.Enabled = true;
                lnkRemove.Enabled = true;
            }
        }

        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //
            // This is a new Receiver
            editCtrl.UserID = null;
            if (popup.ShowDialog() == DialogResult.OK)
                RefreshList();
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // View the detail of a deal
            if (gvUsers.SelectedRows.Count > 0)
            {
                editCtrl.UserID = Convert.ToInt32(gvUsers.SelectedRows[0].Cells[0].Value);

                if (popup.ShowDialog() == DialogResult.OK)
                    RefreshList();
            }
        }

        private void lnkRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // View the detail of a deal
            if (gvUsers.SelectedRows.Count > 0)
            {
                int userID = Convert.ToInt32(gvUsers.SelectedRows[0].Cells["iDDataGridViewTextBoxColumn"].Value);
                string userName = Convert.ToString(gvUsers.SelectedRows[0].Cells["usernameDataGridViewTextBoxColumn"].Value);
                if (MessageBox.Show(string.Format("Are you sure you want to delete {0} from the list of Users?", userName)
                    , "Remove User"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Data.User selecteduser = context
                        .Users
                        .Where(r => r.ID == userID)
                        .FirstOrDefault();

                    selecteduser.Status = 0;
                    context.SubmitChanges();
                }
           
                RefreshList();
            }
        }

        private void UserListControl_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void RefreshList()
        {            
            var users = context
                .Users
                .Where(u => u.Status == 1)
                .ToList();
            
            context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, users);            

            userBindingSource.DataSource = users;
            gvUsers.Refresh();
        }
    }
}

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
    public partial class ReceiversListControl : UserControl
    {
        private Data.TMBDataContext context;
        private ReceiversControl editCtrl;
        private PopupForm popup;
        private Reuters.RTNSReferenceFile rtnsconfig;

        public ReceiversListControl()
        {
            InitializeComponent();
            context = new Data.TMBDataContext();
            editCtrl = new ReceiversControl();
            rtnsconfig = new Reuters.RTNSReferenceFile(ConfigurationManager.AppSettings["publisherConfigFile"],
                    ConfigurationManager.AppSettings["publisherAdminUrl"],
                    ConfigurationManager.AppSettings["publisherUsername"],
                    ConfigurationManager.AppSettings["publisherPassword"],
                    context);
            popup = new PopupForm();

            popup.AddControl(editCtrl, false);
        }

        private void gvBanks_SelectionChanged(object sender, EventArgs e)
        {
            if (gvBanks.SelectedRows.Count > 0)
            {
                lblEdit.Enabled = true;
                lblRemove.Enabled = true;
            }
        }

        private void lblEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // View the detail of a deal
            if (gvBanks.SelectedRows.Count > 0)
            {
                editCtrl.ReceiverID = Convert.ToInt32(gvBanks.SelectedRows[0].Cells[0].Value);

                if (popup.ShowDialog() == DialogResult.OK)
                    RefreshList();
            }
        }

        private void ReceiversListControl_Load(object sender, EventArgs e)
        {
            // Load receivers list
            RefreshList();
        }

        public void RefreshList()
        {
            var banks = context
                .Banks
                .Where(b => b.Status == 1)
                .OrderBy(b => b.Name)
                .ToList();

            context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, banks);

            bankBindingSource.DataSource = banks;
            gvBanks.Refresh();
        }

        private void lblAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // This is a new Receiver
            editCtrl.ReceiverID = null;
            if (popup.ShowDialog() == DialogResult.OK)
                RefreshList();
        }

        private void lblRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // View the detail of a deal
            if (gvBanks.SelectedRows.Count > 0)
            {
                int receiverID = Convert.ToInt32(gvBanks.SelectedRows[0].Cells["ID"].Value);
                string receiverName = Convert.ToString(gvBanks.SelectedRows[0].Cells["BankName2"].Value);
                if (MessageBox.Show(string.Format("Are you sure you want to delete {0} from the list of Receivers?", receiverName)
                    , "Remove Receiver"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Data.Bank selectedReceiver = context
                        .Banks
                        .Where(r => r.ID == receiverID)
                        .FirstOrDefault();

                    selectedReceiver.Status = 0;
                    context.SubmitChanges();

                    rtnsconfig.RemoveReceiver(selectedReceiver);
                }

                RefreshList();
            }
        }
    }
}

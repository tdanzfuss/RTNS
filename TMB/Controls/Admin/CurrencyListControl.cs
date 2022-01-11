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
    public partial class CurrencyListControl : UserControl
    {
        private Data.TMBDataContext context;
        private CurrencyControl editCtrl;
        private PopupForm popup;

        public CurrencyListControl()
        {
            InitializeComponent();
            context = new Data.TMBDataContext();

            editCtrl = new CurrencyControl();
            popup = new PopupForm();

            popup.AddControl(editCtrl, false);
        }

        private void CurrencyListControl_Load(object sender, EventArgs e)
        {
            //
            RefreshList();
            
        }

        public void RefreshList()
        {
            var currencies = context
                .Currencies
                .Where(c => c.Status == 1)
                .ToList();

            context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, currencies);

            currencyBindingSource.DataSource = currencies;
            gvCurrency.Refresh();
        }

        private void gvCurrency_SelectionChanged(object sender, EventArgs e)
        {
            if (gvCurrency.SelectedRows.Count > 0)
            {
                lnkEdit.Enabled = true;
                lnkDelete.Enabled = true;
            }
        }

        private void lnkAdd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Show EditCurrency Control
            // This is a new Receiver
            editCtrl.CurrencyID = null;
            if (popup.ShowDialog() == DialogResult.OK)
                RefreshList();
        }

        private void lnkEdit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // View the detail of a deal
            if (gvCurrency.SelectedRows.Count > 0)
            {
                editCtrl.CurrencyID = Convert.ToInt32(gvCurrency.SelectedRows[0].Cells[0].Value);

                if (popup.ShowDialog() == DialogResult.OK)
                    RefreshList();
            }
        }

        private void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            // View the detail of a deal
            if (gvCurrency.SelectedRows.Count > 0)
            {
                int currencyID = Convert.ToInt32(gvCurrency.SelectedRows[0].Cells["iDDataGridViewTextBoxColumn"].Value);
                string currencyName = Convert.ToString(gvCurrency.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"].Value);
                if (MessageBox.Show(string.Format("Are you sure you want to delete {0} from the list of Currencies?", currencyName)
                    , "Remove Currency"
                    , MessageBoxButtons.YesNo
                    , MessageBoxIcon.Question
                    , MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    Data.Currency selectedCurrency = context
                        .Currencies
                        .Where(r => r.ID == currencyID)
                        .FirstOrDefault();

                    selectedCurrency.Status = 0;
                    context.SubmitChanges();
                }

                RefreshList();
            }
        }

    }
}

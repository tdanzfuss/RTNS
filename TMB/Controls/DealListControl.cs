using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using TMB.Data;
using TMB.Reuters;

namespace TMB.Controls
{
    public partial class DealListControl : UserControl
    {
        private TMBDataContext context;
        private Reuters.RTNSConnection connection;        

        public DealListControl()
        {
            context = new TMBDataContext();
            InitializeComponent();            
        }

        public Reuters.RTNSConnection Connection
        {
            get;
            set;
        }

        public frmMain ParentControl
        {
            get;
            set;
        }

        private void DealListControl_Load(object sender, EventArgs e)
        {            
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now.AddDays(1);

            RefreshList();
            // dataGridView1.DataSource = transactionBindingSource;            
        }

        delegate void RefreshListCallback();
        public void RefreshList()
        {
            if (this.InvokeRequired)
            {
                RefreshListCallback d = new RefreshListCallback(RefreshList);
                this.Invoke(d, new object[] { });
            }
            else
            {
                try
                {
                    transactionBindingSource.DataSource = context.Transactions
                        .Where(w => w.Status != 6 && (w.TransactionDate >= dtFrom.Value.Date && w.TransactionDate < dtTo.Value.Date))
                        .Select(w => new { w.ID, w.Description, w.Message, Status = w.Status1.Name, w.TransactionDate, w.TransactionReference })
                        .OrderByDescending(w => w.TransactionDate)
                        .ToList();
                    //statusBindingSource.DataSource = context.Products.ToList();
                    dataGridView1.Refresh();
                }
                catch { }
            }
        }

    

        private void lnkViewDeal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // View the detail of a deal
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedDealID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                DealDetailControl ctrl = new DealDetailControl();
                ctrl.Connection = this.Connection;
                ctrl.ParentControl = this.ParentControl;
                ctrl.DealID = selectedDealID;
                PopupForm frm = new PopupForm();
                frm.AddControl(ctrl,true);
                frm.ShowDialog();
                RefreshList();
            }
        }

        private void lnkTerminate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedDealID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                    if (DialogResult.Yes == MessageBox.Show(string.Format("Are you sure you want to Terminate Deal {0}?", selectedDealID), "Terminate Deal", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        // ((Transaction)dataGridView1.SelectedRows[0].DataBoundItem).Status = 6;
                        Transaction trxn = context.Transactions.Where(w => w.ID == selectedDealID).FirstOrDefault();

                        // Submit the terminate to RTNS
                        RTNSResponseMessage resp = Connection.CancelDeal(trxn);

                        trxn.Status = 6;
                        context.SubmitChanges();
                        RefreshList();
                    }
                }
            }
            catch { }
        }

        private void lblFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RefreshList();
        }

        private void lnkResubmit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Transaction trxn = GetSelectedTransaction();
                if (trxn != null)
                {
                    if (trxn.TransactionReference == string.Empty)
                        trxn.TransactionReference = ConfigurationManager.AppSettings["brokercode"] + "-" + trxn.ID.ToString("D8") + "-" + trxn.TransactionLegs[0].ID.ToString("D8");
                    RTNSResponseMessage response = Connection.UpdateDeal(
                        ParentControl.BrokerName,
                        ParentControl.UserDisplayName,
                        trxn);

                    trxn.RTNSReference = response.Reference;
                    int trxnStatus = (response.Success) ? 3 : 7;
                    trxn.Status1 = context.Status.Where(w => w.ID == trxnStatus).First();
                    context.SubmitChanges();

                    if (!response.Success)
                        MessageBox.Show("Could not submit deal to Reuters. The following error occured " + response.ErrorName, "RTNS Integration");
                }
            }
            catch { }
        }

        public Transaction GetSelectedTransaction()
        {
            Transaction trxn = null;

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedDealID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                trxn = context
                    .Transactions
                    .Where(w => w.ID == selectedDealID)
                    .FirstOrDefault();
            }

            return trxn;
        }
    }
}

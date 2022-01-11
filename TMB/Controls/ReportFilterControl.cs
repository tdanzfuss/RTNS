using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TMB.Data;

namespace TMB.Controls
{
    public partial class ReportFilterControl : UserControl, IPopupFormControl
    {
        private TMBDataContext context;
        public ReportFilterControl()
        {
            InitializeComponent();
            context = new TMBDataContext();
        }

        public string Report
        {
            get { return lblReportName.Text; }
            set { lblReportName.Text = value; }
        }
        public DateTime FromDate
        {
            get
            {
                return dtFromDate.Value.Date;
            }
            set
            {
                dtFromDate.Value = value;
            }
        }
        public DateTime ToDate 
        {
            get
            {
                return dtToDate.Value.Date;
            }
            set
            {
                dtToDate.Value = value;
            }
        }
        public List<Bank> SelectedBanks
        {
            get
            {
                if (chkBanks.CheckedItems.Count <= 0)
                    return context.Banks.ToList();
                else
                {
                    List<Bank> selectedBanks = new List<Bank>();
                    foreach (object o in chkBanks.CheckedItems)                    
                        selectedBanks.Add((Bank)o);
                    
                    //chkBanks.SelectedItems.
                    return selectedBanks;
                }
            }
        }

        private void LoadBankList()
        {
            var bankList = context.Banks.ToList();
            ((ListBox)chkBanks).DataSource = bankList;
            ((ListBox)chkBanks).DisplayMember = "Name";
            ((ListBox)chkBanks).ValueMember = "ID";

            /*foreach (Bank b in bankList)            
                chkBanks.Items.Add(b, false);*/

        }

        private void ReportFilterControl_Load(object sender, EventArgs e)
        {
            LoadBankList();
        }

        public bool SaveControl()
        {
            // If all the parameters are set then display the ReportViewer control.
            // ReportViewerControl ctrl = new ReportViewerControl();
            // ctrl.Report = txtReportName.Text;            
            return true;
        }

        public void CancelControl()
        {
            
        }

        public string HeadingText
        {
            get { return "Parameters for Report: " + lblReportName.Text; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace TMB.Controls
{
    public partial class ReportViewerControl : UserControl, IPopupFormControl
    {        
        private string reportName;
        private System.Collections.Hashtable tblReportPaths;
        
        
        public ReportViewerControl()
        {
            InitializeComponent();            
            tblReportPaths = LoadReportPaths();
        }
        
        public string Report
        {
            get { return reportName; }
            set { reportName = value; }
        }

        public DateTime FromDate
        {
            get;
            set;
        }
        public DateTime ToDate
        {
            get;
            set;
        }
        public List<TMB.Data.Bank> Banks
        {
            get;
            set;
        }

        private System.Collections.Hashtable LoadReportPaths()
        {
            System.Collections.Hashtable tbl = new System.Collections.Hashtable();
            tbl.Add("Daily Deal Confirmation Sheets", new TMB.Reports.DealConfirmationPerBankUserReportData());
            tbl.Add("Deal Ticket Sheet", new TMB.Reports.DealTicketReportData());            
            tbl.Add("Deal Status Report", new TMB.Reports.DealStatusReportData());
            // tbl.Add("Deal Summary Report", "");
            return tbl;
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            if (tblReportPaths.ContainsKey(reportName))
            {
                TMB.Reports.ReportData reportData = (TMB.Reports.ReportData)tblReportPaths[reportName];
                reportViewer1.LocalReport.ReportPath = reportData.ReportPath;
                reportData.LoadData(reportViewer1.LocalReport.DataSources, new List<object> { FromDate,ToDate,Banks });
                reportViewer1.RefreshReport();

                // reportViewer1.LocalReport.DataSources
                // reportViewer1.LocalReport.LoadReportDefinition();
            }
        }

        public bool SaveControl()
        {
            return true;
            //throw new NotImplementedException();
        }

        public void CancelControl()
        {
            //throw new NotImplementedException();
        }

        public string HeadingText
        {
            get { return "View " + Report; }
        }
    }
}

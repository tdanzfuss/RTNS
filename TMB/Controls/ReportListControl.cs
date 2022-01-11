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
    public partial class ReportListControl : UserControl
    {
        private TMBDataContext context;
        // Show the report for the selected Row
        private PopupForm frm; 
        private ReportFilterControl filterControl;
        private ReportViewerControl viewerControl;

        public ReportListControl()
        {
            InitializeComponent();
            frm = new PopupForm();
            context = new TMBDataContext();
            filterControl = new ReportFilterControl();
            viewerControl = new ReportViewerControl();
        }

        public void RefreshList()
        {
            bindingSourceReports.DataSource = context.Reports.OrderBy(w => w.Seq).ToList();
            dataGridView1.Refresh();
        }

        private void ReportListControl_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void lblViewReport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string reportName = Convert.ToString(dataGridView1.SelectedRows[0].Cells[1].Value);
                filterControl.Report = reportName;
                frm.WindowState = FormWindowState.Normal;
                frm.SwitchControl(filterControl, true);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    viewerControl.Report = reportName;
                    viewerControl.FromDate = filterControl.FromDate;
                    viewerControl.ToDate = filterControl.ToDate;
                    viewerControl.Banks = filterControl.SelectedBanks;
                    frm.SwitchControl(viewerControl, true);
                    viewerControl.Dock = DockStyle.Fill;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.ShowDialog();
                }
            }
        }
    }
}

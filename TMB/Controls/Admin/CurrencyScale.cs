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
    public partial class CurrencyScale : UserControl
    {
        private Data.TMBDataContext context;

        public CurrencyScale()
        {
            InitializeComponent();

            context = new Data.TMBDataContext();
        }

        private void CurrencyScale_Load(object sender, EventArgs e)
        {
            currencyScaleBindingSource.DataSource = context.CurrencyScales.ToList();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            context.SubmitChanges();
            MessageBox.Show("Succesfully updated Currency Scales", "Currency Scales", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            var currScales = context.CurrencyScales;
            context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, currScales);
            currencyScaleBindingSource.DataSource = currScales.ToList();
        }
    }
}

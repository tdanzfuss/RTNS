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
    public partial class CurrencyControl : UserControl, IPopupFormControl
    {
        private int? currencyID;
        private Data.Currency currency;
        private Data.TMBDataContext context;

        public CurrencyControl()
        {
            InitializeComponent();
            context = new Data.TMBDataContext();
        }

        private void CurrencyControl_Load(object sender, EventArgs e)
        {
            // This is a new currency
            if (!currencyID.HasValue)
            {
                currency = new Data.Currency();
                currency.Status = 1;
                context.Currencies.InsertOnSubmit(currency);
            }
            currencyBindingSource.DataSource = currency;
            roundingModeBindingSource.DataSource = context.RoundingModes;
        }

        public int? CurrencyID
        {
            get { return currencyID; }
            set
            {
                currencyID = value;
                currency = context
                    .Currencies
                    .Where(c => c.ID == value)
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
            get { return "Manage Currencies"; }
        }
    }
}

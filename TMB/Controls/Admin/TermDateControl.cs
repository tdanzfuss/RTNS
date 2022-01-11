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
    public partial class TermDateControl : UserControl
    {
        private Data.TMBDataContext context;

        public TermDateControl()
        {
            InitializeComponent();

            context = new Data.TMBDataContext();
        }

        private void TermDateControl_Load(object sender, EventArgs e)
        {
            // Load the term dates for today
            RefreshList();
        }

        public void RefreshList()
        {
            var terms = context.Terms.ToList();
            termBindingSource.DataSource = terms;

            var termDates = context
               .TermDates
               .Where(t => t.ConfigDate >= DateTime.Now.Date)
               .ToList();

            if (termDates.Count <= 0)
            {
                CreateDefaultTermDates();

                termDates = context
               .TermDates
               .Where(t => t.ConfigDate >= DateTime.Now.Date)
               .ToList();
            }

            context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, termDates);
            
            termDateBindingSource.DataSource = termDates;
            gvTermDates.Refresh();
        }

        public void CreateDefaultTermDates()
        {
            DateTime configDate = DateTime.Now;
            var terms = context.Terms.Where(t => t.Name != "Custom");
            foreach (var t in terms)
            {
                Data.TermDate newTermDate = new Data.TermDate()
                {
                    ConfigDate = configDate,
                    Term = t,
                    ValueDate = configDate.AddDays(t.Increment.Value)
                };
                context.TermDates.InsertOnSubmit(newTermDate);
            }

            context.SubmitChanges();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            context.SubmitChanges();
            MessageBox.Show("Succesfully updated Term Dates","Term Dates",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}

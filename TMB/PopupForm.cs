using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TMB
{
    public partial class PopupForm : Form
    {
        private TMB.Controls.IPopupFormControl control;
        public PopupForm()
        {
            InitializeComponent();
        }

        public void AddControl(TMB.Controls.IPopupFormControl ctrl, bool resizeToFit)
        {
            control = ctrl;
            splitContainer1.Panel1.Controls.Add((UserControl)ctrl);
            ((UserControl)ctrl).Dock = DockStyle.Fill;
            this.Text = control.HeadingText;
            if (resizeToFit)
            {
                this.Width = ((UserControl)ctrl).Width;
                this.Height = ((UserControl)ctrl).Height+200;
            }
        }

        public void SwitchControl(TMB.Controls.IPopupFormControl ctrl, bool resizeToFit)
        {
            splitContainer1.Panel1.Controls.Clear();
            AddControl(ctrl, resizeToFit);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (control.SaveControl())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                var result = MessageBox.Show("Could not save form, are you sure you want to close?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error,MessageBoxDefaultButton.Button2);
                
                if (result == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.No;
                    this.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            control.CancelControl();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

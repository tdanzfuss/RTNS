using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TMB
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                LoginForm frm = new LoginForm();
                if (frm.ShowDialog() == DialogResult.OK)
                    Application.Run(new frmMain(frm.UserInternalID, frm.UserDisplayName));
            }
            catch (Exception ex)
            {
                MessageBox.Show("The following error occured. "+ex.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}

namespace TMB.Controls
{
    partial class ReportFilterControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtToDate = new System.Windows.Forms.DateTimePicker();
            this.chkBanks = new System.Windows.Forms.CheckedListBox();
            this.lblReportName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "From:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "To:";
            // 
            // dtFromDate
            // 
            this.dtFromDate.Location = new System.Drawing.Point(66, 60);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(198, 22);
            this.dtFromDate.TabIndex = 3;
            // 
            // dtToDate
            // 
            this.dtToDate.Location = new System.Drawing.Point(319, 60);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(186, 22);
            this.dtToDate.TabIndex = 4;
            // 
            // chkBanks
            // 
            this.chkBanks.CheckOnClick = true;
            this.chkBanks.FormattingEnabled = true;
            this.chkBanks.Location = new System.Drawing.Point(8, 89);
            this.chkBanks.Name = "chkBanks";
            this.chkBanks.Size = new System.Drawing.Size(497, 208);
            this.chkBanks.TabIndex = 6;
            // 
            // lblReportName
            // 
            this.lblReportName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReportName.Location = new System.Drawing.Point(3, 4);
            this.lblReportName.Name = "lblReportName";
            this.lblReportName.Size = new System.Drawing.Size(521, 53);
            this.lblReportName.TabIndex = 7;
            this.lblReportName.Text = "label4";
            this.lblReportName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReportFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblReportName);
            this.Controls.Add(this.chkBanks);
            this.Controls.Add(this.dtToDate);
            this.Controls.Add(this.dtFromDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ReportFilterControl";
            this.Size = new System.Drawing.Size(527, 308);
            this.Load += new System.EventHandler(this.ReportFilterControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.DateTimePicker dtToDate;
        private System.Windows.Forms.CheckedListBox chkBanks;
        private System.Windows.Forms.Label lblReportName;
    }
}

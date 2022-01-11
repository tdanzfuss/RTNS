namespace TMB.Controls.Admin
{
    partial class TermDateControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gvTermDates = new System.Windows.Forms.DataGridView();
            this.Term = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.termBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.valueDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.termDateBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTermDates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.termBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.termDateBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gvTermDates);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Size = new System.Drawing.Size(670, 469);
            this.splitContainer1.SplitterDistance = 421;
            this.splitContainer1.TabIndex = 0;
            // 
            // gvTermDates
            // 
            this.gvTermDates.AutoGenerateColumns = false;
            this.gvTermDates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTermDates.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Term,
            this.valueDateDataGridViewTextBoxColumn});
            this.gvTermDates.DataSource = this.termDateBindingSource;
            this.gvTermDates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTermDates.Location = new System.Drawing.Point(0, 0);
            this.gvTermDates.Name = "gvTermDates";
            this.gvTermDates.RowTemplate.Height = 24;
            this.gvTermDates.Size = new System.Drawing.Size(670, 421);
            this.gvTermDates.TabIndex = 0;
            // 
            // Term
            // 
            this.Term.DataPropertyName = "TermID";
            this.Term.DataSource = this.termBindingSource;
            this.Term.DisplayMember = "Name";
            this.Term.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.Nothing;
            this.Term.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Term.HeaderText = "Term";
            this.Term.Name = "Term";
            this.Term.ReadOnly = true;
            this.Term.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Term.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Term.ValueMember = "ID";
            // 
            // termBindingSource
            // 
            this.termBindingSource.DataSource = typeof(TMB.Data.Term);
            // 
            // valueDateDataGridViewTextBoxColumn
            // 
            this.valueDateDataGridViewTextBoxColumn.DataPropertyName = "ValueDate";
            dataGridViewCellStyle1.Format = "yyyy/MM/dd";
            this.valueDateDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.valueDateDataGridViewTextBoxColumn.HeaderText = "Future Date";
            this.valueDateDataGridViewTextBoxColumn.Name = "valueDateDataGridViewTextBoxColumn";
            this.valueDateDataGridViewTextBoxColumn.Width = 200;
            // 
            // termDateBindingSource
            // 
            this.termDateBindingSource.DataSource = typeof(TMB.Data.TermDate);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(511, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(592, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 28);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // TermDateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TermDateControl";
            this.Size = new System.Drawing.Size(670, 469);
            this.Load += new System.EventHandler(this.TermDateControl_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvTermDates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.termBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.termDateBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView gvTermDates;
        private System.Windows.Forms.BindingSource termDateBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn termIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource termBindingSource;
        private System.Windows.Forms.DataGridViewComboBoxColumn Term;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDateDataGridViewTextBoxColumn;
    }
}

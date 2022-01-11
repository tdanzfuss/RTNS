namespace TMB.Controls.Admin
{
    partial class ReceiversListControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gvBanks = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BankName2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.internalIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sFNNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fDCNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bankBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblAdd = new System.Windows.Forms.LinkLabel();
            this.lblEdit = new System.Windows.Forms.LinkLabel();
            this.lblRemove = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvBanks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankBindingSource)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.gvBanks);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(594, 328);
            this.splitContainer1.SplitterDistance = 299;
            this.splitContainer1.TabIndex = 0;
            // 
            // gvBanks
            // 
            this.gvBanks.AllowUserToAddRows = false;
            this.gvBanks.AllowUserToDeleteRows = false;
            this.gvBanks.AutoGenerateColumns = false;
            this.gvBanks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBanks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.BankName2,
            this.codeDataGridViewTextBoxColumn,
            this.internalIDDataGridViewTextBoxColumn,
            this.sFNNameDataGridViewTextBoxColumn,
            this.fDCNameDataGridViewTextBoxColumn});
            this.gvBanks.DataSource = this.bankBindingSource;
            this.gvBanks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvBanks.Location = new System.Drawing.Point(0, 0);
            this.gvBanks.MultiSelect = false;
            this.gvBanks.Name = "gvBanks";
            this.gvBanks.ReadOnly = true;
            this.gvBanks.RowTemplate.Height = 24;
            this.gvBanks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvBanks.Size = new System.Drawing.Size(594, 299);
            this.gvBanks.TabIndex = 0;
            this.gvBanks.SelectionChanged += new System.EventHandler(this.gvBanks_SelectionChanged);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // BankName2
            // 
            this.BankName2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BankName2.DataPropertyName = "Name";
            this.BankName2.HeaderText = "Name";
            this.BankName2.Name = "BankName2";
            this.BankName2.ReadOnly = true;
            // 
            // codeDataGridViewTextBoxColumn
            // 
            this.codeDataGridViewTextBoxColumn.DataPropertyName = "Code";
            this.codeDataGridViewTextBoxColumn.HeaderText = "Code";
            this.codeDataGridViewTextBoxColumn.Name = "codeDataGridViewTextBoxColumn";
            this.codeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // internalIDDataGridViewTextBoxColumn
            // 
            this.internalIDDataGridViewTextBoxColumn.DataPropertyName = "InternalID";
            this.internalIDDataGridViewTextBoxColumn.HeaderText = "InternalID";
            this.internalIDDataGridViewTextBoxColumn.Name = "internalIDDataGridViewTextBoxColumn";
            this.internalIDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sFNNameDataGridViewTextBoxColumn
            // 
            this.sFNNameDataGridViewTextBoxColumn.DataPropertyName = "SFNName";
            this.sFNNameDataGridViewTextBoxColumn.HeaderText = "SFNName";
            this.sFNNameDataGridViewTextBoxColumn.Name = "sFNNameDataGridViewTextBoxColumn";
            this.sFNNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fDCNameDataGridViewTextBoxColumn
            // 
            this.fDCNameDataGridViewTextBoxColumn.DataPropertyName = "FDCName";
            this.fDCNameDataGridViewTextBoxColumn.HeaderText = "FDCName";
            this.fDCNameDataGridViewTextBoxColumn.Name = "fDCNameDataGridViewTextBoxColumn";
            this.fDCNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bankBindingSource
            // 
            // this.bankBindingSource.DataSource = typeof(Data.Bank);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblAdd);
            this.flowLayoutPanel1.Controls.Add(this.lblEdit);
            this.flowLayoutPanel1.Controls.Add(this.lblRemove);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(594, 25);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // lblAdd
            // 
            this.lblAdd.AutoSize = true;
            this.lblAdd.Location = new System.Drawing.Point(3, 0);
            this.lblAdd.Name = "lblAdd";
            this.lblAdd.Size = new System.Drawing.Size(64, 17);
            this.lblAdd.TabIndex = 0;
            this.lblAdd.TabStop = true;
            this.lblAdd.Text = "Add New";
            this.lblAdd.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAdd_LinkClicked);
            // 
            // lblEdit
            // 
            this.lblEdit.AutoSize = true;
            this.lblEdit.Enabled = false;
            this.lblEdit.Location = new System.Drawing.Point(73, 0);
            this.lblEdit.Name = "lblEdit";
            this.lblEdit.Size = new System.Drawing.Size(65, 17);
            this.lblEdit.TabIndex = 1;
            this.lblEdit.TabStop = true;
            this.lblEdit.Text = "Edit/View";
            this.lblEdit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblEdit_LinkClicked);
            // 
            // lblRemove
            // 
            this.lblRemove.AutoSize = true;
            this.lblRemove.Enabled = false;
            this.lblRemove.Location = new System.Drawing.Point(144, 0);
            this.lblRemove.Name = "lblRemove";
            this.lblRemove.Size = new System.Drawing.Size(60, 17);
            this.lblRemove.TabIndex = 2;
            this.lblRemove.TabStop = true;
            this.lblRemove.Text = "Remove";
            this.lblRemove.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblRemove_LinkClicked);
            // 
            // ReceiversListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ReceiversListControl";
            this.Size = new System.Drawing.Size(594, 328);
            this.Load += new System.EventHandler(this.ReceiversListControl_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvBanks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankBindingSource)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.LinkLabel lblAdd;
        private System.Windows.Forms.LinkLabel lblEdit;
        private System.Windows.Forms.LinkLabel lblRemove;
        private System.Windows.Forms.DataGridView gvBanks;
        private System.Windows.Forms.BindingSource bankBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn BankName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BankName2;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn internalIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sFNNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fDCNameDataGridViewTextBoxColumn;
    }
}

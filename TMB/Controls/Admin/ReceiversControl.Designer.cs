namespace TMB.Controls.Admin
{
    partial class ReceiversControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtShortCode = new System.Windows.Forms.TextBox();
            this.bankBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtFDCName = new System.Windows.Forms.TextBox();
            this.txtSFNName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtInternalID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gvBankUsers = new System.Windows.Forms.DataGridView();
            this.internalIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ManualType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rTNSNotificationTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ElectronicType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.rTNSNotificationTypeBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bankUserBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lnkAddUser = new System.Windows.Forms.LinkLabel();
            this.lnkRemove = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvBankUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rTNSNotificationTypeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rTNSNotificationTypeBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankUserBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtShortCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(756, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Receiver";
            // 
            // txtShortCode
            // 
            this.txtShortCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankBindingSource, "Code", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtShortCode.Location = new System.Drawing.Point(497, 31);
            this.txtShortCode.Name = "txtShortCode";
            this.txtShortCode.Size = new System.Drawing.Size(235, 22);
            this.txtShortCode.TabIndex = 3;
            this.txtShortCode.Leave += new System.EventHandler(this.txtShortCode_Leave);
            // 
            // bankBindingSource
            // 
            this.bankBindingSource.DataSource = typeof(TMB.Data.Bank);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(400, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "ShortCode";
            // 
            // txtName
            // 
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtName.Location = new System.Drawing.Point(152, 31);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(235, 22);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtFDCName);
            this.groupBox2.Controls.Add(this.txtSFNName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtInternalID);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(3, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(758, 138);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RTNS Settings";
            // 
            // txtFDCName
            // 
            this.txtFDCName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankBindingSource, "FDCName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtFDCName.Location = new System.Drawing.Point(152, 99);
            this.txtFDCName.Name = "txtFDCName";
            this.txtFDCName.Size = new System.Drawing.Size(235, 22);
            this.txtFDCName.TabIndex = 11;
            // 
            // txtSFNName
            // 
            this.txtSFNName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankBindingSource, "SFNName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtSFNName.Location = new System.Drawing.Point(152, 60);
            this.txtSFNName.Name = "txtSFNName";
            this.txtSFNName.Size = new System.Drawing.Size(235, 22);
            this.txtSFNName.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "FDC Group Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "SFN Group Name";
            // 
            // txtInternalID
            // 
            this.txtInternalID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bankBindingSource, "InternalID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtInternalID.Location = new System.Drawing.Point(152, 21);
            this.txtInternalID.Name = "txtInternalID";
            this.txtInternalID.Size = new System.Drawing.Size(235, 22);
            this.txtInternalID.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Internal ID";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.gvBankUsers);
            this.groupBox3.Location = new System.Drawing.Point(4, 226);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(755, 449);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Users";
            // 
            // gvBankUsers
            // 
            this.gvBankUsers.AllowUserToDeleteRows = false;
            this.gvBankUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvBankUsers.AutoGenerateColumns = false;
            this.gvBankUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBankUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.internalIDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.fullNameDataGridViewTextBoxColumn,
            this.ManualType,
            this.ElectronicType});
            this.gvBankUsers.DataSource = this.bankUserBindingSource;
            this.gvBankUsers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvBankUsers.Location = new System.Drawing.Point(8, 22);
            this.gvBankUsers.Name = "gvBankUsers";
            this.gvBankUsers.RowTemplate.Height = 24;
            this.gvBankUsers.Size = new System.Drawing.Size(730, 421);
            this.gvBankUsers.TabIndex = 0;
            // 
            // internalIDDataGridViewTextBoxColumn
            // 
            this.internalIDDataGridViewTextBoxColumn.DataPropertyName = "InternalID";
            this.internalIDDataGridViewTextBoxColumn.HeaderText = "InternalID";
            this.internalIDDataGridViewTextBoxColumn.Name = "internalIDDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Routing Code";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Width = 150;
            // 
            // fullNameDataGridViewTextBoxColumn
            // 
            this.fullNameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fullNameDataGridViewTextBoxColumn.DataPropertyName = "FullName";
            this.fullNameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.fullNameDataGridViewTextBoxColumn.Name = "fullNameDataGridViewTextBoxColumn";
            // 
            // ManualType
            // 
            this.ManualType.DataPropertyName = "ManualType";
            this.ManualType.DataSource = this.rTNSNotificationTypeBindingSource;
            this.ManualType.DisplayMember = "Name";
            this.ManualType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ManualType.FillWeight = 200F;
            this.ManualType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ManualType.HeaderText = "ManualType";
            this.ManualType.MinimumWidth = 120;
            this.ManualType.Name = "ManualType";
            this.ManualType.ValueMember = "ID";
            this.ManualType.Width = 120;
            // 
            // rTNSNotificationTypeBindingSource
            // 
            this.rTNSNotificationTypeBindingSource.DataSource = typeof(TMB.Data.RTNSNotificationType);
            // 
            // ElectronicType
            // 
            this.ElectronicType.DataPropertyName = "ElectronicType";
            this.ElectronicType.DataSource = this.rTNSNotificationTypeBindingSource1;
            this.ElectronicType.DisplayMember = "Name";
            this.ElectronicType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ElectronicType.FillWeight = 200F;
            this.ElectronicType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ElectronicType.HeaderText = "ElectronicType";
            this.ElectronicType.MinimumWidth = 120;
            this.ElectronicType.Name = "ElectronicType";
            this.ElectronicType.ValueMember = "ID";
            this.ElectronicType.Width = 120;
            // 
            // rTNSNotificationTypeBindingSource1
            // 
            this.rTNSNotificationTypeBindingSource1.DataSource = typeof(TMB.Data.RTNSNotificationType);
            // 
            // bankUserBindingSource
            // 
            this.bankUserBindingSource.DataSource = typeof(TMB.Data.BankUser);
            // 
            // lnkAddUser
            // 
            this.lnkAddUser.AutoSize = true;
            this.lnkAddUser.Location = new System.Drawing.Point(9, 678);
            this.lnkAddUser.Name = "lnkAddUser";
            this.lnkAddUser.Size = new System.Drawing.Size(64, 17);
            this.lnkAddUser.TabIndex = 3;
            this.lnkAddUser.TabStop = true;
            this.lnkAddUser.Text = "Add New";
            // 
            // lnkRemove
            // 
            this.lnkRemove.AutoSize = true;
            this.lnkRemove.Location = new System.Drawing.Point(79, 678);
            this.lnkRemove.Name = "lnkRemove";
            this.lnkRemove.Size = new System.Drawing.Size(60, 17);
            this.lnkRemove.TabIndex = 4;
            this.lnkRemove.TabStop = true;
            this.lnkRemove.Text = "Remove";
            // 
            // ReceiversControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lnkRemove);
            this.Controls.Add(this.lnkAddUser);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ReceiversControl";
            this.Size = new System.Drawing.Size(770, 718);
            this.Load += new System.EventHandler(this.ReceiversControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bankBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvBankUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rTNSNotificationTypeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rTNSNotificationTypeBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bankUserBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtShortCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtInternalID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFDCName;
        private System.Windows.Forms.TextBox txtSFNName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView gvBankUsers;
        private System.Windows.Forms.LinkLabel lnkAddUser;
        private System.Windows.Forms.LinkLabel lnkRemove;
        private System.Windows.Forms.BindingSource bankUserBindingSource;
        private System.Windows.Forms.BindingSource rTNSNotificationTypeBindingSource;
        private System.Windows.Forms.BindingSource bankBindingSource;
        private System.Windows.Forms.BindingSource rTNSNotificationTypeBindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn internalIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn ManualType;
        private System.Windows.Forms.DataGridViewComboBoxColumn ElectronicType;
    }
}

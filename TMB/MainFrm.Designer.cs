namespace TMB
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDealToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fXSpotToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fXSwapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fXForwardToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fxNDFToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mmDepoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnReports = new System.Windows.Forms.Button();
            this.btnDeals = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlAdministrationLinks = new System.Windows.Forms.Panel();
            this.btnCurrencyScales = new System.Windows.Forms.Button();
            this.btnTermDates = new System.Windows.Forms.Button();
            this.btnUsers = new System.Windows.Forms.Button();
            this.btnCurrencies = new System.Windows.Forms.Button();
            this.btnReceivers = new System.Windows.Forms.Button();
            this.btnFixingCenters = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dealListControl1 = new TMB.Controls.DealListControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.imgOK = new System.Windows.Forms.PictureBox();
            this.imgWarning = new System.Windows.Forms.PictureBox();
            this.imgError = new System.Windows.Forms.PictureBox();
            this.lblConnectionStatus = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlAdministrationLinks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgError)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.newDealToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(896, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(44, 24);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 24);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // newDealToolStripMenuItem
            // 
            this.newDealToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fXSpotToolStripMenuItem1,
            this.fXSwapToolStripMenuItem1,
            this.fXForwardToolStripMenuItem1,
            this.fxNDFToolStripMenuItem1,
            this.mmDepoToolStripMenuItem1});
            this.newDealToolStripMenuItem.Name = "newDealToolStripMenuItem";
            this.newDealToolStripMenuItem.Size = new System.Drawing.Size(86, 24);
            this.newDealToolStripMenuItem.Text = "New Deal";
            // 
            // fXSpotToolStripMenuItem1
            // 
            this.fXSpotToolStripMenuItem1.Name = "fXSpotToolStripMenuItem1";
            this.fXSpotToolStripMenuItem1.Size = new System.Drawing.Size(152, 24);
            this.fXSpotToolStripMenuItem1.Text = "FX Spot";
            this.fXSpotToolStripMenuItem1.Click += new System.EventHandler(this.fXSpotToolStripMenuItem1_Click);
            // 
            // fXSwapToolStripMenuItem1
            // 
            this.fXSwapToolStripMenuItem1.Name = "fXSwapToolStripMenuItem1";
            this.fXSwapToolStripMenuItem1.Size = new System.Drawing.Size(152, 24);
            this.fXSwapToolStripMenuItem1.Text = "FX Swap";
            this.fXSwapToolStripMenuItem1.Click += new System.EventHandler(this.fXSwapToolStripMenuItem1_Click);
            // 
            // fXForwardToolStripMenuItem1
            // 
            this.fXForwardToolStripMenuItem1.Name = "fXForwardToolStripMenuItem1";
            this.fXForwardToolStripMenuItem1.Size = new System.Drawing.Size(152, 24);
            this.fXForwardToolStripMenuItem1.Text = "FX Forward";
            this.fXForwardToolStripMenuItem1.Click += new System.EventHandler(this.fXForwardToolStripMenuItem1_Click);

            // 
            // fxNDFToolStripMenuItem1
            // 
            this.fxNDFToolStripMenuItem1.Name = "fxNDFToolStripMenuItem1";
            this.fxNDFToolStripMenuItem1.Size = new System.Drawing.Size(152, 24);
            this.fxNDFToolStripMenuItem1.Text = "FX NDF";
            this.fxNDFToolStripMenuItem1.Click += new System.EventHandler(this.fxNDFToolStripMenuItem1_Click);

            //
            // MMDepoToolstripMenuItem1
            //
            this.mmDepoToolStripMenuItem1.Name = "mmDepoToolStripMenuItem1";
            this.mmDepoToolStripMenuItem1.Size = new System.Drawing.Size(152, 24);
            this.mmDepoToolStripMenuItem1.Text = "MM Deposit";
            this.mmDepoToolStripMenuItem1.Click += new System.EventHandler(this.mmDepoToolStripMenuItem1_Click);

            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 28);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer3.Size = new System.Drawing.Size(896, 631);
            this.splitContainer3.SplitterDistance = 600;
            this.splitContainer3.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.btnSettings);
            this.splitContainer1.Panel1.Controls.Add(this.btnReports);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeals);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(896, 600);
            this.splitContainer1.SplitterDistance = 144;
            this.splitContainer1.TabIndex = 1;
            // 
            // btnSettings
            // 
            this.btnSettings.BackgroundImage = global::TMB.Properties.Resources.Settings_small;
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.Location = new System.Drawing.Point(7, 113);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(124, 46);
            this.btnSettings.TabIndex = 7;
            this.btnSettings.Text = "Settings";
            this.btnSettings.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnReports
            // 
            this.btnReports.BackgroundImage = global::TMB.Properties.Resources.invoice_small;
            this.btnReports.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReports.FlatAppearance.BorderSize = 0;
            this.btnReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReports.Location = new System.Drawing.Point(7, 60);
            this.btnReports.Name = "btnReports";
            this.btnReports.Size = new System.Drawing.Size(124, 47);
            this.btnReports.TabIndex = 6;
            this.btnReports.Text = "Reports";
            this.btnReports.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnReports.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReports.UseVisualStyleBackColor = true;
            this.btnReports.Click += new System.EventHandler(this.btnReports_Click);
            // 
            // btnDeals
            // 
            this.btnDeals.BackgroundImage = global::TMB.Properties.Resources.coins_small;
            this.btnDeals.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDeals.FlatAppearance.BorderSize = 0;
            this.btnDeals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeals.Location = new System.Drawing.Point(7, 4);
            this.btnDeals.Name = "btnDeals";
            this.btnDeals.Size = new System.Drawing.Size(108, 50);
            this.btnDeals.TabIndex = 5;
            this.btnDeals.Text = "Deals";
            this.btnDeals.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDeals.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDeals.UseVisualStyleBackColor = true;
            this.btnDeals.Click += new System.EventHandler(this.btnDeals_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pnlAdministrationLinks);
            this.panel1.Location = new System.Drawing.Point(3, 165);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(138, 402);
            this.panel1.TabIndex = 4;
            this.panel1.Visible = false;
            // 
            // pnlAdministrationLinks
            // 
            this.pnlAdministrationLinks.BackColor = System.Drawing.Color.White;
            this.pnlAdministrationLinks.Controls.Add(this.btnCurrencyScales);
            this.pnlAdministrationLinks.Controls.Add(this.btnTermDates);
            this.pnlAdministrationLinks.Controls.Add(this.btnUsers);
            this.pnlAdministrationLinks.Controls.Add(this.btnCurrencies);
            this.pnlAdministrationLinks.Controls.Add(this.btnReceivers);
            this.pnlAdministrationLinks.Controls.Add(this.btnFixingCenters);
            this.pnlAdministrationLinks.Location = new System.Drawing.Point(3, 13);
            this.pnlAdministrationLinks.Name = "pnlAdministrationLinks";
            this.pnlAdministrationLinks.Size = new System.Drawing.Size(127, 371);
            this.pnlAdministrationLinks.TabIndex = 3;
            this.pnlAdministrationLinks.Visible = false;
            // 
            // btnCurrencyScales
            // 
            this.btnCurrencyScales.FlatAppearance.BorderSize = 0;
            this.btnCurrencyScales.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCurrencyScales.Image = global::TMB.Properties.Resources.money;
            this.btnCurrencyScales.Location = new System.Drawing.Point(5, 287);
            this.btnCurrencyScales.Name = "btnCurrencyScales";
            this.btnCurrencyScales.Size = new System.Drawing.Size(121, 70);
            this.btnCurrencyScales.TabIndex = 7;
            this.btnCurrencyScales.Text = "Currency Scales";
            this.btnCurrencyScales.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCurrencyScales.UseVisualStyleBackColor = true;
            this.btnCurrencyScales.Click += new System.EventHandler(this.btnCurrencyScales_Click);
            // 
            // btnTermDates
            // 
            this.btnTermDates.FlatAppearance.BorderSize = 0;
            this.btnTermDates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTermDates.Image = global::TMB.Properties.Resources.reports;
            this.btnTermDates.Location = new System.Drawing.Point(3, 216);
            this.btnTermDates.Name = "btnTermDates";
            this.btnTermDates.Size = new System.Drawing.Size(121, 65);
            this.btnTermDates.TabIndex = 6;
            this.btnTermDates.Text = "Term Dates";
            this.btnTermDates.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnTermDates.UseVisualStyleBackColor = true;
            this.btnTermDates.Click += new System.EventHandler(this.btnTermDates_Click);
            // 
            // btnUsers
            // 
            this.btnUsers.FlatAppearance.BorderSize = 0;
            this.btnUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUsers.Image = global::TMB.Properties.Resources.User_small;
            this.btnUsers.Location = new System.Drawing.Point(5, 3);
            this.btnUsers.Name = "btnUsers";
            this.btnUsers.Size = new System.Drawing.Size(121, 65);
            this.btnUsers.TabIndex = 5;
            this.btnUsers.Text = "Users";
            this.btnUsers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnUsers.UseVisualStyleBackColor = true;
            this.btnUsers.Click += new System.EventHandler(this.btnUsers_Click);
            // 
            // btnCurrencies
            // 
            this.btnCurrencies.FlatAppearance.BorderSize = 0;
            this.btnCurrencies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCurrencies.Image = global::TMB.Properties.Resources.dollar_small;
            this.btnCurrencies.Location = new System.Drawing.Point(3, 74);
            this.btnCurrencies.Name = "btnCurrencies";
            this.btnCurrencies.Size = new System.Drawing.Size(121, 65);
            this.btnCurrencies.TabIndex = 4;
            this.btnCurrencies.Text = "Currencies";
            this.btnCurrencies.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCurrencies.UseVisualStyleBackColor = true;
            this.btnCurrencies.Click += new System.EventHandler(this.btnCurrencies_Click);
            // 
            // btnReceivers
            // 
            this.btnReceivers.FlatAppearance.BorderSize = 0;
            this.btnReceivers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReceivers.Image = global::TMB.Properties.Resources.Receivers_small;
            this.btnReceivers.Location = new System.Drawing.Point(3, 145);
            this.btnReceivers.Name = "btnReceivers";
            this.btnReceivers.Size = new System.Drawing.Size(121, 65);
            this.btnReceivers.TabIndex = 3;
            this.btnReceivers.Text = "Receivers";
            this.btnReceivers.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnReceivers.UseVisualStyleBackColor = true;
            this.btnReceivers.Click += new System.EventHandler(this.btnReceivers_Click);

            // 
            // btnCurrencyScales
            // 
            this.btnFixingCenters.FlatAppearance.BorderSize = 0;
            this.btnFixingCenters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFixingCenters.Image = global::TMB.Properties.Resources.money;
            this.btnFixingCenters.Location = new System.Drawing.Point(5, 387);
            this.btnFixingCenters.Name = "btnFixingCenters";
            this.btnFixingCenters.Size = new System.Drawing.Size(121, 70);
            this.btnFixingCenters.TabIndex = 7;
            this.btnFixingCenters.Text = "Fixing Centers";
            this.btnFixingCenters.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnFixingCenters.UseVisualStyleBackColor = true;
            this.btnFixingCenters.Click += new System.EventHandler(this.btnFixingCenters_Click);
            // 

            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dealListControl1);
            this.splitContainer2.Size = new System.Drawing.Size(748, 600);
            this.splitContainer2.SplitterDistance = 72;
            this.splitContainer2.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.lblHeaderText);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(748, 72);
            this.panel2.TabIndex = 0;
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.AutoSize = true;
            this.lblHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderText.Location = new System.Drawing.Point(74, 8);
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Size = new System.Drawing.Size(228, 46);
            this.lblHeaderText.TabIndex = 1;
            this.lblHeaderText.Text = "Deal Listing";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::TMB.Properties.Resources.coins;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // dealListControl1
            // 
            this.dealListControl1.BackColor = System.Drawing.SystemColors.Control;
            this.dealListControl1.Connection = null;
            this.dealListControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dealListControl1.Location = new System.Drawing.Point(0, 0);
            this.dealListControl1.Name = "dealListControl1";
            this.dealListControl1.ParentControl = null;
            this.dealListControl1.Size = new System.Drawing.Size(748, 524);
            this.dealListControl1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.imgOK);
            this.flowLayoutPanel1.Controls.Add(this.imgWarning);
            this.flowLayoutPanel1.Controls.Add(this.imgError);
            this.flowLayoutPanel1.Controls.Add(this.lblConnectionStatus);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(896, 27);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // imgOK
            // 
            this.imgOK.BackgroundImage = global::TMB.Properties.Resources.ok;
            this.imgOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgOK.Location = new System.Drawing.Point(877, 3);
            this.imgOK.Name = "imgOK";
            this.imgOK.Size = new System.Drawing.Size(16, 16);
            this.imgOK.TabIndex = 0;
            this.imgOK.TabStop = false;
            // 
            // imgWarning
            // 
            this.imgWarning.BackgroundImage = global::TMB.Properties.Resources.warning;
            this.imgWarning.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgWarning.Location = new System.Drawing.Point(855, 3);
            this.imgWarning.Name = "imgWarning";
            this.imgWarning.Size = new System.Drawing.Size(16, 16);
            this.imgWarning.TabIndex = 1;
            this.imgWarning.TabStop = false;
            // 
            // imgError
            // 
            this.imgError.BackgroundImage = global::TMB.Properties.Resources.error;
            this.imgError.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgError.Location = new System.Drawing.Point(833, 3);
            this.imgError.Name = "imgError";
            this.imgError.Size = new System.Drawing.Size(16, 16);
            this.imgError.TabIndex = 2;
            this.imgError.TabStop = false;
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConnectionStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionStatus.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.lblConnectionStatus.Location = new System.Drawing.Point(139, 0);
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Size = new System.Drawing.Size(688, 22);
            this.lblConnectionStatus.TabIndex = 4;
            this.lblConnectionStatus.Text = "label1";
            this.lblConnectionStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 659);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Deal Capturing Solution";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlAdministrationLinks.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgError)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.PictureBox imgOK;
        private System.Windows.Forms.PictureBox imgWarning;
        private System.Windows.Forms.PictureBox imgError;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.ToolStripMenuItem newDealToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fXSpotToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fXSwapToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fXForwardToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fxNDFToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mmDepoToolStripMenuItem1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlAdministrationLinks;
        private System.Windows.Forms.Panel panel1;
        private Controls.DealListControl dealListControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.Button btnDeals;
        private System.Windows.Forms.Button btnReports;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnReceivers;
        private System.Windows.Forms.Button btnCurrencies;
        private System.Windows.Forms.Button btnUsers;
        private System.Windows.Forms.Button btnTermDates;
        private System.Windows.Forms.Button btnCurrencyScales;
        private System.Windows.Forms.Button btnFixingCenters;
    }
}


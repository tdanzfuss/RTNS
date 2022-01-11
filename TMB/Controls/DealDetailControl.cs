using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using TMB.Data;

namespace TMB.Controls
{
    public partial class DealDetailControl : UserControl, IPopupFormControl
    {
        private int? dealID;
        private int? productID;
        private bool newDeal;
        private Transaction transaction;
        private TMBDataContext context;
        private string headingText;
        private bool isMultiLeg;
        private bool inBinding;
        private TMB.Reuters.RTNSConnection connection;

        public DealDetailControl()
        {
            InitializeComponent();
            context = new TMBDataContext();
            headingText = "New Deal";
            newDeal = true;
            isMultiLeg = false;
        }

        public DealDetailControl(int prmProductID)
            : this()
        {
            productID = prmProductID;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        public int? DealID
        {
            get
            {
                return dealID;
            }
            set
            {
                dealID = value;
                if (dealID.HasValue)
                {
                    headingText = "Edit Deal";
                    newDeal = false;
                    transaction = context.Transactions
                        .Where(t => t.ID.Equals(dealID))
                        .FirstOrDefault();

                    isMultiLeg = (transaction.Product.IsMultiLeg.Value == 1);
                    setPanelVisibility(transaction.Product.ID);
                    /*isMultiLeg = (transaction.Product.IsMultiLeg.Value == 1);
                    pnlForward.Visible = (isMultiLeg || transaction.Product.ID == 2 || transaction.Product.ID == 4);
                    lblAllIn.Visible = txtAllIn.Visible = (transaction.Product.ID == 2 || transaction.Product.ID == 4);
                    // context.Transactions.InsertOnSubmit(transaction);
                    pnlFix.Visible = transaction.Product.ID == 4;*/
                }
            }
        }

        private void setPanelVisibility(int productId)
        {
            pnlForward.Visible = (isMultiLeg || productId == 2 || productId == 4);
            lblAllIn.Visible = txtAllIn.Visible = (productId == 2 || productId == 4);
            // context.Transactions.InsertOnSubmit(transaction);
            pnlFix.Visible = (productId == 4);
            pnlMMDepo.Visible = (productID == 5);
            if (pnlFix.Visible)
            {
                lblAllIn.Text = "FWD Rate:";
                BindFixingCenters();
                BindFixingCurrencies();
            }
            else if (pnlMMDepo.Visible)
            {
                lblSpotDate.Text = "Maturity Date";
                BindPaymentFrequency();
            }
        }

        private void DealDetailControl_Load(object sender, EventArgs e)
        {
            try
            {
                inBinding = true;
                if (connection == null)
                    throw new Exception("RTNS Connection not initialized.");

                if (ParentControl == null)
                    throw new Exception("Parent control not initialized");

                if (newDeal)
                {
                    headingText = "New Deal";
                    if (productID.HasValue)
                    {
                        // cbxProduct.SelectedValue = productID.Value;
                        var pr = context.Products.Where(p => p.ID == productID.Value).FirstOrDefault();
                        isMultiLeg = (pr.IsMultiLeg.Value == 1);
                        /*pnlForward.Visible = (isMultiLeg || productID.Value == 2);
                        lblAllIn.Visible = txtAllIn.Visible = (productID.Value == 2);*/
                        setPanelVisibility(productID.Value);

                        // By default the base currency is the Dealt Currency
                        cbxDealtCurrency.SelectedIndex = 0;
                    }

                    transaction = context.NewTransaction(isMultiLeg, productID);
                    dealID = transaction.ID;
                    context.Transactions.InsertOnSubmit(transaction);
                    // we must do this to get the ID
                    // context.SubmitChanges();
                }

                BindTransaction();

                if (newDeal)
                {
                    SetInitialHeadings();
                    CheckTermDates((((Data.Term)cbxTerm.SelectedItem).ID));
                }
                else
                {
                    CalculateSummaries();
                }
            }
            finally 
            {
                inBinding = false;
            }
        }

        private void BindTransaction()
        {
            int lineNumber = 0;
            try
            {
                txtDealNr.DataBindings.Add(new Binding("Text", transaction, "ID"));
                txtDescription.DataBindings.Add(new Binding("Text", transaction, "Description"));

                txtMessage.DataBindings.Add(new Binding("Text", transaction, "Message"));
                txtReference.DataBindings.Add(new Binding("Text", transaction, "TransactionReference"));
                // txtStatus.DataBindings.Add(new Binding("Text", transaction, "Status1.Name")); lineNumber++;
                if (transaction.Status.HasValue)
                    txtStatus.Text = transaction.Status1.Name;

                txtSubStatus.DataBindings.Add(new Binding("Text", transaction, "SubStatus"));

                txtForwardScale.DataBindings.Add(new Binding("Text", transaction, "Scale"));

                dtSpotDate.DataBindings.Add(new Binding("Value", transaction, "spotdate", true, DataSourceUpdateMode.OnPropertyChanged, DateTime.Now.AddDays(2)));

                cbxTerm.DataSource = context.Terms.ToList(); lineNumber++;
                cbxTerm.DisplayMember = "Name"; lineNumber++;
                cbxTerm.ValueMember = "ID"; lineNumber++;

                cbxProduct.DataSource = context.Products.ToList(); lineNumber++;
                cbxProduct.DisplayMember = "Name"; lineNumber++;
                cbxProduct.ValueMember = "ID"; lineNumber++;
                cbxProduct.DataBindings.Add(new Binding("SelectedItem", transaction, "Product", true, DataSourceUpdateMode.OnPropertyChanged)); lineNumber++;
                // cbxProduct.DataBindings.Add(new Binding("SelectedValue", transaction, "ProductType", true, DataSourceUpdateMode.OnPropertyChanged));
                // cbxProduct.Text = "Please select a product...";
                if (newDeal && productID.HasValue)
                {
                    cbxProduct.SelectedValue = productID.Value; lineNumber++;
                    transaction.ProductType = productID.Value; lineNumber++;
                    transaction.Product = ((Product)cbxProduct.SelectedItem); lineNumber++;
                    isMultiLeg = (((Product)cbxProduct.SelectedItem).IsMultiLeg.Value == 1); lineNumber++;
                    /*pnlForward.Visible = (isMultiLeg || productID.Value == 2);
                    lblAllIn.Visible = txtAllIn.Visible = (productID.Value == 2);*/
                    setPanelVisibility(productID.Value);

                    txtForwardScale.Text = "4";
                }

                // Set base and dealt currencies.
                if (transaction.DealtCurrency.HasValue)
                    cbxDealtCurrency.SelectedIndex = transaction.DealtCurrency.Value;
                if (transaction.BaseCurrency.HasValue)
                {
                    rbBaseBuyer.Checked = (transaction.BaseCurrency.Value == 0);
                    rbBaseSeller.Checked = (transaction.BaseCurrency.Value > 0);
                }


                BindTransactionLegA(); lineNumber++;
                if (isMultiLeg)
                    BindTransactionLegB(); lineNumber++;

                // Bind NDF specific controls...
                if (productID.HasValue && productID.Value == 4)
                    BindTransactionFix();

                // Bind MM Deposit specific controls...
                if (productID.HasValue && productID.Value == 5)
                    BindTransactionMM();

                if (!newDeal)
                    ComputeDescription();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Binding to transaction. " + lineNumber + ": " + ex.ToString(), "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindFixingCenters()
        {
            // cbxFixincCentre
            cbxFixincCentre.DataSource = context.FixingCentres                                   
                                    .OrderBy(b => b.Name)
                                    .ToList();
            cbxFixincCentre.DisplayMember = "Name";
            cbxFixincCentre.ValueMember = "ID"; 
        }

        private void BindPaymentFrequency()
        {
            cbxPaymentFrequency.DataSource = context.PaymentFrequencyLookups.ToList();
            cbxPaymentFrequency.DisplayMember = "Name";
            cbxPaymentFrequency.ValueMember = "ID";
        }

        private List<Currency> settlementCurrencies;
        private void BindFixingCurrencies()
        {            
            if (settlementCurrencies == null)
                settlementCurrencies = new List<Currency>();

            cbxFixCurr.DataSource = settlementCurrencies;
            cbxFixCurr.DisplayMember = "Code";
            cbxFixCurr.ValueMember = "ID";
        }

        private void AddCurrency()
        {
            string selectedElementCode = (cbxFixCurr.SelectedItem != null) ? ((Currency)cbxFixCurr.SelectedItem).Code : string.Empty;

            settlementCurrencies.Clear();
            if (cbxByerCurrency_1.SelectedItem != null)
                settlementCurrencies.Add((Currency)cbxByerCurrency_1.SelectedItem);
            if (cbxSellerCurrency_1.SelectedItem != null)
                settlementCurrencies.Add((Currency)cbxSellerCurrency_1.SelectedItem);         

            cbxFixCurr.DataSource = null;
            cbxFixCurr.DataSource = settlementCurrencies;
            cbxFixCurr.DisplayMember = "Code";
            cbxFixCurr.ValueMember = "ID";

            if (selectedElementCode.Length > 0)
            {
                foreach (Currency c in settlementCurrencies)
                {
                    if (c.Code.Equals(selectedElementCode))
                    {
                        cbxFixCurr.SelectedItem = c;
                        return;
                    }
                }
            }

        }

        private void BindTransactionFix()
        {
            // TODO: Bind all the TransactionFix controls.       
            cbxFixincCentre.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionFixes, "FixingCentre", true, DataSourceUpdateMode.OnPropertyChanged));
            cbxFixCurr.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionFixes, "Currency", true, DataSourceUpdateMode.OnPropertyChanged) );
            txtFixDays.DataBindings.Add(new Binding("Text", transaction.TransactionFixes, "FixingDays", true, DataSourceUpdateMode.OnPropertyChanged, 0));
            dtFixDate.DataBindings.Add(new Binding("Value", transaction.TransactionFixes, "FixingDate", true, DataSourceUpdateMode.OnPropertyChanged, DateTime.Now.AddDays(transaction.TransactionFixes.FixingDays)));
        }

        private void BindTransactionMM()
        {
            txtDaysPerYear.DataBindings.Add(new Binding("Text", transaction.TransactionMMs, "DaysPerYear", true, DataSourceUpdateMode.OnPropertyChanged, 0));
            txtInterest.DataBindings.Add(new Binding("Text", transaction.TransactionMMs, "InterestAmount", true, DataSourceUpdateMode.OnPropertyChanged, 0));
            cbxPaymentFrequency.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionMMs, "PaymentFrequencyLookup", true, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void BindTransactionLegA()
        {
            cbxBuyer_1.DataSource = context.Banks
                                    .Where(b=>b.Status == 1)
                                    .OrderBy(b=>b.Name)
                                    .ToList();
            cbxBuyer_1.DisplayMember = "Name";
            cbxBuyer_1.ValueMember = "ID";
            cbxBuyer_1.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[0], "Bank",true,DataSourceUpdateMode.OnPropertyChanged));
            //cbxBuyer_1.Text = "Buyer...";

            cbxBuyerPerson.DisplayMember = "Name";
            cbxBuyerPerson.ValueMember = "ID";
            cbxBuyerPerson.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[0], "BankUser",true,DataSourceUpdateMode.OnPropertyChanged));

            cbxByerCurrency_1.DataSource = context.Currencies.OrderBy(w=>w.Code).ToList();
            cbxByerCurrency_1.DisplayMember = "Code";
            cbxByerCurrency_1.ValueMember = "ID";
            cbxByerCurrency_1.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[0], "Currency", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxByerCurrency_1.Text = "Buyer Currency...";

            cbxSeller_1.DataSource = context.Banks
                                      .Where(b => b.Status == 1)
                                      .OrderBy(b => b.Name)
                                      .ToList();
            cbxSeller_1.DisplayMember = "Name";
            cbxSeller_1.ValueMember = "ID";
            cbxSeller_1.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[0], "Bank1", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxSeller_1.Text = "Seller...";

            cbxSellerPerson.DisplayMember = "Name";
            cbxSellerPerson.ValueMember = "ID";
            cbxSellerPerson.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[0], "BankUser1", true, DataSourceUpdateMode.OnPropertyChanged));

            cbxSellerCurrency_1.DataSource = context.Currencies.OrderBy(w=>w.Code).ToList();
            cbxSellerCurrency_1.DisplayMember = "Code";
            cbxSellerCurrency_1.ValueMember = "ID";
            cbxSellerCurrency_1.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[0], "Currency1", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxSellerCurrency_1.Text = "Seller Currency...";

            cbxAmountType_1.DataSource = context.AmountTypes.ToList();
            cbxAmountType_1.DisplayMember = "Code";
            cbxAmountType_1.ValueMember = "ID";
            cbxAmountType_1.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[0], "AmountType1", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxAmountType_1.Text = "Amount Type...";

            //, true, DataSourceUpdateMode.OnPropertyChanged, string.Empty
            dtLegDate_1.DataBindings.Add(new Binding("Value", transaction.TransactionLegs[0], "ActionDate", true, DataSourceUpdateMode.OnPropertyChanged, DateTime.Now));
            txtAmount_1.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[0], "Amount",true,DataSourceUpdateMode.OnPropertyChanged,0));
            txtBuyerBrokerage_1.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[0], "BuyerBrokerage",true,DataSourceUpdateMode.OnPropertyChanged,0));
            txtSellerBrokerage_1.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[0], "SellerBrokerage", true, DataSourceUpdateMode.OnPropertyChanged, 0));
            
            // txtPoints_1.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[0].Points, "BuyerBrokerage"));
            txtRate_1.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[0], "Rate", true, DataSourceUpdateMode.OnPropertyChanged, 0));
            txtTerm_1.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[0], "Term", true, DataSourceUpdateMode.OnPropertyChanged, string.Empty));
            //if (isMultiLeg)
            //    txtPoints_1.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[1], "ForwardPoints"));

            if (!newDeal)
            {
                BindBuyerPersons();
                BindSellerPersons();

                // Load the points
                // If this is SWAP then save it on the FAR leg
                if (isMultiLeg)
                    txtPoints_1.Text = (transaction.TransactionLegs[1].ForwardPoints.HasValue)
                        ? transaction.TransactionLegs[1].ForwardPoints.Value.ToString()
                        : "0";

                // If it is FWD then save it on the near leg
                else if (transaction.ProductType.Value == 2)
                    txtPoints_1.Text = (transaction.TransactionLegs[0].ForwardPoints.HasValue)
                        ? transaction.TransactionLegs[0].ForwardPoints.Value.ToString()
                        : "0";                    
            }
        }
        private void BindTransactionLegB()
        {
            cbxBuyer_2.DataSource = context.Banks.OrderBy(b => b.Name).ToList();
            cbxBuyer_2.DisplayMember = "Name";
            cbxBuyer_2.ValueMember = "ID";
            cbxBuyer_2.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[1], "Bank", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxBuyer_2.Text = "Buyer...";

            cbxByerCurrency_2.DataSource = context.Currencies.OrderBy(w=>w.Code).ToList();
            cbxByerCurrency_2.DisplayMember = "Code";
            cbxByerCurrency_2.ValueMember = "ID";
            cbxByerCurrency_2.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[1], "Currency", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxByerCurrency_2.Text = "Buyer Currency...";            

            cbxSeller_2.DataSource = context.Banks.OrderBy(b => b.Name).ToList();
            cbxSeller_2.DisplayMember = "Name";
            cbxSeller_2.ValueMember = "ID";
            cbxSeller_2.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[1], "Bank1", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxSeller_2.Text = "Seller...";

            cbxSellerCurrency_2.DataSource = context.Currencies.OrderBy(w=>w.Code).ToList();
            cbxSellerCurrency_2.DisplayMember = "Code";
            cbxSellerCurrency_2.ValueMember = "ID";
            cbxSellerCurrency_2.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[1], "Currency1", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxSellerCurrency_2.Text = "Seller Currency...";

            cbxAmountType_2.DataSource = context.AmountTypes.ToList();
            cbxAmountType_2.DisplayMember = "Code";
            cbxAmountType_2.ValueMember = "ID";
            cbxAmountType_2.DataBindings.Add(new Binding("SelectedItem", transaction.TransactionLegs[1], "AmountType1", true, DataSourceUpdateMode.OnPropertyChanged));
            //cbxAmountType_2.Text = "Amount Type...";

            dt_LegDate_2.DataBindings.Add(new Binding("Value", transaction.TransactionLegs[1], "ActionDate", true, DataSourceUpdateMode.OnPropertyChanged, DateTime.Now));
            txtAmount_2.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[1], "Amount", true, DataSourceUpdateMode.OnPropertyChanged, 0));
            txtRate_2.DataBindings.Add(new Binding("Text", transaction.TransactionLegs[1], "Rate", true, DataSourceUpdateMode.OnPropertyChanged, 0));

        }

        private void SetInitialHeadings()
        {
            cbxBuyer_1.Text = "Buyer...";
            cbxByerCurrency_1.Text = "Buyer Currency...";
            cbxSeller_1.Text = "Seller...";
            cbxSellerCurrency_1.Text = "Seller Currency...";
            cbxAmountType_1.Text = "Amount Type...";
            cbxBuyer_2.Text = "Buyer...";
            cbxByerCurrency_2.Text = "Buyer Currency...";
            cbxSeller_2.Text = "Seller...";
            cbxSellerCurrency_2.Text = "Seller Currency...";
            cbxAmountType_2.Text = "Amount Type...";

            cbxBuyerPerson.Text = "Buyer Person...";
            cbxSellerPerson.Text = "Seller Person...";

            cbxFixincCentre.Text = "Fixing Centre...";
            cbxFixCurr.Text = "Settlement Currency...";

            cbxPaymentFrequency.Text = "Payment Frequency...";
        }

        public bool SaveControl()
        {
            if (!IsValid())
            {
                return false;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (transaction.Status == 1)
                    transaction.Status1 = context.Status.Where(w => w.ID == 2).First();

                try
                {
                    transaction.TransactionLegs[0].BuyerPersonID = (int)cbxBuyerPerson.SelectedValue;
                    transaction.TransactionLegs[0].SellerPersonID = (int)cbxSellerPerson.SelectedValue;
                    if (isMultiLeg)
                    {
                        transaction.TransactionLegs[1].BuyerPersonID = (int)cbxSellerPerson.SelectedValue;
                        transaction.TransactionLegs[1].SellerPersonID = (int)cbxBuyerPerson.SelectedValue;
                    }
                }
                catch (System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException ex)
                {
                    transaction.TransactionLegs[0].BankUser = (BankUser)cbxBuyerPerson.SelectedItem;
                    transaction.TransactionLegs[0].BankUser1 = (BankUser)cbxSellerPerson.SelectedItem;
                    if (isMultiLeg)
                    {
                        transaction.TransactionLegs[1].BankUser = (BankUser)cbxSellerPerson.SelectedItem;
                        transaction.TransactionLegs[1].BankUser1 = (BankUser)cbxBuyerPerson.SelectedItem;
                    }
                }
                // Set the BaseCurrency to either Buyer or Seller currency
                transaction.BaseCurrency = (rbBaseBuyer.Checked) ? 0 : 1;
                transaction.DealtCurrency = cbxDealtCurrency.SelectedIndex;

                // If this is SWAP then save it on the FAR leg
                if (isMultiLeg && txtPoints_1.Text.Length > 0)
                    transaction.TransactionLegs[1].ForwardPoints = Convert.ToDouble(txtPoints_1.Text);
                // If it is FWD OR NDF then save it on the near leg
                else if ((transaction.ProductType.Value == 2 || transaction.ProductType.Value == 4) && txtPoints_1.Text.Length > 0)
                    transaction.TransactionLegs[0].ForwardPoints = Convert.ToDouble(txtPoints_1.Text);

                context.SubmitChanges();

                // Build the unique RTNS reference
                transaction.TransactionReference = ConfigurationManager.AppSettings["brokercode"] + transaction.ID.ToString("D6");

                // If it is a new deal automatically submit to RTNS
                // Else ask the user if he wants to submit
                if (newDeal || (DialogResult.Yes == MessageBox.Show("Do you want to resubmit this trade to RTNS?", "Submit Trade to RTNS", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)))
                {
                    // Submit the transaction to Reuters
                    Reuters.RTNSResponseMessage response =
                        (newDeal)
                        ?
                        connection.ConfirmDeal(
                        ParentControl.BrokerName,
                        ParentControl.UserDisplayName,
                        transaction)
                        :
                        connection.UpdateDeal(
                        ParentControl.BrokerName,
                        ParentControl.UserDisplayName,
                        transaction);

                    transaction.RTNSReference = response.Reference;
                    int responseStatus = (response.Success) ? 3 : 7;
                    transaction.Status1 = context.Status.Where(w => w.ID == responseStatus).First();

                    context.SubmitChanges();

                    if (!response.Success)
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Could not submit deal to Reuters. The following error occured " + response.ErrorName, "RTNS Integration");
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;

                MessageBox.Show(ex.ToString());
                System.IO.File.WriteAllLines("error.log", new string[] { ex.ToString() });

                return false;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            return true;
        }

        public void CancelControl()
        {
            
        }

        public string HeadingText
        {
            get { return headingText; }
        }

        public TMB.Reuters.RTNSConnection Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }

        public frmMain ParentControl
        {
            get;
            set;
        }

        private void BindBuyerPersons()
        {
            var bankusers = context
               .BankUsers
               .Where(w => w.BankID == (int)cbxBuyer_1.SelectedValue);

            cbxBuyerPerson.DataSource = bankusers.ToList();
            cbxBuyerPerson.DisplayMember = "Name";
            cbxBuyerPerson.ValueMember = "ID";
            if (bankusers.Count() > 0)
            {
                if (newDeal)
                {
                    cbxBuyerPerson.SelectedItem = bankusers.First();
                    cbxBuyerPerson.SelectedValue = bankusers.First().ID;
                }
                else
                {
                    cbxBuyerPerson.SelectedItem = transaction.TransactionLegs[0].BankUser;
                    cbxBuyerPerson.SelectedValue = transaction.TransactionLegs[0].BuyerPersonID;
                }
                
                // transaction.TransactionLegs[0].BankUser = bankusers.First();
                // transaction.TransactionLegs[0].BuyerPersonID = bankusers.First().ID;
            } 
        }

        private void BindSellerPersons()
        {
            var bankusers = context.BankUsers.Where(w => w.BankID == (int)cbxSeller_1.SelectedValue);
            cbxSellerPerson.DataSource = bankusers.ToList();
            cbxSellerPerson.DisplayMember = "Name";
            cbxSellerPerson.ValueMember = "ID";

            if (bankusers.Count() > 0)
            {
                if (newDeal)
                {
                    cbxSellerPerson.SelectedItem = bankusers.First();
                    cbxSellerPerson.SelectedValue = bankusers.First().ID;
                }
                else
                {
                    cbxSellerPerson.SelectedItem = transaction.TransactionLegs[0].BankUser1;
                    cbxSellerPerson.SelectedValue = transaction.TransactionLegs[0].SellerPersonID;
                }
                // transaction.TransactionLegs[0].BankUser1 = bankusers.First();
                // transaction.TransactionLegs[0].SellerPersonID = bankusers.First().ID;
            }
        }

        #region auto update Buyer and Seller
        private void cbxBuyer_1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg A selected Buyer changed, set the Seller of Leg B   
            /*if (isMultiLeg)
            {
                cbxSeller_2.SelectedText = cbxBuyer_1.SelectedText;
                transaction.TransactionLegs[1].Bank1 = (Bank)cbxBuyer_1.SelectedItem;
            }

            BindBuyerPersons();            */
        }

        private void cbxSeller_2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg B selected Seller changed, set the Buyer of Leg A                        
            cbxBuyer_1.SelectedText = cbxSeller_2.SelectedText;            
            transaction.TransactionLegs[0].Bank = (Bank)cbxSeller_2.SelectedItem;
        }

        private void cbxSeller_1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg A selected Seller changed, set the Buyer of Leg B  
            /*if (isMultiLeg)
            {
                cbxBuyer_2.SelectedText = cbxSeller_1.SelectedText;
                transaction.TransactionLegs[1].Bank = (Bank)cbxSeller_1.SelectedItem;
            }

            BindSellerPersons();            */
        }

        private void cbxBuyer_2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg B selected Bueyer changed, set the Seller of Leg A            
            cbxSeller_1.SelectedText = cbxBuyer_2.SelectedText;
            transaction.TransactionLegs[0].Bank1 = (Bank)cbxBuyer_2.SelectedItem;
        }
        #endregion

        #region auto update Currencies
        private void cbxByerCurrency_1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg A Buyer currency changed, set Buyer currency of Leg B   
            /*if (isMultiLeg)
            {
                cbxByerCurrency_2.SelectedText = cbxByerCurrency_1.SelectedText;
                transaction.TransactionLegs[1].Currency = (Currency)cbxByerCurrency_1.SelectedItem;
            }
            ComputeDescription();*/
        }

        private void cbxSellerCurrency_1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg A Seller currency changed, set the Seller currency of Leg B        
            /*if (isMultiLeg)
            {
                cbxSellerCurrency_2.SelectedText = cbxSellerCurrency_1.SelectedText;
                transaction.TransactionLegs[1].Currency1 = (Currency)cbxSellerCurrency_1.SelectedItem;
            }
            ComputeDescription();*/
        }

        private void cbxByerCurrency_2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg B Buyer currency changed, set the Bueyer currency of Leg A  
            cbxByerCurrency_1.SelectedText = cbxByerCurrency_2.SelectedText;
            transaction.TransactionLegs[0].Currency = (Currency)cbxByerCurrency_2.SelectedItem;

            // ComputeDescription();
        }

        private void cbxSellerCurrency_2_SelectionChangeCommitted(object sender, EventArgs e)
        {            
            // Leg B Seller Currency changed, set the Buyer currency of Leg A            
            cbxSellerCurrency_1.SelectedText = cbxSellerCurrency_2.SelectedText;
            transaction.TransactionLegs[0].Currency1 = (Currency)cbxSellerCurrency_2.SelectedItem;

            // ComputeDescription();
        }
        #endregion

        private void txtAmount_1_TextChanged(object sender, EventArgs e)
        {
            if (productID == 5)
            {
                ComputeMaturity(txtRate_1);
            }
            else
            {
                // Leg A Amount changed, set the Leg B amount
                txtAmount_2.Text = txtAmount_1.Text;
            }
            
            // Update the Summaries
            if(!inBinding)
                CalculateSummaries();
        }

        private void cbxAmountType_1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // Leg A Amount Type changed, set the Leg B amount Type
            /*if (isMultiLeg)
            {
                cbxAmountType_2.SelectedText = cbxAmountType_1.SelectedText;
                transaction.TransactionLegs[1].AmountType1 = (AmountType)cbxAmountType_1.SelectedItem;
            } */          
        }

        private void txtRate_1_TextChanged(object sender, EventArgs e)
        {
            // If this is the MM Deposit product
            if (productID == 5)
            {
                ComputeMaturity(txtRate_1);
            }
            else
            {
                // Leg A rate changed, update the forward rate            
                if (txtAllIn.Visible)
                    ComputeForwardRate(txtAllIn);
                else
                    ComputeForwardRate(txtRate_2);
            }

            // Update the Summaries
            if (!inBinding)
                CalculateSummaries();
        }

        private void txtPoints_1_TextChanged(object sender, EventArgs e)
        {
            if (txtAllIn.Visible)
                ComputeForwardRate(txtAllIn);
            else
                ComputeForwardRate(txtRate_2);
            if (!inBinding)
                CalculateSummaries();
        }

        private void ComputeForwardRate(TextBox txtForwardRate)
        {
            double rate = 0;
            double points = 0;
            int scale = GetCurrencyScale(GetBaseCurrency(),GetDealtCurrency());
            double divisor = Math.Pow(10,scale);            

            Double.TryParse(txtRate_1.Text, out rate);
            Double.TryParse(txtPoints_1.Text, out points);
            points = points / divisor;
            txtForwardRate.Text = Convert.ToString(rate + points);
        }
        private void ComputeDescription()
        {
            txtDescription.Text = string.Format("{0} {1} v {2}, {3} AND {4}",
                cbxProduct.Text,
                (this.cbxByerCurrency_1.Text == "Buyer Currency...") ? string.Empty : this.cbxByerCurrency_1.Text,
                (this.cbxSellerCurrency_1.Text == "Seller Currency...") ? string.Empty : this.cbxSellerCurrency_1.Text,
                (this.cbxBuyer_1.Text == "Buyer...") ? string.Empty : this.cbxBuyer_1.Text,
                (this.cbxSeller_1.Text == "Seller...") ? string.Empty : this.cbxSeller_1.Text);

            transaction.Description = txtDescription.Text;

            // Also set the currency summaries
            CalculateSummaries();
        }

        private void ComputeMaturity(TextBox txtInterestRate)
        {
            // Maturity ( (Amount*(RATE/100)) /DAY_YEAR_Basis * (Maturity Date - Start Date) )
            double amt = 0;
            double ir = 0;
            double daysPerYear = 0;
            double numDays = 1;

            Double.TryParse(txtAmount_1.Text, out amt);
            Double.TryParse(txtInterestRate.Text, out ir);
            Double.TryParse(txtDaysPerYear.Text, out daysPerYear);

            double dealAmount = GetTransactionAmountFromText(amt, cbxAmountType_1.Text);

            double AnnualInterest = dealAmount * (ir / 100);
            double dailyInterest = AnnualInterest / daysPerYear;
            double interestAtMaturity = dailyInterest * numDays;

            txtInterest.Text = interestAtMaturity.ToString("N");
            txtMaturity.Text = (dealAmount + interestAtMaturity).ToString("N");
        }

        private void CalculateSummaries()
        {
            // txtBuySummary.Text = ""

            double amt = (txtAmount_1.Text != string.Empty)
                ? Convert.ToDouble(txtAmount_1.Text)
                : 0d;
            double dealAmount = GetTransactionAmountFromText(amt, cbxAmountType_1.Text);
            double dealRate = (txtRate_1.Text == string.Empty) ? 1d : Convert.ToDouble(txtRate_1.Text);

            // If DEALT Currency is term then the rate is 1/rate
            if (cbxDealtCurrency.SelectedIndex > 0)
                dealRate = 1d / dealRate;

            // Buy currency is set as base AND Dealt Currency is Base currency
            // OR Sell currency is set as base AND Dealt Currency is set as Term Currency
            if (((rbBaseBuyer.Checked) && (cbxDealtCurrency.SelectedIndex == 0))
                || ((rbBaseSeller.Checked) && (cbxDealtCurrency.SelectedIndex > 0)))
            {
                txtBuySummary.Text = "BUY: " + dealAmount.ToString("N") + " " + ((this.cbxByerCurrency_1.Text == "Buyer Currency...") ? string.Empty : this.cbxByerCurrency_1.Text);
                txtSellSummary.Text = "SELL: " + (dealAmount * dealRate).ToString("N") + " " + ((this.cbxSellerCurrency_1.Text == "Seller Currency...") ? string.Empty : this.cbxSellerCurrency_1.Text);
            }
            else
            {
                txtSellSummary.Text = "SELL: " + dealAmount.ToString("N") + " " + ((this.cbxSellerCurrency_1.Text == "Seller Currency...") ? string.Empty : this.cbxSellerCurrency_1.Text);
                txtBuySummary.Text = "BUY: " + (dealAmount * dealRate).ToString("N") + " " + ((this.cbxByerCurrency_1.Text == "Buyer Currency...") ? string.Empty : this.cbxByerCurrency_1.Text);
            }

            // Lets also show the forward leg's summary
            if (isMultiLeg)
            {
                // Lets make sure we're using the correct forward rate.
                ComputeForwardRate(txtRate_2);

                double forwardRate = (txtRate_2.Text == string.Empty) ? 1d : Convert.ToDouble(txtRate_2.Text);
                // If DEALT Currency is term then the forward rate is 1/forwardrate
                if (cbxDealtCurrency.SelectedIndex > 0)
                    forwardRate = 1d / forwardRate;

                if (((rbBaseBuyer.Checked) && (cbxDealtCurrency.SelectedIndex == 0))
               || ((rbBaseSeller.Checked) && (cbxDealtCurrency.SelectedIndex > 0)))
                {
                    txtBuySummary_2.Text = "BUY: " + dealAmount.ToString("N") + " " + ((this.cbxByerCurrency_2.Text == "Buyer Currency...") ? string.Empty : this.cbxByerCurrency_2.Text);
                    txtSellSummary_2.Text = "SELL: " + (dealAmount * forwardRate).ToString("N") + " " + ((this.cbxSellerCurrency_2.Text == "Seller Currency...") ? string.Empty : this.cbxSellerCurrency_2.Text);
                }
                else
                {
                    txtSellSummary_2.Text = "SELL: " + dealAmount.ToString("N") + " " + ((this.cbxSellerCurrency_2.Text == "Seller Currency...") ? string.Empty : this.cbxSellerCurrency_2.Text);
                    txtBuySummary_2.Text = "BUY: " + (dealAmount * forwardRate).ToString("N") + " " + ((this.cbxByerCurrency_2.Text == "Buyer Currency...") ? string.Empty : this.cbxByerCurrency_2.Text);
                }
            }
        }

        private string GetBaseCurrency()
        {
            if (rbBaseBuyer.Checked)
                return cbxByerCurrency_1.Text;
            else
                return cbxSellerCurrency_1.Text;
        }

        private string GetDealtCurrency()
        {
            if (rbBaseBuyer.Checked)
                return cbxSellerCurrency_1.Text;
            else
                return cbxByerCurrency_1.Text;
        }

        public double GetTransactionAmountFromText(double amt, string amountType)
        {
            double multiplier = 1d;
            var amtType = context.AmountTypes.Where(a => a.Code == amountType).FirstOrDefault();
            if ((amtType != null) && (amtType.Multiplier.HasValue))
                multiplier = amtType.Multiplier.Value;

            amt = amt * multiplier;
            return amt;
        }

        private void cbxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComputeDescription();

            if (cbxProduct.SelectedItem != null)
                gpLegB.Visible = (((Product)cbxProduct.SelectedItem).IsMultiLeg.Value == 1);
        }

        private void cbxByerCurrency_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inBinding)
            {
                if (isMultiLeg)
                {
                    cbxByerCurrency_2.SelectedText = cbxByerCurrency_1.SelectedText;
                    transaction.TransactionLegs[1].Currency = (Currency)cbxByerCurrency_1.SelectedItem;
                }          
                
                if ((transaction !=null) && (transaction.Product.ID == 4))
                {
                    AddCurrency();
                }      

                ComputeDescription();
            }
        }

        private void cbxSellerCurrency_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inBinding)
            {
                if (isMultiLeg)
                {
                    cbxSellerCurrency_2.SelectedText = cbxSellerCurrency_1.SelectedText;
                    transaction.TransactionLegs[1].Currency1 = (Currency)cbxSellerCurrency_1.SelectedItem;
                }

                if ((transaction != null) && (transaction.Product.ID == 4))
                {
                    AddCurrency();
                }
                ComputeDescription();
            }
        }

        private void cbxBuyerPerson_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isMultiLeg && cbxBuyerPerson.SelectedValue != null)
            {
                transaction.TransactionLegs[1].SellerPersonID = (int)cbxBuyerPerson.SelectedValue;
            }
        }

        private void cbxSellerPerson_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isMultiLeg && cbxSellerPerson.SelectedValue != null)
            {
                transaction.TransactionLegs[1].BuyerPersonID = (int)cbxSellerPerson.SelectedValue;
            }
        }

        private void cbxTerm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTerm.Text == "Custom")
            {
                txtTerm_1.Text = string.Empty;
                txtTerm_1.Enabled = true;
            }
            else
            {
                txtTerm_1.Text = cbxTerm.Text;
                txtTerm_1.Enabled = false;

                if (isMultiLeg && !inBinding)
                    CheckTermDates(((Data.Term)cbxTerm.SelectedItem).ID);
            }
        }

        private void CheckTermDates(int termID)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["autoPopulateDate"] == "1")
            {
                var termDateValue = context
                  .TermDates
                  .Where(t => (t.ConfigDate >= DateTime.Now.Date) && (t.TermID == termID))
                  .FirstOrDefault();

                if (termDateValue == null)
                {
                    MessageBox.Show("Please note, your Term Dates are not configured for Today. Setting the Term will not effect the Future Date."
                        , "Term Dates"
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Exclamation);
                    return;
                }

                dt_LegDate_2.Value = termDateValue.ValueDate.Value;
                if (isMultiLeg)
                    transaction.TransactionLegs[1].ActionDate = termDateValue.ValueDate.Value;
            }
        }

        private void cbxBuyer_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inBinding)
            {
                if (isMultiLeg)
                {
                    cbxSeller_2.SelectedText = cbxBuyer_1.SelectedText;
                    transaction.TransactionLegs[1].Bank1 = (Bank)cbxBuyer_1.SelectedItem;
                }

                BindBuyerPersons();  

                ComputeDescription();
            }
        }

        private void cbxSeller_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inBinding)
            {
                if (isMultiLeg)
                {
                    cbxBuyer_2.SelectedText = cbxSeller_1.SelectedText;
                    transaction.TransactionLegs[1].Bank = (Bank)cbxSeller_1.SelectedItem;
                }

                    BindSellerPersons();

                ComputeDescription();
            }
        }

        private void cbxDealtCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inBinding)
                CalculateSummaries();
        }

        private void cbxAmountType_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!inBinding)
            {
                if (isMultiLeg)
                {
                    cbxAmountType_2.SelectedText = cbxAmountType_1.SelectedText;
                    transaction.TransactionLegs[1].AmountType1 = (AmountType)cbxAmountType_1.SelectedItem;
                } 
                CalculateSummaries();
                if (productID == 5)
                {
                    ComputeMaturity(txtRate_1);
                }
            }
        }

        private void rbBaseBuyer_CheckedChanged(object sender, EventArgs e)
        {
            if (!inBinding)
                CalculateSummaries();
        }

        private void rbBaseSeller_CheckedChanged(object sender, EventArgs e)
        {
            if (!inBinding)
                CalculateSummaries();
        }

        private void DealDetailControl_Validating(object sender, CancelEventArgs e)
        {
           
            // If we've picked up errors then cancel the validation event

            /*
                      e.Cancel = !isValid;  
             */
        }

        private bool IsValid()
        {
            // Lets start from scratch
            bool isValid = true;
            errorProvider1.Clear();

            // Make sure all the controls are set            
            if (((cbxByerCurrency_1.Text == "Buyer Currency...") || (cbxByerCurrency_1.Text == string.Empty)))
            {
                errorProvider1.SetError(cbxByerCurrency_1, "Near Leg Buyer currency not set");
                isValid = false;
            }
            if ((cbxSellerCurrency_1.Text == "Seller Currency...") || (cbxSellerCurrency_1.Text == string.Empty))
            {
                errorProvider1.SetError(cbxSellerCurrency_1, "Near Leg Seller currency not set");
                isValid = false;
            }
            if ((cbxBuyer_1.Text == "Buyer...") || (cbxBuyer_1.Text ==string.Empty))
            {
                errorProvider1.SetError(cbxBuyer_1, "Near Leg Buyer not set");
                isValid = false;
            }
            if ((cbxSeller_1.Text == "Seller...") || (cbxSeller_1.Text == string.Empty))
            {
                errorProvider1.SetError(cbxSeller_1, "Near Leg Seller not set");
                isValid = false;
            }
            if ((cbxAmountType_1.Text == "Amount Type...") || (cbxAmountType_1.Text == string.Empty))
            {
                errorProvider1.SetError(cbxAmountType_1, "Amount Type not set");
                isValid = false;
            }
            if (txtAmount_1.Text == string.Empty)
            {
                errorProvider1.SetError(txtAmount_1, "Near leg Amount not set");
                isValid = false;
            }
            if (txtRate_1.Text == string.Empty)
            {
                errorProvider1.SetError(txtRate_1, "Exchange rate not set for near leg");
                isValid = false;
            }

            if (isMultiLeg)
            {
                if (cbxTerm.Text == string.Empty)
                {
                    errorProvider1.SetError(cbxTerm, "Tenor not set for far leg.");
                    isValid = false;
                }
                if (txtPoints_1.Text == string.Empty)
                {
                    errorProvider1.SetError(txtPoints_1, "Points not set for far leg.");
                    isValid = false;
                }
            }

            // NDF checks
            if (pnlFix.Visible)
            {
                if (cbxFixincCentre.Text == "Fixing Centre...")
                {
                    errorProvider1.SetError(cbxFixincCentre,"Fixing centre not set.");
                    isValid = false;
                }
                if (cbxFixCurr.Text == "Settlement Currency...")
                {
                    errorProvider1.SetError(cbxFixCurr, "Settlement Currency not set.");
                    isValid = false;
                }    
            }

            // MM Deposit
            if (pnlMMDepo.Visible)
            {
                if (cbxPaymentFrequency.Text == "Payment Frequency...")
                {
                    errorProvider1.SetError(cbxPaymentFrequency, "Payment Frequence not set");
                    isValid = false;
                }
            }

            return isValid;
        }

        private void DealDetailControl_Validated(object sender, EventArgs e)
        {
            //errorProvider1.Clear();
        }

        private int GetCurrencyScale(string baseCurrency, string dealtCurrency)
        {
            //default to 4
            int scale = 4;
            var currencyScale = context
                .CurrencyScales
                .Where(w => (w.firstCurrency == baseCurrency) && (w.secondCurrency == dealtCurrency))
                .FirstOrDefault();

            if ((currencyScale != null) && (currencyScale.forwardScalePnts.HasValue))
            {
                transaction.Scale = currencyScale.forwardScalePnts.Value;
                return currencyScale.forwardScalePnts.Value;
            }

            currencyScale = context
                .CurrencyScales
                .Where(w => (w.firstCurrency == dealtCurrency) && (w.secondCurrency == baseCurrency))
                .FirstOrDefault();

            if ((currencyScale != null) && (currencyScale.inverseScalePnts.HasValue))
            {
                transaction.Scale = currencyScale.inverseScalePnts.Value;
                return currencyScale.inverseScalePnts.Value;
            }

            transaction.Scale = scale;
            return scale;
        }

        private void label25_Click(object sender, EventArgs e)
        {

        }
    }
}

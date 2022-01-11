using System.Linq;

namespace TMB.Data
{
    partial class TMBDataContext
    {        
        public Transaction NewTransaction(bool isMultiLeg, int? productID)
        {
            Transaction trxn = new Transaction();
            trxn.Status1 = Status.Where(w=>w.ID==1).FirstOrDefault();
            trxn.ProductType = 1;
            trxn.TransactionLegs.Add(new TransactionLeg { TransactionSide = 1 });            
            trxn.TransactionDate = System.DateTime.Now;
            trxn.spotdate = trxn.TransactionDate.Value.AddDays(2);
            trxn.CreatedOn = trxn.TransactionDate;

            trxn.TransactionLegs[0].BuyerID = -1;
            trxn.TransactionLegs[0].SellerID = -1;
            trxn.TransactionLegs[0].Transaction = trxn;
            trxn.TransactionLegs[0].SellerCurrency = -1;
            trxn.TransactionLegs[0].BuyerCurrency = -1;
            trxn.TransactionLegs[0].AmountType = -1;
            trxn.TransactionLegs[0].ActionDate = System.DateTime.Now;

            if (isMultiLeg)
            {
                trxn.TransactionLegs.Add(new TransactionLeg { TransactionSide = 2 });
                trxn.TransactionLegs[1].BuyerID = -1;
                trxn.TransactionLegs[1].SellerID = -1;
                trxn.TransactionLegs[1].Transaction = trxn;
                trxn.TransactionLegs[1].SellerCurrency = -1;
                trxn.TransactionLegs[1].BuyerCurrency = -1;
                trxn.TransactionLegs[1].AmountType = -1;
                trxn.TransactionLegs[1].ActionDate = System.DateTime.Now.AddDays(2);
            }

            if (productID.HasValue && (productID.Value == 4) && trxn.TransactionFixes == null)
            {
                trxn.TransactionFixes = new TransactionFix(); // create a new transactionfix entry
            }
            else if ((productID.HasValue && (productID.Value == 5) && trxn.TransactionMMs == null))
            {
                trxn.TransactionMMs = new TransactionMM(); // create a new TransactionMM entry
            }
            return trxn;
        }
        public double GetTransactionAmount(Transaction trxn)
        {            
            double amt = (trxn.TransactionLegs[0].Amount.HasValue)
                ? trxn.TransactionLegs[0].Amount.Value
                : 0d;
            double multiplier = 1d;
            if ((trxn.TransactionLegs[0].AmountType.HasValue) && (trxn.TransactionLegs[0].AmountType.Value > -1))            
                multiplier = trxn.TransactionLegs[0].AmountType1.Multiplier.Value;

            amt = amt * multiplier;
            return amt;
        }        
    }
}

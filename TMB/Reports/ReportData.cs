using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Reporting.WinForms;

namespace TMB.Reports
{
    public abstract class ReportData
    {
        public string ReportName { get; set; }
        public string ReportPath { get; set; }
        public Data.TMBDataContext Context { get; set; }

        public ReportData()
        {
            Context = new Data.TMBDataContext();
                      
        }
        
        public abstract void LoadData(ReportDataSourceCollection datasources, List<object> parms);
    }

    public class DealConfirmationReportData : ReportData
    {
        public DealConfirmationReportData()
            : base()
        {
            ReportPath = @".\Reports\dealconfirmation.rdlc";
            ReportName = "Daily Deal Confirmation Sheets";
        }

        override public void LoadData(ReportDataSourceCollection datasources, List<object> parms)
        {
            datasources.Clear();
            // Context = new Data.TMBDataContext();

            DateTime startDate = (DateTime)parms[0];
            DateTime endDate = (DateTime)parms[1];
            List<TMB.Data.Bank> banks = (List<TMB.Data.Bank>)parms[2];
            /*ReportDataSource rds = new ReportDataSource("TransactionData", Context
                .Transactions
                .Where(t =>
                    t.TransactionDate >= (DateTime)parms[0]
                    && t.TransactionDate < (DateTime)parms[1]));

            datasources.Add(rds);*/
            var broker = Context.Brokers.FirstOrDefault();
            TMB.Data.ReportParameters rp = new Data.ReportParameters();
            rp.FromCompany = broker.Name;
            rp.FromPerson = frmMain.StaticUserDisplayName;
            rp.Date = startDate.ToString("yyyy/MM/dd");
            if (!startDate.Equals(endDate))
                rp.Date = rp.Date + " - " + endDate.ToString("yyyy/MM/dd");
            
            rp.Fax = string.Empty;
            rp.Tel = "0123487577";
            rp.ToCompany = "Bank 1";
            rp.ToPerson = "Broker";
            rp.Product = "FX SWAP";

            if (broker.ContactDetails.HasValue)
            {
                rp.AddressLine1 = broker.ContactDetail.Address1;
                rp.AddressLine2 = broker.ContactDetail.Address2;
                rp.AddressLine3 = broker.ContactDetail.Address3;
                rp.AddressLine4 = broker.ContactDetail.Address4;
            }
            ReportDataSource reportParms = new ReportDataSource("ReportParameters",new List<TMB.Data.ReportParameters>{rp});
            datasources.Add(reportParms);

            var selectedBankIDs = from b in banks
                                  select b.ID;
            var details = from vw in Context.vwDealConfirmations
                          where (selectedBankIDs.Contains(vw.BuyerID.Value) || selectedBankIDs.Contains(vw.SellerID.Value))
                          && (vw.TransactionDate >= startDate && vw.TransactionDate <= endDate)
                          select vw;
            
            ReportDataSource dealConfirmation = new ReportDataSource("DealConfirmation", details);
            datasources.Add(dealConfirmation);
        }
    }

    public class DealTicketReportData : ReportData
    {
        public DealTicketReportData()
            : base()
        {
            ReportPath = @".\Reports\dealticketsheet.rdlc";
            ReportName = "Deal Ticket Sheet";
        }

        override public void LoadData(ReportDataSourceCollection datasources, List<object> parms)
        {
            datasources.Clear();

            DateTime startDate = (DateTime)parms[0];
            DateTime endDate = (DateTime)parms[1];
            List<TMB.Data.Bank> banks = (List<TMB.Data.Bank>)parms[2];
            var broker = Context.Brokers.FirstOrDefault();
            TMB.Data.ReportParameters rp = new Data.ReportParameters();
            rp.FromCompany = broker.Name;
            rp.FromPerson = frmMain.StaticUserDisplayName;
            rp.Date = startDate.ToString("yyyy/MM/dd");
            if (!startDate.Equals(endDate))
                rp.Date = rp.Date + " - " + endDate.ToString("yyyy/MM/dd");
            ReportDataSource reportParms = new ReportDataSource("ReportParameters", new List<TMB.Data.ReportParameters> { rp });
            datasources.Add(reportParms);

            var selectedBankIDs = from b in banks
                                  select b.ID;
            var details = from vw in Context.vwDealTickets
                          where (selectedBankIDs.Contains(vw.BuyerID.Value) || selectedBankIDs.Contains(vw.SellerID.Value))
                          && (vw.TransactionDate >= startDate && vw.TransactionDate <= endDate)
                          select vw;

            ReportDataSource dealConfirmationPerBankUser = new ReportDataSource("DealTickets", details);
            datasources.Add(dealConfirmationPerBankUser);

        }
    }

    public class DealConfirmationPerBankUserReportData : ReportData
    {
        public DealConfirmationPerBankUserReportData()
            : base()
        {
            ReportPath = @".\Reports\dealconfirmationPerBankUser.rdlc";
            ReportName = "Daily Deal Confirmation Sheets";
        }

        override public void LoadData(ReportDataSourceCollection datasources, List<object> parms)
        {
            datasources.Clear();
            // Context = new Data.TMBDataContext();

            DateTime startDate = (DateTime)parms[0];
            DateTime endDate = (DateTime)parms[1];
            List<TMB.Data.Bank> banks = (List<TMB.Data.Bank>)parms[2];
   
            var broker = Context.Brokers.FirstOrDefault();
            TMB.Data.ReportParameters rp = new Data.ReportParameters();
            rp.FromCompany = broker.Name;
            rp.FromPerson = frmMain.StaticUserDisplayName;
            rp.Date = startDate.ToString("yyyy/MM/dd");
            if (!startDate.Equals(endDate))
                rp.Date = rp.Date + " - " + endDate.ToString("yyyy/MM/dd");           

            if (broker.ContactDetails.HasValue)
            {
                rp.AddressLine1 = broker.ContactDetail.Address1;
                rp.AddressLine2 = broker.ContactDetail.Address2;
                rp.AddressLine3 = broker.ContactDetail.Address3;
                rp.AddressLine4 = broker.ContactDetail.Address4;
            }
            ReportDataSource reportParms = new ReportDataSource("ReportParameters", new List<TMB.Data.ReportParameters> { rp });
            datasources.Add(reportParms);

            var selectedBankIDs = from b in banks
                                  select b.ID;
            var details = from vw in Context.vwDealConfirmationPerBankUsers
                          where (selectedBankIDs.Contains(vw.DealID))
                          && (vw.TransactionDate >= startDate && vw.TransactionDate <= endDate)
                          select vw;
            
            ReportDataSource dealConfirmationPerBankUser = new ReportDataSource("DealConfirmationPerBankUser", details);
            datasources.Add(dealConfirmationPerBankUser);
        }
    }

    public class DealStatusReportData : ReportData
    {

        public DealStatusReportData()
            : base()
        {
            ReportPath = @".\Reports\dealStatusSheet.rdlc";
            ReportName = "Deal Status Sheet";
        }

        public override void LoadData(ReportDataSourceCollection datasources, List<object> parms)
        {
            datasources.Clear();

            DateTime startDate = (DateTime)parms[0];
            DateTime endDate = (DateTime)parms[1];
            List<TMB.Data.Bank> banks = (List<TMB.Data.Bank>)parms[2];
            var broker = Context.Brokers.FirstOrDefault();
            TMB.Data.ReportParameters rp = new Data.ReportParameters();
            rp.FromCompany = broker.Name;
            rp.FromPerson = frmMain.StaticUserDisplayName;
            rp.Date = startDate.ToString("yyyy/MM/dd");
            if (!startDate.Equals(endDate))
                rp.Date = rp.Date + " - " + endDate.ToString("yyyy/MM/dd");
            ReportDataSource reportParms = new ReportDataSource("ReportParameters", new List<TMB.Data.ReportParameters> { rp });
            datasources.Add(reportParms);

            var selectedBankIDs = from b in banks
                                  select b.ID;
            var details = from vw in Context.vwDealStatus
                          where (selectedBankIDs.Contains(vw.BuyerID.Value) || selectedBankIDs.Contains(vw.SellerID.Value))
                          && (vw.TransactionDate >= startDate && vw.TransactionDate <= endDate)
                          select vw;

            ReportDataSource dealConfirmationPerBankUser = new ReportDataSource("DealStatus", details);
            datasources.Add(dealConfirmationPerBankUser);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ExpatManager.Models;

namespace ExpatManager.DAL
{

          
    public class ExpatriateManagerInitializer : DropCreateDatabaseIfModelChanges<ExpatriateManagementContext>
    {
        protected override void Seed(ExpatriateManagementContext context)
        {
            var expatriates = new List<Expatriate>
            {
                new Expatriate {CIF="500017", FamilyStatusId=0, JobGradeId=1, TitleId=0, FirstName="Terence", LastName="Terence", CeilingValue=decimal.Parse("0.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CostCode="EE"},
                new Expatriate {CIF="500066", FamilyStatusId=0, JobGradeId=1, TitleId=0, FirstName="Jihad", LastName="Mikaiel",CeilingValue=decimal.Parse("0.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CostCode="ED"},
                new Expatriate {CIF="575449", FamilyStatusId=0, JobGradeId=1, TitleId=0, FirstName="Aman", LastName="Gupta",CeilingValue=decimal.Parse("0.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CostCode="ED"},
                new Expatriate {CIF="575506", FamilyStatusId=0, JobGradeId=1, TitleId=1, FirstName="Sofia", LastName="Marques",CeilingValue=decimal.Parse("0.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CostCode="LT"},
                new Expatriate {CIF="575704", FamilyStatusId=0, JobGradeId=1, TitleId=1, FirstName="Sachie", LastName="Kaneko",CeilingValue=decimal.Parse("0.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CostCode="LA"}
            };
            expatriates.ForEach(s => context.Expatriates.Add(s));
            context.SaveChanges();

            var expatriateHistorys = new List<ExpatriateHistory>
            {
                new ExpatriateHistory { ExpatriateID=1, FamilyStatusId=0, JobGradeId=1, Promotion="VP", CostCode="AP", DateOfPromotion=DateTime.Now.AddDays(-100), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new ExpatriateHistory { ExpatriateID=1, FamilyStatusId=1, JobGradeId=2, Promotion="P", CostCode="AP", DateOfPromotion=DateTime.Now.AddDays(-100), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new ExpatriateHistory { ExpatriateID=2, FamilyStatusId=0, JobGradeId=2, Promotion="VP", CostCode="AP", DateOfPromotion=DateTime.Now.AddDays(-100), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new ExpatriateHistory { ExpatriateID=3, FamilyStatusId=1, JobGradeId=2, Promotion="B", CostCode="AP", DateOfPromotion=DateTime.Now.AddDays(-100), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new ExpatriateHistory { ExpatriateID=4, FamilyStatusId=0, JobGradeId=2, Promotion="D", CostCode="AP", DateOfPromotion=DateTime.Now.AddDays(-100), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true }
            };

            expatriateHistorys.ForEach(s => context.ExpatriateHistorys.Add(s));
            context.SaveChanges();

            var agentDetails = new List<AgentDetail>
            {
                new AgentDetail { AgentName="KYO Service Co", BranchOffice="Harrow", ContactName="John Dow", Email=@"", Fax="", Address1="280 Preston Road", Address2="Harrow", Address3="Middlesex", PostCode="HA3 0QA", TelNo="020 8908 1135", Status=true, CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now},
            };

            agentDetails.ForEach(s => context.AgentDetails.Add(s));
            context.SaveChanges();

            var agreementDetails = new List<AgreementDetail>
            {
                new AgreementDetail { AgreementNo=1, LandlordBankDetailID=1, CostCode="AH", ExpatriateID=1, ExpireDate=DateTime.Today.AddDays(365), Length=12, MonthlyPayment=decimal.Parse("2300.00"), ProRataPayment=decimal.Parse("4039.18"), ProRataPaymentDate=DateTime.Today, StartDate=DateTime.Today, TerminationDate=DateTime.Today, TotalAmount=decimal.Parse("27600.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CouncilName="Greenwich", CouncilTaxAmount=Convert.ToDecimal("1270.00"), DepositAmount=Convert.ToDecimal("1380.00"), Address1="Flat 10 Breezers Court", Address2="12 The Highway", Address3="London", PostCode="E1 2AB", TelNo="02075771234", Reference="701 Lyndhurst CT"},
                new AgreementDetail { AgreementNo=2, LandlordBankDetailID=1, CostCode="AH", ExpatriateID=1, ExpireDate=DateTime.Today.AddDays(730), Length=12, MonthlyPayment=decimal.Parse("2300.00"), ProRataPayment=decimal.Parse("4029.00"), ProRataPaymentDate=DateTime.Today.AddDays(365), StartDate=DateTime.Today.AddDays(365), TerminationDate=DateTime.Today.AddDays(730), TotalAmount=decimal.Parse("27600.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true,  CouncilName="Tower Hamelets", CouncilTaxAmount=Convert.ToDecimal("1470.00"), DepositAmount=Convert.ToDecimal("1180.00"), Address1="Flat 10 Breezers Court", Address2="12 The Highway", Address3="London", PostCode="E1 2AB", TelNo="02075771234", Reference="15 ELGAR HOUSE"},
                new AgreementDetail { AgreementNo=1, LandlordBankDetailID=1, CostCode="ET", ExpatriateID=2, ExpireDate=DateTime.Today.AddDays(365), Length=12, MonthlyPayment=decimal.Parse("2300.00"), ProRataPayment=decimal.Parse("4019.00"), ProRataPaymentDate=DateTime.Today, StartDate=DateTime.Today, TerminationDate=DateTime.Today, TotalAmount=decimal.Parse("27600.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CouncilName="Lewisham", CouncilTaxAmount=Convert.ToDecimal("1470.00"), DepositAmount=Convert.ToDecimal("1180.00"), Address1="Flat 10 Breezers Court", Address2="12 The Highway", Address3="London", PostCode="E1 2AB", TelNo="02075771234", Reference="8100"}, 
                new AgreementDetail { AgreementNo=1, LandlordBankDetailID=1, CostCode="CS", ExpatriateID=3, ExpireDate=DateTime.Today.AddDays(365), Length=12, MonthlyPayment=decimal.Parse("2300.00"), ProRataPayment=decimal.Parse("4009.00"), ProRataPaymentDate=DateTime.Today, StartDate=DateTime.Today, TerminationDate=DateTime.Today, TotalAmount=decimal.Parse("27600.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CouncilName="Islington", CouncilTaxAmount=Convert.ToDecimal("1570.00"), DepositAmount=Convert.ToDecimal("1980.00"), Address1="Flat 10 Breezers Court", Address2="12 The Highway", Address3="London", PostCode="E1 2AB", TelNo="02075771234", Reference="11 CRESTA"},
                new AgreementDetail { AgreementNo=1, LandlordBankDetailID=1, CostCode="FA", ExpatriateID=4, ExpireDate=DateTime.Today.AddDays(365), Length=12, MonthlyPayment=decimal.Parse("2300.00"), ProRataPayment=decimal.Parse("4029.00"), ProRataPaymentDate=DateTime.Today, StartDate=DateTime.Today, TerminationDate=DateTime.Today, TotalAmount=decimal.Parse("27600.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true, CouncilName="Westminister", CouncilTaxAmount=Convert.ToDecimal("1670.00"), DepositAmount=Convert.ToDecimal("1680.00"),  Address1="Flat 10 Breezers Court", Address2="12 The Highway", Address3="London", PostCode="E1 2AB", TelNo="02075771234", Reference="2 ORMOND HOUSE"},
            };

            agreementDetails.ForEach(s => context.AgreementDetails.Add(s));
            context.SaveChanges();

            var agreementPayments = new List<AgreementPayment>
            {
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("1000.00"), PaymentDate=DateTime.Today, MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(1), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(2), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(3), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(4), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(5), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(6), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(7), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(8), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(9), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(10), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(11), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new AgreementPayment { AgreementDetailID=1, CostCode="OP", ProRataPayment=decimal.Parse("00.00"), PaymentDate=DateTime.Today.AddMonths(12), MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                //new AgreementPayment { AgreementDetailID=2, CostCode="AP", ProRataPayment=decimal.Parse("1500.00"), PaymentDate=DateTime.Today, MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                //new AgreementPayment { AgreementDetailID=3, CostCode="ET", ProRataPayment=decimal.Parse("1200.00"), PaymentDate=DateTime.Today, MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                //new AgreementPayment { AgreementDetailID=4, CostCode="SH", ProRataPayment=decimal.Parse("2000.00"), PaymentDate=DateTime.Today, MonthlyPayment=decimal.Parse("2,100.00"), TotalAmount=decimal.Parse("35,000.00"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
            };

            agreementPayments.ForEach(s => context.AgreementPayments.Add(s));
            context.SaveChanges();

            var ceilingTable = new List<CeilingTable>
            {
                new CeilingTable { JobGradeId=0, FamilyStatusId = 0, CeilingValue=decimal.Parse("2045"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=0, FamilyStatusId = 1, CeilingValue=decimal.Parse("2300"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=0, FamilyStatusId = 2, CeilingValue=decimal.Parse("2556"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                
                new CeilingTable { JobGradeId=1, FamilyStatusId = 0, CeilingValue=decimal.Parse("2272"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=1, FamilyStatusId = 1, CeilingValue=decimal.Parse("2556"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=1, FamilyStatusId = 2, CeilingValue=decimal.Parse("2840"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },

                new CeilingTable { JobGradeId=2, FamilyStatusId = 0, CeilingValue=decimal.Parse("2726"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=2, FamilyStatusId = 1, CeilingValue=decimal.Parse("3067"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=2, FamilyStatusId = 2, CeilingValue=decimal.Parse("3408"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },

                new CeilingTable { JobGradeId=3, FamilyStatusId = 0, CeilingValue=decimal.Parse("2726"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=3, FamilyStatusId = 1, CeilingValue=decimal.Parse("3067"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=3, FamilyStatusId = 2, CeilingValue=decimal.Parse("3408"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },

                new CeilingTable { JobGradeId=4, FamilyStatusId = 0, CeilingValue=decimal.Parse("3181"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=4, FamilyStatusId = 1, CeilingValue=decimal.Parse("3578"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=4, FamilyStatusId = 2, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },

                new CeilingTable { JobGradeId=5, FamilyStatusId = 0, CeilingValue=decimal.Parse("3181"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=5, FamilyStatusId = 1, CeilingValue=decimal.Parse("3578"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=5, FamilyStatusId = 2, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },

                new CeilingTable { JobGradeId=6, FamilyStatusId = 0, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=6, FamilyStatusId = 1, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=6, FamilyStatusId = 2, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },

                new CeilingTable { JobGradeId=7, FamilyStatusId = 0, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=7, FamilyStatusId = 1, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new CeilingTable { JobGradeId=7, FamilyStatusId = 2, CeilingValue=decimal.Parse("3976"), CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
            };

            ceilingTable.ForEach(s => context.CeilingTables.Add(s));
            context.SaveChanges();

            var familys = new List<Family>
            {
                new Family { FamilyTypeId=0,  DateOfBirth=DateTime.Now.AddYears(-49),   ArriveDate=DateTime.Now,  LeaveDate=DateTime.Now  , ExpatriateID=1, CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new Family { FamilyTypeId=1,  DateOfBirth=DateTime.Now.AddYears(-10),   ArriveDate=DateTime.Now,  LeaveDate=DateTime.Now  , ExpatriateID=1, CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                new Family { FamilyTypeId=0,  DateOfBirth=DateTime.Now.AddYears(-10),   ArriveDate=DateTime.Now,  LeaveDate=DateTime.Now  , ExpatriateID=2, CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
            };

            familys.ForEach(s => context.Familys.Add(s));
            context.SaveChanges();

            var landlordbankdetail = new List<LandlordBankDetail>
            {
                new LandlordBankDetail { AgentDetailID=1, AccountName="KYO Service Co",  BankName="Lloyds", BankAccountNo="09025854", SortCode="204080",  Status=true, CreateBy=HttpContext.Current.User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=HttpContext.Current.User.Identity.Name, ModifiedDateTime=DateTime.Now},
            };

            familys.ForEach(s => context.Familys.Add(s));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
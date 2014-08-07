using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpatManager.DAL;
using ExpatManager.Helper;
using ExpatManager.Models;
using PagedList;
using System.Web.Configuration;

namespace ExpatManager.Controllers
{
    public class AgreementPaymentController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /AgreementPayment/

        public ViewResult Index(string sortOrder, string currentFilter, string search, int? pageNo)
        {
            if (Request.HttpMethod == "GET")
            {
                search = currentFilter;
            }
            else
            {
                pageNo = 1;
            }

            ViewBag.CurrentFilter = search;

            var agreementPayments = from s in db.AgreementPayments
                                    select s;

            if (!String.IsNullOrEmpty(search))
            {
                agreementPayments = agreementPayments.Where(s =>
                    s.CostCode.Contains(search.ToUpper())
                    );
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm1 = String.IsNullOrEmpty(sortOrder) ? "Sort1 desc" : "";
            ViewBag.SortParm2 = sortOrder == "Sort2" ? "Sort2 desc" : "Sort2";
            ViewBag.SortParm3 = sortOrder == "Sort3" ? "Sort3 desc" : "Sort3";
            ViewBag.SortParm4 = sortOrder == "Sort4" ? "Sort4 desc" : "Sort4";
            ViewBag.SortParm5 = sortOrder == "Sort5" ? "Sort5 desc" : "Sort5";
            ViewBag.SortParm6 = sortOrder == "Sort6" ? "Sort6 desc" : "Sort6";
            ViewBag.SortParm7 = sortOrder == "Sort7" ? "Sort7 desc" : "Sort7";

            switch (sortOrder)
            {
                case "Sort1 desc":
                    agreementPayments = agreementPayments.OrderByDescending(s => s.AgreementPaymentID);
                    break;
                case "Sort2":
                    agreementPayments = agreementPayments.OrderBy(s => s.CostCode);
                    break;
                case "Sort2 desc":
                    agreementPayments = agreementPayments.OrderByDescending(s => s.CostCode);
                    break;
                case "Sort3":
                    agreementPayments = agreementPayments.OrderBy(s => s.ProRataPayment);
                    break;
                case "Sort3 desc":
                    agreementPayments = agreementPayments.OrderByDescending(s => s.ProRataPayment);
                    break;
                case "Sort4":
                    agreementPayments = agreementPayments.OrderBy(s => s.MonthlyPayment);
                    break;
                case "Sort4 desc":
                    agreementPayments = agreementPayments.OrderByDescending(s => s.MonthlyPayment);
                    break;
                case "Sort5":
                    agreementPayments = agreementPayments.OrderBy(s => s.TotalAmount);
                    break;
                case "Sort5 desc":
                    agreementPayments = agreementPayments.OrderByDescending(s => s.TotalAmount);
                    break;
                case "Sort6":
                    agreementPayments = agreementPayments.OrderBy(s => s.PaymentDate);
                    break;
                case "Sort6 desc":
                    agreementPayments = agreementPayments.OrderByDescending(s => s.PaymentDate);
                    break;
                case "Sort7":
                    agreementPayments = agreementPayments.OrderBy(s => s.Status);
                    break;
                case "Sort7 desc":
                    agreementPayments = agreementPayments.OrderByDescending(s => s.Status);
                    break;
                default:
                    agreementPayments = agreementPayments.OrderBy(s => s.AgreementPaymentID);
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);

            int pageIndex = (pageNo ?? 1);

            return View(agreementPayments.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /AgreementPayment/Details/5

        public ViewResult Details(int id)
        {
            AgreementPayment agreementpayment = db.AgreementPayments.Find(id);
            return View(agreementpayment);
        }

        //
        // GET: /AgreementPayment/Create

        public ActionResult Create(int id)
        {
            AgreementPayment agreementpayment = db.AgreementPayments.Find(id);

            agreementpayment.PaymentDate = agreementpayment.PaymentDate.AddMonths(1);
            agreementpayment.AgreementDetail = db.AgreementDetails.Find(agreementpayment.AgreementDetailID);
            /*
            

            agreementpayment.CostCode = agreementdetail.CostCode;
            agreementpayment.AgreementDetailID = agreementdetail.AgreementDetailID;
            if (agreementdetail.AgreementNo > 1)
            {
                agreementpayment.ProRataPayment = ExpatCalculation.ProRataPayment(agreementdetail.StartDate, agreementdetail.MonthlyPayment);
            }
            else
            {
                agreementpayment.ProRataPayment = ExpatCalculation.ProRataPaymentAndMonthly(agreementdetail.StartDate, agreementdetail.MonthlyPayment);
            }
            agreementpayment.MonthlyPayment = agreementdetail.MonthlyPayment;
            agreementpayment.TotalAmount = ExpatCalculation.TotalAmount(agreementdetail.MonthlyPayment);
            agreementpayment.PaymentDate = agreementdetail.StartDate;
            agreementpayment.CreateBy = HttpContext.User.Identity.Name;
            agreementpayment.CreateDateTime = DateTime.Now;
            agreementpayment.ModifiedBy = HttpContext.User.Identity.Name;
            agreementpayment.ModifiedDateTime = DateTime.Now;
            agreementpayment.Status = true;
             */
            return View(agreementpayment);
        }

        //
        // POST: /AgreementPayment/Create

        [HttpPost]
        public ActionResult Create(AgreementPayment agreementpayment, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    db.AgreementPayments.Add(agreementpayment);
                    db.SaveChanges();

                    return RedirectToAction("Details", "AgreementDetail", new { id = agreementpayment.AgreementDetailID.ToString() });
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(agreementpayment);
        }

        public ActionResult CreateMultiple(int id)
        {
            AgreementPayment agreementpayment = new AgreementPayment();
            AgreementDetail agreementdetail = db.AgreementDetails.Find(id);

            agreementpayment.CostCode = agreementdetail.CostCode;
            agreementpayment.AgreementDetailID = agreementdetail.AgreementDetailID;
            if (agreementdetail.AgreementNo > 1)
            {
                agreementpayment.ProRataPayment = ExpatCalculation.ProRataPayment(agreementdetail.StartDate, agreementdetail.MonthlyPayment);
            }
            else
            {
                agreementpayment.ProRataPayment = ExpatCalculation.ProRataPaymentAndMonthly(agreementdetail.StartDate, agreementdetail.MonthlyPayment);
            }
            agreementpayment.MonthlyPayment = agreementdetail.MonthlyPayment;
            agreementpayment.TotalAmount = ExpatCalculation.TotalAmount(agreementdetail.MonthlyPayment);
            agreementpayment.PaymentDate = agreementdetail.StartDate;
            agreementpayment.CreateBy = HttpContext.User.Identity.Name;
            agreementpayment.CreateDateTime = DateTime.Now;
            agreementpayment.ModifiedBy = HttpContext.User.Identity.Name;
            agreementpayment.ModifiedDateTime = DateTime.Now;
            agreementpayment.Status = true;
            return View(agreementpayment);
        }

        //
        // POST: /AgreementPayment/Create

        [HttpPost]
        public ActionResult CreateMultiple(AgreementPayment agreementpayment, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    DateTime PaymentDate = DateTime.Parse(formCollection.GetValue("PaymentDate").AttemptedValue);
                    //decimal ProRataPayment = ExpatCalculation.ProRataPaymentAndMonthly( PaymentDate, Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue));
                    decimal ProRataPayment = Decimal.Parse(formCollection.GetValue("ProRataPayment").AttemptedValue);
                    decimal FinalPayment = ExpatCalculation.FinalPayment(Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue), ProRataPayment);
                    decimal TotalPayment = ExpatCalculation.TotalAmount(Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue));

                    AgreementDetail agreementdetail = db.AgreementDetails.Find(agreementpayment.AgreementDetailID);

                    ExpatriateManagementContext context = new ExpatriateManagementContext();

                    if (PaymentDate.Day >= 10 && agreementdetail.AgreementNo == 1)
                    {
                        var agreementPayments = new List<AgreementPayment>
                        {
                            
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=ProRataPayment, PaymentDate=PaymentDate.AddMonths(0), MonthlyPayment=decimal.Parse("00.00"), TotalAmount=ProRataPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(1), MonthlyPayment=decimal.Parse("00.00"), TotalAmount=ProRataPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(2), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(3), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*2), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(4), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*3), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(5), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*4), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(6), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*5), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(7), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*6), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(8), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*7), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(9), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*8), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(10), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*9), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(11), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*10), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=PaymentDate.AddMonths(12), MonthlyPayment=FinalPayment, TotalAmount=TotalPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        };
                        agreementPayments.ForEach(s => context.AgreementPayments.Add(s));
                    }
                    else if (PaymentDate.Day < 10 && agreementdetail.AgreementNo == 1)
                    {

                        if (agreementdetail.ProRataPaymentDate.Month < PaymentDate.Month)
                        {
                            var agreementPayments = new List<AgreementPayment>
                            {
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=ProRataPayment, PaymentDate=PaymentDate.AddMonths(-1), MonthlyPayment=decimal.Parse("00.00"), TotalAmount=ProRataPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(0), MonthlyPayment=decimal.Parse("00.00"), TotalAmount=ProRataPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(1), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + agreementpayment.MonthlyPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(2), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*2), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(3), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*3), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(4), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*4), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(5), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*5), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(6), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*6), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(7), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*7), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(8), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*8), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(9), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*9), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(10), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*10), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(11), MonthlyPayment=FinalPayment, TotalAmount=TotalPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },

                            };
                            agreementPayments.ForEach(s => context.AgreementPayments.Add(s));
                        }
                        else
                        {
                            var agreementPayments = new List<AgreementPayment>
                            {
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=ProRataPayment, PaymentDate=PaymentDate, MonthlyPayment=decimal.Parse("00.00"), TotalAmount=ProRataPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(1), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + agreementpayment.MonthlyPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(2), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*2), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(3), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*3), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(4), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*4), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(5), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*5), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(6), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*6), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(7), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*7), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(8), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*8), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(9), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*9), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(10), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*10), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(11), MonthlyPayment=FinalPayment, TotalAmount=TotalPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                                };
                            agreementPayments.ForEach(s => context.AgreementPayments.Add(s));
                        }
                    }
                    else if (PaymentDate.Day < 10 && agreementdetail.AgreementNo > 1)
                    {
                        ProRataPayment = ExpatCalculation.ProRataPayment(DateTime.Parse(formCollection.GetValue("PaymentDate").AttemptedValue), Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue));
                        FinalPayment = ExpatCalculation.FinalPaymentRenewal(Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue), ProRataPayment);

                        var agreementPayments = new List<AgreementPayment>
                        {
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=ProRataPayment, PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(-1), MonthlyPayment=decimal.Parse("00.00"), TotalAmount=ProRataPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(0), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*1), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(1), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*2), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(2), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*3), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(3), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*4), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(4), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*5), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(5), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*6), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(6), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*7), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(7), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*8), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(8), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*9), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(9), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*10), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(10), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*11), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(11), MonthlyPayment=FinalPayment, TotalAmount=TotalPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        };
                        agreementPayments.ForEach(s => context.AgreementPayments.Add(s));
                    }
                    else if (PaymentDate.Day >= 10 && agreementdetail.AgreementNo > 1)
                    {
                        ProRataPayment = ExpatCalculation.ProRataPayment(DateTime.Parse(formCollection.GetValue("PaymentDate").AttemptedValue), Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue));
                        FinalPayment = ExpatCalculation.FinalPaymentRenewal(Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue), ProRataPayment);

                        var agreementPayments = new List<AgreementPayment>
                        {
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=ProRataPayment, PaymentDate=PaymentDate, MonthlyPayment=decimal.Parse("00.00"), TotalAmount=ProRataPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(1), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*1), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(2), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*2), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(3), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*3), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(4), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*4), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(5), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*5), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(6), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*6), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(7), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*7), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(8), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*8), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(9), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*9), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(10), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*10), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(11), MonthlyPayment=agreementpayment.MonthlyPayment, TotalAmount=ProRataPayment + (agreementpayment.MonthlyPayment*11), CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        new AgreementPayment { AgreementDetailID=agreementpayment.AgreementDetailID, CostCode=agreementpayment.CostCode, ProRataPayment=decimal.Parse("00.00"), PaymentDate=Convert.ToDateTime(PaymentDate).AddMonths(12), MonthlyPayment=FinalPayment, TotalAmount=TotalPayment, CreateBy=User.Identity.Name, CreateDateTime=DateTime.Now, ModifiedBy=User.Identity.Name, ModifiedDateTime=DateTime.Now, Status=true },
                        };
                        agreementPayments.ForEach(s => context.AgreementPayments.Add(s));
                    }

                    context.SaveChanges();

                    agreementdetail.ShowPaymentCreateLink = false;
                    db.Entry(agreementdetail).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "AgreementDetail", new { id = agreementpayment.AgreementDetailID.ToString() });
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(agreementpayment);
        }


        //
        // GET: /AgreementPayment/Edit/5

        public ActionResult Edit(int id)
        {
            AgreementPayment agreementpayment = db.AgreementPayments.Find(id);

            return View(agreementpayment);
        }

        //
        // POST: /AgreementPayment/Edit/5

        [HttpPost]
        public ActionResult Edit(AgreementPayment agreementpayment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    agreementpayment.ModifiedDateTime = DateTime.Now;
                    agreementpayment.ModifiedBy = HttpContext.User.Identity.Name;
                    db.Entry(agreementpayment).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "AgreementDetail", new { id = agreementpayment.AgreementDetailID.ToString() });
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(agreementpayment);
        }

        public ActionResult TerminationDate(int id)
        {
            AgreementPayment agreementpayment = new AgreementPayment();
            agreementpayment.AgreementDetailID = id;
            return View(agreementpayment);
        }

        //
        // POST: /AgreementPayment/Edit/5

        [HttpPost]
        public ActionResult TerminationDate(AgreementPayment agreementpayment)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                var _AgreementPayments = db.AgreementPayments
                    .Where(d => d.PaymentDate >= agreementpayment.PaymentDate && d.AgreementDetailID.Equals(agreementpayment.AgreementDetailID))
                    .AsNoTracking()
                    .ToList();

                Decimal TotalAmount = 0;
                int index = 0;
                foreach (var ap in _AgreementPayments)
                {

                    ap.ModifiedDateTime = DateTime.Now;
                    ap.ModifiedBy = HttpContext.User.Identity.Name;

                    if (index == 0)
                    {
                        TotalAmount = (ap.TotalAmount - ap.MonthlyPayment) + agreementpayment.MonthlyPayment;
                        ap.TotalAmount = TotalAmount;
                        ap.MonthlyPayment = agreementpayment.MonthlyPayment;
                    }
                    else
                    {
                        ap.MonthlyPayment = Decimal.Parse("0.00");
                        ap.TotalAmount = TotalAmount;
                    }

                    db.Entry(ap).State = EntityState.Modified;
                    index++;
                    //UpdateModel(ap);
                    db.SaveChanges();
                }

                AgreementDetail agreementDetail = db.AgreementDetails.Find(agreementpayment.AgreementDetailID);
                agreementDetail.TerminationDate = agreementpayment.PaymentDate;
                db.Entry(agreementDetail).State = EntityState.Modified;
                db.SaveChanges();
                //}
                return RedirectToAction("Details", "AgreementDetail", new { id = agreementpayment.AgreementDetailID.ToString() });
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(agreementpayment);
        }

        //
        // GET: /AgreementPayment/Delete/5

        public ActionResult Delete(int id)
        {
            AgreementPayment agreementpayment = db.AgreementPayments.Find(id);
            return View(agreementpayment);
        }

        //
        // POST: /AgreementPayment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                AgreementPayment AgreementPaymentToDelete = db.AgreementPayments.Find(id);
                int AgreementDetailId = AgreementPaymentToDelete.AgreementDetailID;
                db.Entry(AgreementPaymentToDelete).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Details", "AgreementDetail", new System.Web.Routing.RouteValueDictionary {
                        { "id", AgreementDetailId }});
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                return RedirectToAction("Delete",
                    new System.Web.Routing.RouteValueDictionary {
                        { "id", id },
                        { "saveChangesError", true }
                    });
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public AgreementPayment RunningTotal { get; set; }
    }
}
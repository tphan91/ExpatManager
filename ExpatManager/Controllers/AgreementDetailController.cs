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
using ExpatManager.ViewModels;
using PagedList;
using System.Web.Configuration;
using System.IO;

namespace ExpatManager.Controllers
{
    public class AgreementDetailController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();
        //private HRContext HR = new HRContext();
        //
        // GET: /AgreementDetail/

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

            var agreementDetails = from s in db.AgreementDetails
                                   select s;

            if (!String.IsNullOrEmpty(search))
            {
                agreementDetails = agreementDetails.Where(s =>
                    SqlFunctions.StringConvert((double)s.AgreementDetailID).Contains(search.ToUpper())
                    || s.Expatriates.FullName.ToUpper().Contains(search.ToUpper())
                    || s.CostCode.ToUpper().Contains(search.ToUpper())
                    || s.CouncilName.ToUpper().Contains(search.ToUpper())
                    || s.Address1.ToUpper().Contains(search.ToUpper())
                    || s.Address2.ToUpper().Contains(search.ToUpper())
                    || s.Address3.ToUpper().Contains(search.ToUpper())
                    || s.PostCode.ToUpper().Contains(search.ToUpper())
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
            ViewBag.SortParm8 = sortOrder == "Sort8" ? "Sort8 desc" : "Sort8";
            ViewBag.SortParm9 = sortOrder == "Sort9" ? "Sort9 desc" : "Sort9";
            ViewBag.SortParm10 = sortOrder == "Sort10" ? "Sort10 desc" : "Sort10";
            ViewBag.SortParm11 = sortOrder == "Sort11" ? "Sort11 desc" : "Sort11";
            ViewBag.SortParm12 = sortOrder == "Sort12" ? "Sort12 desc" : "Sort12";
            ViewBag.SortParm13 = sortOrder == "Sort13" ? "Sort13 desc" : "Sort13";

            switch (sortOrder)
            {
                case "Sort1 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => string.Concat(s.Expatriates.LastName, ", ", s.Expatriates.FirstName));
                    break;
                case "Sort2":
                    agreementDetails = agreementDetails.OrderBy(s => s.AgreementNo);
                    break;
                case "Sort2 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.AgreementNo);
                    break;
                case "Sort3":
                    agreementDetails = agreementDetails.OrderBy(s => s.CostCode);
                    break;
                case "Sort3 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.CostCode);
                    break;
                case "Sort4":
                    agreementDetails = agreementDetails.OrderBy(s => s.StartDate);
                    break;
                case "Sort4 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.StartDate);
                    break;
                case "Sort5":
                    agreementDetails = agreementDetails.OrderBy(s => s.ExpireDate);
                    break;
                case "Sort5 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.ExpireDate);
                    break;
                case "Sort6":
                    agreementDetails = agreementDetails.OrderBy(s => s.MonthlyPayment);
                    break;
                case "Sort6 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.MonthlyPayment);
                    break;
                case "Sort7":
                    agreementDetails = agreementDetails.OrderBy(s => s.ProRataPayment);
                    break;
                case "Sort7 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.ProRataPayment);
                    break;
                case "Sort8":
                    agreementDetails = agreementDetails.OrderBy(s => s.ProRataPaymentDate);
                    break;
                case "Sort8 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.ProRataPaymentDate);
                    break;
                case "Sort9":
                    agreementDetails = agreementDetails.OrderBy(s => s.TerminationDate);
                    break;
                case "Sort9 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.TerminationDate);
                    break;
                case "Sort10":
                    agreementDetails = agreementDetails.OrderBy(s => s.TotalAmount);
                    break;
                case "Sort10 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.TotalAmount);
                    break;
                case "Sort11":
                    agreementDetails = agreementDetails.OrderBy(s => s.CouncilName);
                    break;
                case "Sort11 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.CouncilName);
                    break;
                case "Sort12":
                    agreementDetails = agreementDetails.OrderBy(s => s.CouncilTaxAmount);
                    break;
                case "Sort12 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.CouncilTaxAmount);
                    break;
                case "Sort13":
                    agreementDetails = agreementDetails.OrderBy(s => s.DepositAmount);
                    break;
                case "Sort13 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.DepositAmount);
                    break;
                default:
                    agreementDetails = agreementDetails.OrderBy(s => string.Concat(s.Expatriates.LastName, ", ", s.Expatriates.FirstName));
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);
            int pageIndex = (pageNo ?? 1);

            return View(agreementDetails.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /AgreementDetail/Details/5

        public ViewResult Details(int id)
        {
            AgreementDetail agreementdetail = db.AgreementDetails.Find(id);
            if (DateTime.Parse(agreementdetail.TerminationDate.ToString()).ToShortDateString().Equals(DateTime.MaxValue.ToShortDateString())) { agreementdetail.TerminationDate = null; };
            return View(agreementdetail);
        }

        //
        // GET: /AgreementDetail/Print/5
        public ViewResult Print(int id)
        {
            AgreementDetail agreementdetail = db.AgreementDetails.Find(id);
            if (DateTime.Parse(agreementdetail.TerminationDate.ToString()).ToShortDateString().Equals(DateTime.MaxValue.ToShortDateString())) { agreementdetail.TerminationDate = null; };
            return View(agreementdetail);
        }

        //
        // GET: /AgreementDetail/Create

        public ActionResult Create(int id)
        {
            //PopulateExpatriateDropDownList(id);
            //PopulateAgentAndBankDropDownList(1);
            DateTime DateTimeNow = DateTime.Now;

            AgreementDetail agreementDetail = new AgreementDetail();
            agreementDetail.CreateBy = HttpContext.User.Identity.Name;
            agreementDetail.CreateDateTime = DateTimeNow;
            agreementDetail.ModifiedBy = HttpContext.User.Identity.Name;
            agreementDetail.ModifiedDateTime = DateTimeNow;
            agreementDetail.Length = 12;
            agreementDetail.StartDate = DateTimeNow;
            agreementDetail.ExpireDate = DateTimeNow;
            agreementDetail.ProRataPaymentDate = DateTimeNow;
            agreementDetail.TerminationDate = DateTime.MaxValue;
            ViewBag.EnumTitle = HtmlDropDownExtensions.ToSelectList(agreementDetail.DepartureReason);

            Expatriate expatriate = db.Expatriates.Find(id);
            agreementDetail.AgreementNo = expatriate.AgreementDetails.Count() + 1;
            agreementDetail.CostCode = expatriate.CostCode;
            agreementDetail.ExpatriateID = expatriate.ExpatriateID;
            agreementDetail.Expatriates = expatriate;
            agreementDetail.ShowBankCreateLink = true;
            agreementDetail.ShowPaymentCreateLink = true;
            agreementDetail.Status = true;

            //agreementDetail.CeilingBreach = expatriate.CeilingValue;

            vPersonJob _pj = dbHelper.vPersonJobFind(expatriate.CIF);

            if (_pj != null)
            {
                if (string.IsNullOrEmpty(_pj.TelNo))
                {
                    agreementDetail.TelNo = "";
                }
                else
                {
                    agreementDetail.TelNo = _pj.TelNo;
                }
            }


            return View(agreementDetail);
        }

        // POST: /AgreementDetail/Create
        [HttpPost]
        public ActionResult Create(AgreementDetail agreementdetail, int id, string key)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] landlordArray = key.Split('-');
                    if (key.IndexOf("-") > 0)
                    {
                        int LandlordBankDetailID = int.Parse(landlordArray.Last().Trim());
                        agreementdetail.LandlordBankDetailID = LandlordBankDetailID;
                        agreementdetail.CreateBy = HttpContext.User.Identity.Name;
                        agreementdetail.CreateDateTime = DateTime.Now;
                        agreementdetail.ModifiedBy = HttpContext.User.Identity.Name;
                        agreementdetail.ModifiedDateTime = DateTime.Now;
                        agreementdetail.TotalAmount = ExpatCalculation.TotalAmount(agreementdetail.MonthlyPayment);
                        agreementdetail.ProRataPayment = ExpatCalculation.ProRataPaymentAndMonthly(agreementdetail.StartDate, agreementdetail.MonthlyPayment);
                        //Agreement Payment link to only show if no payment has been created before.
                        agreementdetail.ShowBankCreateLink = true;
                        agreementdetail.ShowPaymentCreateLink = true;

                        //Calculate the ceiling breach 
                        agreementdetail.CeilingBreach = ExpatCalculation.CeilingBreach(agreementdetail.Expatriates.CeilingValue, agreementdetail.MonthlyPayment);

                        //Default value of expire and termination date should be 364 days
                        DateTime ExTerDate = agreementdetail.StartDate.AddDays(364);
                        agreementdetail.ExpireDate = ExTerDate;
                        agreementdetail.TerminationDate = DateTime.MaxValue;

                        agreementdetail.DepartureReason = (Enums.EnumDepartureReason)Enum.Parse(typeof(Enums.EnumDepartureReason), agreementdetail.DepartureReason.ToString());
                        
                        agreementdetail.Expatriates = null;
                        db.AgreementDetails.Add(agreementdetail);
                        db.SaveChanges();

                        AgreementDetailDocumentUpload documentUpload = new AgreementDetailDocumentUpload();
                        documentUpload.AgreementDetailID = agreementdetail.AgreementDetailID;
                        documentUpload.CreateBy = HttpContext.User.Identity.Name;
                        documentUpload.CreateDateTime = DateTime.Now;
                        documentUpload.ModifiedBy = HttpContext.User.Identity.Name;
                        documentUpload.ModifiedDateTime = DateTime.Now;
                        documentUpload.SecondChecker = false;
                        var fileName = "";
                        var fileSavePath = "";
                        int numFiles = Request.Files.Count;
                        int uploadedCount = 0;
                        for (int i = 0; i < numFiles; i++)
                        {
                            var uploadedFile = Request.Files[i];
                            if (uploadedFile.ContentLength > 0)
                            {
                                Guid newGuid = Guid.NewGuid();
                                fileName = string.Concat(newGuid, "_", Path.GetFileName(uploadedFile.FileName));
                                fileSavePath = Path.Combine(Server.MapPath("~/DocumentUpload"), fileName);
                                uploadedFile.SaveAs(fileSavePath);

                                documentUpload.FileType = uploadedFile.ContentType;
                                documentUpload.FileSize = uploadedFile.ContentLength;
                                documentUpload.FileName = fileName;
                                db.AgreementDetailDocumentUploads.Add(documentUpload);
                                db.SaveChanges();
                                uploadedCount++;
                            }
                        }

                        return RedirectToAction("Details", "Expatriate",
                            new System.Web.Routing.RouteValueDictionary {
                        { "id", agreementdetail.ExpatriateID },
                        { "saveChangesError", true }});
                    }
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            //PopulateExpatriateDropDownList(agreementdetail.ExpatriateID);
            //PopulateAgentAndBankDropDownList(0);
            //return RedirectToAction("Details", "Expatriate", id);
            return View(agreementdetail);
            //return RedirectToAction("Expatriate", "Index");
        }

        //
        // GET: /AgreementDetail/Create
        public ActionResult CreateExtend(int id)
        {
            AgreementDetail agreementdetail = db.AgreementDetails.Find(id);
            AgreementDetail newagreementdetail = new AgreementDetail();

            //PopulateAgentAndBankDropDownList(agreementdetail.LandlordBankDetailID);
            Expatriate expatriate = db.Expatriates.Find(agreementdetail.ExpatriateID);
            newagreementdetail.Expatriates = expatriate;

            newagreementdetail.Address1 = agreementdetail.Address1;
            newagreementdetail.Address2 = agreementdetail.Address2;
            newagreementdetail.Address3 = agreementdetail.Address3;
            newagreementdetail.PostCode = agreementdetail.PostCode;
            newagreementdetail.AgreementNo = agreementdetail.AgreementNo + 1;
            newagreementdetail.AgreementPayments = agreementdetail.AgreementPayments;
            newagreementdetail.CeilingBreach = agreementdetail.CeilingBreach;
            newagreementdetail.Comment = agreementdetail.Comment;
            newagreementdetail.CostCode = expatriate.CostCode;
            newagreementdetail.CouncilName = agreementdetail.CouncilNameProRata;
            //newagreementdetail.CouncilTaxAmount = agreementdetail.CouncilTaxAmountProRata;
            //newagreementdetail.CouncilTaxComment = agreementdetail.CouncilTaxCommentProRata;
            //newagreementdetail.DateSentToAP = agreementdetail.DateSentToAPProRata;
            newagreementdetail.CouncilTaxAccountReference = agreementdetail.CouncilTaxAccountReferenceProRata;
            newagreementdetail.AccountAuthorisation = agreementdetail.AccountAuthorisationProRata;
            newagreementdetail.CreateBy = agreementdetail.CreateBy;
            newagreementdetail.CreateDateTime = agreementdetail.CreateDateTime;
            newagreementdetail.DepositAmount = agreementdetail.DepositAmount;
            newagreementdetail.ExpatriateID = agreementdetail.ExpatriateID;
            newagreementdetail.ExpireDate = agreementdetail.ExpireDate.AddYears(1);
            //Default value of expire and termination date should be 364 days
            //AutoCompleteData(agreementdetail.LandlordBankDetails.LongBankDetails);
            newagreementdetail.LandlordBankDetails = agreementdetail.LandlordBankDetails;
            newagreementdetail.Length = agreementdetail.Length;
            newagreementdetail.ModifiedBy = agreementdetail.ModifiedBy;
            newagreementdetail.ModifiedDateTime = agreementdetail.ModifiedDateTime;
            newagreementdetail.MonthlyPayment = agreementdetail.MonthlyPayment;
            newagreementdetail.PostCode = agreementdetail.PostCode;
            newagreementdetail.ProRataPayment = agreementdetail.ProRataPayment;
            newagreementdetail.ProRataPaymentDate = agreementdetail.ProRataPaymentDate.AddYears(1);
            newagreementdetail.Reference = agreementdetail.Reference;
            newagreementdetail.ShowBankCreateLink = true;
            newagreementdetail.ShowPaymentCreateLink = true;
            newagreementdetail.StartDate = agreementdetail.StartDate.AddYears(1);
            newagreementdetail.Status = true;
            newagreementdetail.TelNo = agreementdetail.TelNo;
            newagreementdetail.TerminationDate = DateTime.MaxValue;
            newagreementdetail.TotalAmount = agreementdetail.TotalAmount;



            return View(newagreementdetail);
        }

        // POST: /AgreementDetail/Create
        [HttpPost]
        public ActionResult CreateExtend(AgreementDetail agreementdetail, int id, string key)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] agreementdetailArray = key.Split('-');
                    if (key.IndexOf("-") > 0)
                    {
                        int LandlordBankDetailID = int.Parse(agreementdetailArray.Last().Trim());
                        agreementdetail.LandlordBankDetailID = LandlordBankDetailID;
                        agreementdetail.CreateBy = HttpContext.User.Identity.Name;
                        agreementdetail.CreateDateTime = DateTime.Now;
                        agreementdetail.ModifiedBy = HttpContext.User.Identity.Name;
                        agreementdetail.ModifiedDateTime = DateTime.Now;
                        agreementdetail.TotalAmount = ExpatCalculation.TotalAmount(agreementdetail.MonthlyPayment);
                        agreementdetail.ProRataPayment = ExpatCalculation.ProRataPayment(agreementdetail.StartDate, agreementdetail.MonthlyPayment);

                        //Agreement Payment link to only show if no payment has been created before.
                        agreementdetail.ShowBankCreateLink = true;
                        agreementdetail.ShowPaymentCreateLink = true;

                        //Calculate the ceiling value breach 
                        agreementdetail.CeilingBreach = ExpatCalculation.CeilingBreach(agreementdetail.Expatriates.CeilingValue, agreementdetail.MonthlyPayment);

                        agreementdetail.Expatriates = null;
                        db.AgreementDetails.Add(agreementdetail);
                        db.SaveChanges();

                        AgreementDetailDocumentUpload documentUpload = new AgreementDetailDocumentUpload();
                        documentUpload.AgreementDetailID = agreementdetail.AgreementDetailID;
                        documentUpload.CreateBy = HttpContext.User.Identity.Name;
                        documentUpload.CreateDateTime = DateTime.Now;
                        documentUpload.ModifiedBy = HttpContext.User.Identity.Name;
                        documentUpload.ModifiedDateTime = DateTime.Now;

                        var fileName = "";
                        var fileSavePath = "";
                        int numFiles = Request.Files.Count;
                        int uploadedCount = 0;
                        for (int i = 0; i < numFiles; i++)
                        {
                            var uploadedFile = Request.Files[i];
                            if (uploadedFile.ContentLength > 0)
                            {
                                Guid newGuid = Guid.NewGuid();
                                fileName = string.Concat(newGuid, "_", Path.GetFileName(uploadedFile.FileName));
                                fileSavePath = Path.Combine(Server.MapPath("~/DocumentUpload"), fileName);
                                uploadedFile.SaveAs(fileSavePath);

                                documentUpload.FileType = uploadedFile.ContentType;
                                documentUpload.FileSize = uploadedFile.ContentLength;
                                documentUpload.FileName = fileName;
                                db.AgreementDetailDocumentUploads.Add(documentUpload);
                                db.SaveChanges();
                                uploadedCount++;
                            }
                        }

                        return RedirectToAction("Details", "Expatriate",
                            new System.Web.Routing.RouteValueDictionary {
                        { "id", agreementdetail.ExpatriateID },
                        { "saveChangesError", true }});
                    }
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            //PopulateExpatriateDropDownList(agreementdetail.ExpatriateID);
            //PopulateAgentAndBankDropDownList(0);
            //return RedirectToAction("Details", "Expatriate", id);
            return View(agreementdetail);
            //return RedirectToAction("Expatriate", "Index");
        }

        //
        // GET: /AgreementDetail/Edit/5
        public ActionResult Edit(int id)
        {
            AgreementDetail agreementdetail = db.AgreementDetails.Find(id);

            if (DateTime.Parse(agreementdetail.TerminationDate.ToString()).ToShortDateString().Equals(DateTime.MaxValue.ToShortDateString())) { agreementdetail.TerminationDate = null; };

            ViewBag.EnumTitle = HtmlDropDownExtensions.ToSelectList(agreementdetail.DepartureReason);

            //ViewBag.ExpatriateID = new SelectList(db.Expatriates, "ExpatriateID", "FullName", agreementdetail.ExpatriateID);
            //PopulateExpatriateDropDownList(agreementdetail.ExpatriateID);
            //PopulateAgentAndBankDropDownList(agreementdetail.LandlordBankDetailID);
            return View(agreementdetail);
        }

        //
        // POST: /AgreementDetail/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string[] LandlordArray = formCollection.GetValue("key").AttemptedValue.Split('-');
                    if (LandlordArray.Count() > 0)
                    {
                        int LandlordBankDetailID = int.Parse(LandlordArray.Last().Trim());

                        var agreementdetailToUpdate = db.AgreementDetails
                            //.Include(i => i.AgentDetails)
                         .Include(i => i.AgreementPayments)
                         .Include(i => i.AgreementDetailDocumentUploads)
                         .Include(i => i.LandlordBankDetails)
                         .Include(i => i.Expatriates)
                         .Where(i => i.AgreementDetailID == id)
                         .Single();

                        agreementdetailToUpdate.LandlordBankDetailID = LandlordBankDetailID;
                        agreementdetailToUpdate.ModifiedDateTime = DateTime.Now;
                        agreementdetailToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                        UpdateModel(agreementdetailToUpdate, "", null, new string[] { "AgreementPayments", "Expatriates", "LandlordBankDetails" });
                        decimal ceilingvalue = decimal.Parse(agreementdetailToUpdate.Expatriates.CeilingValue.ToString());
                        agreementdetailToUpdate.CeilingBreach = ceilingvalue - Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue);

                        //agreementdetailToUpdate.TotalAmount = Decimal.Parse(formCollection.GetValue("MonthlyPayment").AttemptedValue) * 12;

                        agreementdetailToUpdate.DepartureReason = (Enums.EnumDepartureReason)Enum.Parse(typeof(Enums.EnumDepartureReason), formCollection.GetValue("AgreementDetail.DepartureReason").AttemptedValue);

                        if (formCollection.GetValue("TerminationDate").AttemptedValue.Length > 1)
                        {
                            DateTime TerminationDate = DateTime.Parse(formCollection.GetValue("TerminationDate").AttemptedValue);
                            //TerminationDate between agreementdetailToUpdate.StartDate and agreementdetailToUpdate.ExpireDate

                            agreementdetailToUpdate.TerminationDate = TerminationDate;
                            agreementdetailToUpdate.ExpireDate = TerminationDate;
                        }
                        else
                        {
                            agreementdetailToUpdate.TerminationDate = DateTime.MaxValue;
                        }

                        if (formCollection.GetValue("Parkings.ParkingAmount").AttemptedValue != "" ||
                            formCollection.GetValue("Parkings.ParkingPaidBy").AttemptedValue != "" ||
                            formCollection.GetValue("Parkings.ParkingComment").AttemptedValue != "")
                        {
                            agreementdetailToUpdate.Parkings.ModifiedDateTime = DateTime.Now;
                            agreementdetailToUpdate.Parkings.ModifiedBy = HttpContext.User.Identity.Name;
                            if (agreementdetailToUpdate.Parkings.CreateDateTime == null)
                            {
                                agreementdetailToUpdate.Parkings.CreateDateTime = DateTime.Now;
                                agreementdetailToUpdate.Parkings.CreateBy = HttpContext.User.Identity.Name;
                            }
                        }

                        int expatriateId = agreementdetailToUpdate.ExpatriateID;
                        db.Entry(agreementdetailToUpdate).State = EntityState.Modified;
                        db.SaveChanges();
                        
                        return RedirectToAction("Details", "AgreementDetail", new System.Web.Routing.RouteValueDictionary {
                        { "id", id }});
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                        return View(formCollection);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    return View(formCollection);
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                AgreementDetail agreementdetail = db.AgreementDetails.Find(id);
                //ViewBag.ExpatriateID = new SelectList(db.Expatriates, "ExpatriateID", "FullName", agreementdetail.ExpatriateID);
                //PopulateExpatriateDropDownList(agreementdetail.ExpatriateID);

                //ViewBag.AgentDetails.AgentDetailID = new SelectList(db.AgentDetails, "AgentDetailID", "AgentName", agreementdetail.AgentDetails.AgentDetailID);
                //PopulateAgentAndBankDropDownList(agreementdetail.AgentDetails.AgentDetailID);
                return View(formCollection);
            }
        }

        //
        // GET: /AgreementDetail/Edit/5
        public ActionResult TerminationDate(int id)
        {
            AgreementDetail agreementdetail = db.AgreementDetails.Find(id);

            return View(agreementdetail);
        }

        //
        // POST: /AgreementDetail/Edit/5
        [HttpPost]
        public ActionResult TerminationDate(int id, FormCollection formCollection)
        {
            try
            {
                var agreementdetailToUpdate = db.AgreementDetails
                 .Include(i => i.AgentDetails)
                 .Include(i => i.AgreementPayments)
                 .Include(i => i.AgreementDetailDocumentUploads)
                 .Include(i => i.LandlordBankDetails)
                 .Include(i => i.Expatriates)
                 .Where(i => i.AgreementDetailID == id)
                 .Single();

                agreementdetailToUpdate.ModifiedDateTime = DateTime.Now;
                agreementdetailToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                UpdateModel(agreementdetailToUpdate, "", null, new string[] { "AgentDetails", "AgreementPayments", "Expatriates" });
                int expatriateId = agreementdetailToUpdate.ExpatriateID;
                db.Entry(agreementdetailToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Expatriate", new System.Web.Routing.RouteValueDictionary {
                        { "id", expatriateId }});
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                AgreementDetail agreementdetail = db.AgreementDetails.Find(id);
                //ViewBag.ExpatriateID = new SelectList(db.Expatriates, "ExpatriateID", "FullName", agreementdetail.ExpatriateID);
                //PopulateExpatriateDropDownList(agreementdetail.ExpatriateID);

                //ViewBag.AgentDetails.AgentDetailID = new SelectList(db.AgentDetails, "AgentDetailID", "AgentName", agreementdetail.AgentDetails.AgentDetailID);
                //PopulateAgentAndBankDropDownList(agreementdetail.AgentDetails.AgentDetailID);
                return View(formCollection);
            }
        }

        //
        // GET: /AgreementDetail/Delete/5
        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            AgreementDetail agreementdetail = db.AgreementDetails.Find(id);
            if (DateTime.Parse(agreementdetail.TerminationDate.ToString()).ToShortDateString().Equals(DateTime.MaxValue.ToShortDateString())) { agreementdetail.TerminationDate = null; };
            return View(agreementdetail);
        }

        //
        // POST: /AgreementDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AgreementDetail agreementDetail = db.AgreementDetails.Find(id);
            int ExpatriateId = agreementDetail.ExpatriateID;
            db.Entry(agreementDetail).State = EntityState.Detached;

            try
            {
                AgreementDetail AgreementDetailToDelete = new AgreementDetail() { AgreementDetailID = id };
                db.Entry(AgreementDetailToDelete).State = EntityState.Deleted;
                db.SaveChanges();
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

            return RedirectToAction("Details", "Expatriate", new System.Web.Routing.RouteValueDictionary {
                        { "id", ExpatriateId }});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        /*
        public void PopulateExpatriateDropDownList(object selected = null)
        {
            var Query = from d in db.Expatriates
                        orderby d.FirstName, d.LastName
                        select d;
            ViewBag.ExpatriateID = new SelectList(Query, "ExpatriateID", "FullName", selected);
        }

        public void PopulateAgentAndBankDropDownList(object selected = null)
        {
            var Query = from d in db.LandlordBankDetails
                        orderby d.AccountName, d.BankAccountNo, d.SortCode
                        select d;
            ViewBag.LandlordBankDetailID = new SelectList(Query, "LandlordBankDetailID", "ShortBankDetails", selected);
        }
        */
        public ActionResult AutoCompleteData(string term)
        {
            var results = from m in db.LandlordBankDetails
                          where m.AccountName.StartsWith(term) ||
                                m.AgentDetails.Address1.StartsWith(term) ||
                                m.AgentDetails.AgentName.StartsWith(term)
                          //SqlFunctions.StringConvert((double)m.PayRollNo).StartsWith(term)
                          select new
                          {
                              label = m.AccountName
                                  + ", "
                                  + m.AgentDetails.Address1
                                  + ", "
                                  + m.BankAccountNo
                                  + "/"
                                  + m.SortCode
                                  + " - "
                                  + SqlFunctions.StringConvert((double)m.LandlordBankDetailID).Trim(),
                              id = m.LandlordBankDetailID
                          };
            results.Select(x => x.label).Take(10);

            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
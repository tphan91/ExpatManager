using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpatManager.Models;
using ExpatManager.DAL;
using ExpatManager.Helper;
using PagedList;
using System.Web.Configuration;
using System.Configuration;

namespace ExpatManager.Controllers
{
    public class ExpatriateController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();
        //private HRContext HR = new HRContext();
        //
        // GET: /Expatriate/
        
        //[Authorize(Users = @"LDNR\6334mhoang")]
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

            var expatriates = (from s in db.Expatriates
                              select s);
            
            if (!String.IsNullOrEmpty(search))
            {
                expatriates = expatriates.Where(s =>
                    //SqlFunctions.StringConvert((Decimal)s.CIF).Contains(search.ToUpper())
                    s.CIF.Contains(search.ToUpper())
                    || s.FirstName.ToUpper().Contains(search.ToUpper())
                    || s.LastName.ToUpper().Contains(search.ToUpper())
                    || s.AgreementDetails.FirstOrDefault().Address1.Contains(search.ToUpper())
                    || s.AgreementDetails.FirstOrDefault().PostCode.ToUpper().Contains(search.ToUpper())
                    );
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm1 = String.IsNullOrEmpty(sortOrder) ? "Sort1 desc" : "";
            ViewBag.SortParm2 = sortOrder == "Sort2" ? "Sort2 desc" : "Sort2";
            ViewBag.SortParm3 = sortOrder == "Sort3" ? "Sort3 desc" : "Sort3";
            ViewBag.SortParm4 = sortOrder == "Sort4" ? "Sort4 desc" : "Sort4";
            ViewBag.SortParm5 = sortOrder == "Sort5" ? "Sort5 desc" : "Sort5";

            switch (sortOrder)
            {
                case "Sort1 desc":
                    expatriates = expatriates.OrderByDescending(s => s.CIF);
                    break;
                case "Sort2":
                    expatriates = expatriates.OrderBy(s => s.TitleId);
                    break;
                case "Sort2 desc":
                    expatriates = expatriates.OrderByDescending(s => s.TitleId);
                    break;
                case "Sort3":
                    expatriates = expatriates.OrderBy(s => (string.Concat(s.LastName, s.FirstName)));
                    break;
                case "Sort3 desc":
                    expatriates = expatriates.OrderByDescending(s => (string.Concat(s.LastName, s.FirstName)));
                    break;
                case "Sort4":
                    expatriates = expatriates.OrderBy(s => s.CostCode);
                    break;
                case "Sort4 desc":
                    expatriates = expatriates.OrderByDescending(s => s.CostCode);
                    break;
                case "Sort5":
                    expatriates = expatriates.OrderBy(s => s.JobGradeId);
                    break;
                case "Sort5 desc":
                    expatriates = expatriates.OrderByDescending(s => s.JobGradeId);
                    break;
                case "Sort6":
                    expatriates = expatriates.OrderBy(s => s.FamilyStatusId);
                    break;
                case "Sort6 desc":
                    expatriates = expatriates.OrderByDescending(s => s.FamilyStatusId);
                    break;
                case "Sort7":
                    expatriates = expatriates.OrderBy(s => s.Status);
                    break;
                case "Sort7 desc":
                    expatriates = expatriates.OrderByDescending(s => s.Status);
                    break;
                default:
                    expatriates = expatriates.OrderBy(s => string.Concat(s.LastName, s.FirstName));
                    break;
            }

            //int pageSize = Helper.StringParsing.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);
            int pageSize = 20;
            int pageIndex = (pageNo ?? 1);

            return View(expatriates.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Expatriate/Details/5

        public ViewResult Details(int id)
        {
            Expatriate expatriate = db.Expatriates.Find(id);
            
            return View(expatriate);
        }

        //
        // GET: /Expatriate/Create

        public ActionResult Create()
        {
            Expatriate expatriate = new Expatriate();
            expatriate.CreateBy = HttpContext.User.Identity.Name;
            expatriate.CreateDateTime = DateTime.Now;
            expatriate.ModifiedBy = HttpContext.User.Identity.Name;
            expatriate.ModifiedDateTime = DateTime.Now;
            expatriate.FamilyStatus = Enums.EnumFamilyStatus.SINGLE;
            expatriate.Status = true;
            return View(expatriate);
        }

        //
        // POST: /Expatriate/Create

        [HttpPost]
        public ActionResult Create(Expatriate expatriate, string key)
        {
            try
            {
                //if (key.IndexOf("-") > 0)
               //{
                 //   expatriate = null;
                //}

                if (ModelState.IsValid)
                {
                    string[] payRollNoArray = key.Split('-');
                    if (key.IndexOf("-") > 0)
                    {
                        String payRollNo = payRollNoArray.Last().Trim();
                        vPersonJob _pj = dbHelper.vPersonJobFind(payRollNo);

                        expatriate.CIF = _pj.PayRollNo;
                        expatriate.FirstName = _pj.FirstName;
                        expatriate.LastName = _pj.LastName;
                        expatriate.Comment = expatriate.Comment;
                        expatriate.CostCode = _pj.CostCode;
                        expatriate.Title = (Enums.EnumTitle)Enum.Parse(typeof(Enums.EnumTitle),_pj.Title.ToUpper());
                        expatriate.CreateBy = HttpContext.User.Identity.Name;
                        expatriate.CreateDateTime = DateTime.Now;
                        expatriate.ModifiedBy = HttpContext.User.Identity.Name;
                        expatriate.ModifiedDateTime = DateTime.Now;
                        expatriate.JobGradeId = expatriate.JobGradeId;
                        expatriate.FamilyStatusId = expatriate.FamilyStatusId;
                        expatriate.CeilingValue = ExpatCalculation.CeilingValue(expatriate.JobGradeId, expatriate.FamilyStatusId);

                        Expatriate NewExpat = db.Expatriates.Add(expatriate);
                        db.SaveChanges();

                        return RedirectToAction("Details", "Expatriate", new System.Web.Routing.RouteValueDictionary {
                        { "id", NewExpat.ExpatriateID }});
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Please make sure you have select a person in the auto complete name textbox. Try again, and if the problem persists see your system administrator.");
                        return View(expatriate);
                    }
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Please make sure you have select a person in the auto complete name textbox. Try again, and if the problem persists see your system administrator.");
                return View(expatriate);
            }
            return View(expatriate);
        }

        //
        // GET: /Expatriate/Edit/5

        public ActionResult Edit(int id)
        {
            Expatriate expatriate = db.Expatriates.Find(id);
            ViewBag.EnumTitle = HtmlDropDownExtensions.ToSelectList(expatriate.Title);
            ViewBag.JobGrade = HtmlDropDownExtensions.ToSelectList(expatriate.JobGrade);

            return View(expatriate);
        }

        //
        // POST: /Expatriate/Edit/5

        [HttpPost]
        public ActionResult Edit(int expatriateId, FormCollection formCollection, bool Status, bool GMFlag)
        {
            try
            {
                var expatriateToUpdate = db.Expatriates
                    .Include(i => i.ExpatriateHistorys)
                    .Include(i => i.Familys)
                    .Include(i => i.AgreementDetails)
                    .Where(i => i.ExpatriateID == expatriateId)
                    .Single();

                if (!expatriateToUpdate.JobGrade.Equals(formCollection.GetValue("Expatriate.JobGrade").AttemptedValue))
                {
                    expatriateToUpdate.JobGrade = (Enums.EnumJobGrade)Enum.Parse(typeof(Enums.EnumJobGrade), formCollection.GetValue("Expatriate.JobGrade").AttemptedValue);
                    expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                }

                expatriateToUpdate.CostCode = formCollection.GetValue("CostCode").AttemptedValue;
                expatriateToUpdate.Promotion = formCollection.GetValue("Promotion").AttemptedValue;
                
                DateTime dt;
                bool dateofpromotion = DateTime.TryParse( formCollection.GetValue("DateOfPromotion").AttemptedValue, out dt);
                if (dateofpromotion == true)
                { expatriateToUpdate.DateOfPromotion = dt; }
                else
                { expatriateToUpdate.DateOfPromotion = null; }
                expatriateToUpdate.Comment = formCollection.GetValue("Comment").AttemptedValue;
                expatriateToUpdate.GMFlag = GMFlag;
                expatriateToUpdate.Status = Status; 

                ExpatriateHistory expatriateHistory = dbHelper.CreateExpatriateHistory(expatriateToUpdate);
                expatriateHistory.ModifiedBy = HttpContext.User.Identity.Name;
                expatriateHistory.CreateBy = HttpContext.User.Identity.Name;
                db.ExpatriateHistorys.Add(expatriateHistory);

                expatriateToUpdate.ModifiedDateTime = DateTime.Now;
                expatriateToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                expatriateToUpdate.Title = (Enums.EnumTitle)Enum.Parse(typeof(Enums.EnumTitle), formCollection.GetValue("Expatriate.Title").AttemptedValue);
                if (!expatriateToUpdate.AgreementDetails.Count.Equals(0))
                {
                    expatriateToUpdate.AgreementDetails.LastOrDefault().CeilingBreach = expatriateToUpdate.CeilingValue - expatriateToUpdate.AgreementDetails.LastOrDefault().MonthlyPayment;
                }
                //UpdateModel(expatriateToUpdate, "", null, new string[] { "Familys" });

                db.Entry(expatriateToUpdate).State = EntityState.Modified;
                db.SaveChanges(); 
                
                return RedirectToAction("Details", "Expatriate", new System.Web.Routing.RouteValueDictionary {
                { "id", expatriateId }});
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                return View();
            }
        }

        //
        // GET: /Expatriate/Delete/5

        public ActionResult Delete(int id)
        {
            Expatriate expatriate = db.Expatriates.Find(id);
            return View(expatriate);
        }

        //
        // POST: /Expatriate/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Expatriate ExpatriateToDelete = new Expatriate() { ExpatriateID = id };
                db.Entry(ExpatriateToDelete).State = EntityState.Deleted;
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
            return RedirectToAction("Index");
        }

        /*
        public ActionResult AutoComplete(string key)
        {
            if (key == null)
            {
                key = string.Empty;
            }

            var results = from m in HR.PersonJobs
                          where m.FirstName.StartsWith(key) ||
                                m.LastName.StartsWith(key)
                          select new { label = m.FirstName, id = m.PersonJobID };
            results.Select(x => x.label).Take(10);
            return View(results);
        }
        */

        public ActionResult AutoCompleteData(string term)
        {
            var results = from m in db.PersonJobs
                          where m.FirstName.StartsWith(term) ||
                                m.LastName.StartsWith(term) ||
                                m.PayRollNo.StartsWith(term)
                                 //SqlFunctions.StringConvert((double)m.PayRollNo).StartsWith(term)
                          select new
                          {
                              label = m.LastName
                                  + ", "
                                  + m.FirstName
                                  + " - "
                                  + m.PayRollNo.Trim(),
                              id = m.PayRollNo
                          };
            results.Select(x => x.label).Take(10);

            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
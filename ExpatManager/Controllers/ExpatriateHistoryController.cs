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

namespace ExpatManager.Controllers
{
    public class ExpatriateHistoryController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /ExpatriateHistory/

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

            var expatriateHistorys = from s in db.ExpatriateHistorys
                              select s;

            if (!String.IsNullOrEmpty(search))
            {

                expatriateHistorys = expatriateHistorys.Where(s =>
                    s.Promotion.ToUpper().Contains(search.ToUpper())
                    || s.Expatriates.FirstName.ToUpper().Contains(search.ToUpper())
                    || s.Expatriates.LastName.ToUpper().Contains(search.ToUpper())
                    || s.CostCode.ToUpper().Contains(search.ToUpper())
                    );
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm1 = String.IsNullOrEmpty(sortOrder) ? "Sort1 desc" : "";
            ViewBag.SortParm2 = sortOrder == "Sort2" ? "Sort2 desc" : "Sort2";
            ViewBag.SortParm3 = sortOrder == "Sort3" ? "Sort3 desc" : "Sort3";
            ViewBag.SortParm4 = sortOrder == "Sort4" ? "Sort4 desc" : "Sort4";
            ViewBag.SortParm5 = sortOrder == "Sort5" ? "Sort5 desc" : "Sort5";
            ViewBag.SortParm5 = sortOrder == "Sort6" ? "Sort6 desc" : "Sort6";

            switch (sortOrder)
            {
                case "Sort1 desc":
                    expatriateHistorys = expatriateHistorys.OrderByDescending(s => (string.Concat(s.Expatriates.LastName, s.Expatriates.FirstName)));
                    break;
                case "Sort2":
                    expatriateHistorys = expatriateHistorys.OrderBy(s => s.JobGrade);
                    break;
                case "Sort2 desc":
                    expatriateHistorys = expatriateHistorys.OrderByDescending(s => s.JobGrade);
                    break;
                case "Sort3":
                    expatriateHistorys = expatriateHistorys.OrderBy(s => s.CostCode);
                    break;
                case "Sort3 desc":
                    expatriateHistorys = expatriateHistorys.OrderByDescending(s => s.CostCode);
                    break;
                case "Sort4":
                    expatriateHistorys = expatriateHistorys.OrderBy(s => s.Promotion);
                    break;
                case "Sort4 desc":
                    expatriateHistorys = expatriateHistorys.OrderByDescending(s => s.Promotion);
                    break;
                case "Sort5":
                    expatriateHistorys = expatriateHistorys.OrderBy(s => s.DateOfPromotion);
                    break;
                case "Sort5 desc":
                    expatriateHistorys = expatriateHistorys.OrderByDescending(s => s.DateOfPromotion);
                    break;
                case "Sort6":
                    expatriateHistorys = expatriateHistorys.OrderBy(s => s.Status);
                    break;
                case "Sort6 desc":
                    expatriateHistorys = expatriateHistorys.OrderByDescending(s => s.Status);
                    break;
                default:
                    expatriateHistorys = expatriateHistorys.OrderBy(s => (string.Concat(s.Expatriates.LastName, s.Expatriates.FirstName)));
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);
            int pageIndex = (pageNo ?? 1);

            return View(expatriateHistorys.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /ExpatriateHistory/Details/5

        public ViewResult Details(int id)
        {
            ExpatriateHistory expatriatehistory = db.ExpatriateHistorys.Find(id);
            return View(expatriatehistory);
        }

        //
        // GET: /ExpatriateHistory/Create

        public ActionResult Create(int id)
        {
            ExpatriateHistory expatriateHistory = new ExpatriateHistory();

            expatriateHistory.CreateBy = HttpContext.User.Identity.Name;
            expatriateHistory.CreateDateTime = DateTime.Now;
            expatriateHistory.ModifiedBy = HttpContext.User.Identity.Name;
            expatriateHistory.ModifiedDateTime = DateTime.Now;
            //expatriateHistory.JobGradei = Enums.EnumJobGrade.SL.ToString("");
            //expatriateHistory.FamilyStatus = Enums.EnumFamilyStatus.Single.ToString("");
            expatriateHistory.Status = true;
            Expatriate expatriate = db.Expatriates.Find(id);
            expatriateHistory.Expatriates = expatriate;
            expatriateHistory.Title = expatriate.Title;
            expatriateHistory.CostCode = expatriate.CostCode;
            expatriateHistory.JobGradeId = expatriate.JobGradeId;
            expatriateHistory.FamilyStatusId = expatriate.FamilyStatusId;
            expatriateHistory.DateOfPromotion = null;
            expatriateHistory.ExpatriateID = expatriate.ExpatriateID;
            return View(expatriateHistory);
        }

        //
        // POST: /ExpatriateHistory/Create

        [HttpPost]
        public ActionResult Create(ExpatriateHistory expatriatehistory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var expatriateToUpdate = db.Expatriates
                    .Include(i => i.ExpatriateHistorys)
                    .Include(i => i.Familys)
                    .Include(i => i.AgreementDetails)
                    .Where(i => i.ExpatriateID == expatriatehistory.ExpatriateID)
                    .Single();

                    expatriatehistory.CreateBy = HttpContext.User.Identity.Name;
                    expatriatehistory.CreateDateTime = DateTime.Now;
                    expatriatehistory.ModifiedBy = HttpContext.User.Identity.Name;
                    expatriatehistory.ModifiedDateTime = DateTime.Now;
                    expatriatehistory.CeilingValue = expatriateToUpdate.CeilingValue;
                    expatriatehistory.CostCode = expatriateToUpdate.CostCode;
                    expatriatehistory.DateOfPromotion = expatriateToUpdate.DateOfPromotion;
                    expatriatehistory.ExpatriateID = expatriateToUpdate.ExpatriateID;
                    expatriatehistory.CeilingValue = expatriateToUpdate.CeilingValue;

                    db.ExpatriateHistorys.Add(expatriatehistory);

                    if (!expatriateToUpdate.Title.Equals(expatriatehistory.Title))
                    {
                        expatriateToUpdate.Title = expatriatehistory.Title;
                    }

                    if (!expatriateToUpdate.JobGrade.Equals(expatriatehistory.JobGrade))
                    {
                        expatriateToUpdate.JobGrade = expatriatehistory.JobGrade;
                        expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                    }
                    expatriateToUpdate.CostCode = expatriatehistory.CostCode;
                    expatriateToUpdate.ModifiedDateTime = DateTime.Now;
                    expatriateToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                    UpdateModel(expatriateToUpdate, "", null, new string[] { "Familys", "AgreementDetails" });
                    db.Entry(expatriateToUpdate).State = EntityState.Modified;

                    expatriatehistory.CreateBy = HttpContext.User.Identity.Name;
                    expatriatehistory.CreateDateTime = DateTime.Now;
                    expatriatehistory.ModifiedBy = HttpContext.User.Identity.Name;
                    expatriatehistory.ModifiedDateTime = DateTime.Now;
                    db.ExpatriateHistorys.Add(expatriatehistory);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(expatriatehistory);
        }

        //
        // GET: /ExpatriateHistory/Edit/5

        public ActionResult Edit(int id)
        {
            ExpatriateHistory expatriatehistory = db.ExpatriateHistorys.Find(id);
            return View(expatriatehistory);
        }

        //
        // POST: /ExpatriateHistory/Edit/5

        [HttpPost]
        public ActionResult Edit(ExpatriateHistory expatriatehistory)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    expatriatehistory.ModifiedDateTime = DateTime.Now;
                    expatriatehistory.ModifiedBy = HttpContext.User.Identity.Name;
                    db.Entry(expatriatehistory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(expatriatehistory);
        }

        //
        // GET: /ExpatriateHistory/Delete/5

        public ActionResult Delete(int id)
        {
            ExpatriateHistory expatriatehistory = db.ExpatriateHistorys.Find(id);
            return View(expatriatehistory);
        }

        //
        // POST: /ExpatriateHistory/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ExpatriateHistory ExpatriateHistoryToDelete = new ExpatriateHistory() { ExpatriateHistoryID = id };
                db.Entry(ExpatriateHistoryToDelete).State = EntityState.Deleted;
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
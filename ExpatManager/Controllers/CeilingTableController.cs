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
    public class CeilingTableController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /CeilingTable/

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

            var ceilingTables = from s in db.CeilingTables
                                select s;

            if (!String.IsNullOrEmpty(search))
            {
                ceilingTables = ceilingTables.Where(s =>
                    SqlFunctions.StringConvert((Decimal)s.CeilingValue).Contains(search.ToUpper())
                    );
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm1 = String.IsNullOrEmpty(sortOrder) ? "Sort2 desc" : "";
            ViewBag.SortParm2 = sortOrder == "Sort2" ? "Sort2 desc" : "Sort2";
            ViewBag.SortParm3 = sortOrder == "Sort3" ? "Sort3 desc" : "Sort3";

            switch (sortOrder)
            {
                case "Sort1":
                    ceilingTables = ceilingTables.OrderBy(s => s.FamilyStatus);
                    break;
                case "Sort1 desc":
                    ceilingTables = ceilingTables.OrderByDescending(s => s.FamilyStatus);
                    break;
                case "Sort2":
                    ceilingTables = ceilingTables.OrderBy(s => s.CeilingValue);
                    break;
                case "Sort2 desc":
                    ceilingTables = ceilingTables.OrderByDescending(s => s.CeilingValue);
                    break;
                default:
                    ceilingTables = ceilingTables.OrderBy(s => s.JobGradeId);
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);

            int pageIndex = (pageNo ?? 1);

            return View(ceilingTables.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /CeilingTable/Details/5

        public ViewResult Details(int id)
        {
            CeilingTable ceilingtable = db.CeilingTables.Find(id);
            return View(ceilingtable);
        }

        //
        // GET: /CeilingTable/Create

        public ActionResult Create()
        {
            CeilingTable ceilingTable = new CeilingTable();
            ceilingTable.CeilingTableID = 1;
            return View(ceilingTable);
        }


        //
        // POST: /CeilingTable/Create

        [HttpPost]
        public ActionResult Create(CeilingTable ceilingtable, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ceilingtable.CreateBy = HttpContext.User.Identity.Name;
                    ceilingtable.CreateDateTime = DateTime.Now;
                    ceilingtable.ModifiedBy = HttpContext.User.Identity.Name;
                    ceilingtable.ModifiedDateTime = DateTime.Now;
                    ceilingtable.JobGradeId = int.Parse(formCollection.GetValue("JobGrade").AttemptedValue);
                    ceilingtable.FamilyStatusId = int.Parse(formCollection.GetValue("FamilyStatus").AttemptedValue);
                    ceilingtable.Status = true;       
                    db.CeilingTables.Add(ceilingtable);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(ceilingtable);
        }

        //
        // GET: /CeilingTable/Edit/5

        public ActionResult Edit(int id)
        {
            CeilingTable ceilingtable = db.CeilingTables.Find(id);
            ViewBag.EnumFamilyStatus = HtmlDropDownExtensions.ToSelectList(ceilingtable.FamilyStatus);
            ViewBag.EnumJobGrade = HtmlDropDownExtensions.ToSelectList(ceilingtable.JobGrade);
            ViewBag.FamilyStatus = ceilingtable.FamilyStatus;
            ViewBag.JobGrade = ceilingtable.JobGrade;
            ceilingtable.CreateDateTime = ceilingtable.CreateDateTime;
            ceilingtable.CreateBy = ceilingtable.CreateBy;
            ceilingtable.ModifiedBy = ceilingtable.ModifiedBy;
            ceilingtable.ModifiedDateTime = ceilingtable.ModifiedDateTime;

            //PopulateJobGradeDropDownList(ceilingtable.);
            //PopulateFamilyStatusDropDownList(agreementdetail.LandlordBankDetailID);

            return View(ceilingtable);
        }

        //
        // POST: /CeilingTable/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CeilingTable ceilingtable = db.CeilingTables.Find(id);
                    ceilingtable.JobGradeId = int.Parse(formCollection.GetValue("JobGrade").AttemptedValue);
                    ceilingtable.FamilyStatusId = int.Parse(formCollection.GetValue("FamilyStatus").AttemptedValue);
                    ceilingtable.CeilingValue = Decimal.Parse(formCollection.GetValue("CeilingValue").AttemptedValue);
                    ceilingtable.ModifiedBy = HttpContext.User.Identity.Name;
                    ceilingtable.ModifiedDateTime = DateTime.Now;
                    db.Entry(ceilingtable).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(formCollection);
        }

        //
        // GET: /CeilingTable/Delete/5

        public ActionResult Delete(int id)
        {
            CeilingTable ceilingtable = db.CeilingTables.Find(id);
            return View(ceilingtable);
        }

        //
        // POST: /CeilingTable/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                CeilingTable CeilingTableToDelete = new CeilingTable() { CeilingTableID = id };
                db.Entry(CeilingTableToDelete).State = EntityState.Deleted;
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

        public ViewResult Search(string JobGrade, string FamilyStatus)
        {
            var results = from m in db.CeilingTables
                          //where m.JobGradeId.StartsWith(JobGradeId)
                          select new { label = m.CeilingValue, id = m.CeilingTableID };
            results.Select(x => x.label).Take(10);

            return View(results);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public void PopulateJobGradeDropDownList(object selected = null)
        {
            var Query = from d in db.Expatriates
                        orderby d.FirstName, d.LastName
                        select d;
            ViewBag.ExpatriateID = new SelectList(Query, "ExpatriateID", "FullName", selected);
        }
    }
}
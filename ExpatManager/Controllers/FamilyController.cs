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
    public class FamilyController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /Family/

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

            var familys = from s in db.Familys
                          select s;

            if (!String.IsNullOrEmpty(search))
            {
                familys = familys.Where(s =>
                    s.Expatriates.FirstName.ToUpper().Contains(search.ToUpper())
                    || s.Expatriates.LastName.ToUpper().Contains(search.ToUpper())
                    );
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm1 = String.IsNullOrEmpty(sortOrder) ? "Sort1 desc" : "";
            ViewBag.SortParm2 = sortOrder == "Sort2" ? "Sort2 desc" : "Sort2";
            ViewBag.SortParm3 = sortOrder == "Sort3" ? "Sort3 desc" : "Sort3";
            ViewBag.SortParm4 = sortOrder == "Sort4" ? "Sort4 desc" : "Sort4";
            ViewBag.SortParm5 = sortOrder == "Sort5" ? "Sort5 desc" : "Sort5";
            ViewBag.SortParm6 = sortOrder == "Sort6" ? "Sort6 desc" : "Sort6";

            switch (sortOrder)
            {
                case "Sort1 desc":
                    familys = familys.OrderByDescending(s => (string.Concat(s.Expatriates.LastName, s.Expatriates.FirstName)));
                    break;
                case "Sort2":
                    familys = familys.OrderBy(s => s.FamilyType);
                    break;
                case "Sort2 desc":
                    familys = familys.OrderByDescending(s => s.FamilyType);
                    break;
                case "Sort3":
                    familys = familys.OrderBy(s => s.ArriveDate);
                    break;
                case "Sort3 desc":
                    familys = familys.OrderByDescending(s => s.ArriveDate);
                    break;
                case "Sort4":
                    familys = familys.OrderBy(s => s.LeaveDate);
                    break;
                case "Sort4 desc":
                    familys = familys.OrderByDescending(s => s.LeaveDate);
                    break;
                case "Sort5":
                    familys = familys.OrderBy(s => s.DateOfBirth);
                    break;
                case "Sort5 desc":
                    familys = familys.OrderByDescending(s => s.DateOfBirth);
                    break;
                case "Sort6":
                    familys = familys.OrderBy(s => s.Status);
                    break;
                case "Sort6 desc":
                    familys = familys.OrderByDescending(s => s.Status);
                    break;
                default:
                    familys = familys.OrderBy(s => (string.Concat(s.Expatriates.LastName, s.Expatriates.FirstName)));
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);
            int pageIndex = (pageNo ?? 1);

            return View(familys.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Family/Details/5

        public ViewResult Details(int id)
        {
            Family family = db.Familys.Find(id);
            Expatriate expatriate = db.Expatriates.Find(family.ExpatriateID);
            family.Expatriates = expatriate;

            return View(family);
        }

        //
        // GET: /Family/Create

        public ActionResult Create(int id)
        {
            Family family = new Family();
            family.CreateBy = HttpContext.User.Identity.Name;
            family.CreateDateTime = DateTime.Now;
            family.ModifiedBy = HttpContext.User.Identity.Name;
            family.ModifiedDateTime = DateTime.Now;
            family.Status = true;
            Expatriate expatriate = db.Expatriates.Find(id);
            family.Expatriates = expatriate;
            family.ExpatriateID = expatriate.ExpatriateID;
            if (expatriate.Familys.Count() > 0)
            {
                family.FamilyType = Enums.EnumFamilyType.CHILD;
            }
            else
            {
                family.FamilyType = Enums.EnumFamilyType.SPOUSE;
            }

            return View(family);
        }

        //
        // POST: /Family/Create

        [HttpPost]
        public ActionResult Create(Family family, int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    family.FamilyType = family.FamilyType;
                    db.Familys.Add(family);

                    var expatriateToUpdate = db.Expatriates
                        .Include(i => i.ExpatriateHistorys)
                        .Include(i => i.Familys)
                        .Include(i => i.AgreementDetails)
                        .Where(i => i.ExpatriateID == family.ExpatriateID)
                        .Single();

                    expatriateToUpdate.ModifiedDateTime = DateTime.Now;
                    expatriateToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                    expatriateToUpdate.CreateDateTime = DateTime.Now;
                    expatriateToUpdate.CreateBy = HttpContext.User.Identity.Name;

                    switch (family.FamilyType)
                    {
                        case Enums.EnumFamilyType.SPOUSE:
                            if (expatriateToUpdate.FamilyStatus.Equals(Enums.EnumFamilyStatus.SINGLE))
                            {
                                //Only update to spouse if the current status is single.

                                expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSE;
                                expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                            }
                            break;
                        case Enums.EnumFamilyType.CHILD:
                            switch (expatriateToUpdate.FamilyStatus)
                            {
                                case Enums.EnumFamilyStatus.SPOUSE:
                                    {
                                        //As child will only come if spouse is here, update to spouse and child.
                                        expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILD;
                                        expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                    }
                                    break;
                                case Enums.EnumFamilyStatus.SPOUSEANDCHILD:
                                    {
                                        //As child will only come if spouse is here, update to spouse and child.
                                        expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILDx2;
                                        expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                    }
                                    break;
                                case Enums.EnumFamilyStatus.SPOUSEANDCHILDx2:
                                    {
                                        //As child will only come if spouse is here, update to spouse and child.
                                        expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILDx3;
                                        expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                    }
                                    break;
                                case Enums.EnumFamilyStatus.SPOUSEANDCHILDx3:
                                    {
                                        //As child will only come if spouse is here, update to spouse and child.
                                        expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILDx4;
                                        expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                    }
                                    break;
                                case Enums.EnumFamilyStatus.SPOUSEANDCHILDx4:
                                    {
                                        //As child will only come if spouse is here, update to spouse and child.
                                        expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILDx5;
                                        expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                    }
                                    break;
                            }
                            break;
                    }

                    ExpatriateHistory expatriateHistory = dbHelper.CreateExpatriateHistory(expatriateToUpdate);
                    db.ExpatriateHistorys.Add(expatriateHistory);

                    if (expatriateToUpdate.AgreementDetails != null && expatriateToUpdate.AgreementDetails.Count > 0)
                    {
                        expatriateToUpdate.AgreementDetails.LastOrDefault().CeilingBreach = expatriateToUpdate.CeilingValue - expatriateToUpdate.AgreementDetails.LastOrDefault().MonthlyPayment;
                    }
                    UpdateModel(expatriateToUpdate, "", new string[] { "Expatriates", "Familys", "Agreements" }, new string[] { "" });
                    db.Entry(expatriateToUpdate).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Details", "Expatriate",
                                new System.Web.Routing.RouteValueDictionary {
                                { "id", Id },
                                { "saveChangesError", true }});
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(family);
        }

        //
        // GET: /Family/Edit/5

        public ActionResult Edit(int id)
        {
            Family family = db.Familys.Find(id);
            Expatriate expatriate = db.Expatriates.Find(family.ExpatriateID);
            family.Expatriates = expatriate;
            return View(family);
        }

        //
        // POST: /Family/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var familyToUpdate = db.Familys
                     .Include(i => i.Expatriates)
                     .Where(i => i.FamilyID == id)
                     .Single();

                    familyToUpdate.ModifiedDateTime = DateTime.Now;
                    familyToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                    UpdateModel(familyToUpdate, "", null, new string[] { "Expatriates" });
                    int expatriateId = familyToUpdate.ExpatriateID;
                    db.Entry(familyToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Expatriate", new System.Web.Routing.RouteValueDictionary {
                        { "id", familyToUpdate.ExpatriateID }});
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            Family family = db.Familys.Find(id);
            Expatriate expatriate = db.Expatriates.Find(family.ExpatriateID);
            family.Expatriates = expatriate;
            return View(family);
        }

        //
        // GET: /Family/Delete/5

        public ActionResult Delete(int id)
        {
            Family family = db.Familys.Find(id);
            Expatriate expatriate = db.Expatriates.Find(family.ExpatriateID);
            family.Expatriates = expatriate;
            return View(family);
        }

        //
        // POST: /Family/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Family FamilyToDelete = db.Familys.Find(id);

                var expatriateToUpdate = db.Expatriates
                      .Include(i => i.ExpatriateHistorys)
                      .Include(i => i.Familys)
                      .Include(i => i.AgreementDetails)
                      .Where(i => i.ExpatriateID == FamilyToDelete.ExpatriateID)
                      .Single();

                expatriateToUpdate.ModifiedDateTime = DateTime.Now;
                expatriateToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                expatriateToUpdate.CreateDateTime = DateTime.Now;
                expatriateToUpdate.CreateBy = HttpContext.User.Identity.Name;

                switch (FamilyToDelete.FamilyType.ToString().ToUpper())
                {
                    case "SPOUSE":
                        //update to single if the current status is spouse.
                        expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SINGLE;
                        expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                        //db.Entry(FamilyToDelete).State = EntityState.Deleted;
                        if (expatriateToUpdate.AgreementDetails.Count > 0)
                        {
                            expatriateToUpdate.AgreementDetails.LastOrDefault().CeilingBreach = expatriateToUpdate.CeilingValue - expatriateToUpdate.AgreementDetails.LastOrDefault().MonthlyPayment;
                        }
                        UpdateModel(expatriateToUpdate, "", null, new string[] { "Familys" });
                        db.Entry(expatriateToUpdate).State = EntityState.Modified;
                        //UpdateModel(expatriateToUpdate, "", null, new string[] { "Familys", "AgreementDetails" });
                        db.Entry(FamilyToDelete).State = EntityState.Deleted;
                        db.SaveChanges();

                        foreach (var f in expatriateToUpdate.Familys)
                        {
                            if (!f.FamilyID.Equals(FamilyToDelete.FamilyID))
                            {
                                ExpatriateManagementContext context = new ExpatriateManagementContext();
                                Family ToDelete = new Family() { FamilyID = f.FamilyID };
                                context.Entry(ToDelete).State = EntityState.Deleted;
                                context.SaveChanges();
                            }
                        }
                        break;
                    case "CHILD":

                        switch (expatriateToUpdate.FamilyStatus)
                        {
                            case Enums.EnumFamilyStatus.SPOUSE:
                                {
                                    //As child will only come if spouse is here, update to spouse and child.
                                    expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SINGLE;
                                    expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                }
                                break;
                            case Enums.EnumFamilyStatus.SPOUSEANDCHILD:
                                {
                                    //As child will only come if spouse is here, update to spouse and child.
                                    expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSE;
                                    expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                }
                                break;
                            case Enums.EnumFamilyStatus.SPOUSEANDCHILDx2:
                                {
                                    //As child will only come if spouse is here, update to spouse and child.
                                    expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILD;
                                    expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                }
                                break;
                            case Enums.EnumFamilyStatus.SPOUSEANDCHILDx3:
                                {
                                    //As child will only come if spouse is here, update to spouse and child.
                                    expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILDx2;
                                    expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                }
                                break;
                            case Enums.EnumFamilyStatus.SPOUSEANDCHILDx4:
                                {
                                    //As child will only come if spouse is here, update to spouse and child.
                                    expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILDx3;
                                    expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                }
                                break;
                            case Enums.EnumFamilyStatus.SPOUSEANDCHILDx5:
                                {
                                    //As child will only come if spouse is here, update to spouse and child.
                                    expatriateToUpdate.FamilyStatus = Enums.EnumFamilyStatus.SPOUSEANDCHILDx4;
                                    expatriateToUpdate.CeilingValue = ExpatCalculation.CeilingValue(expatriateToUpdate.JobGradeId, expatriateToUpdate.FamilyStatusId);
                                }
                                break;
                        }
                
                        if (expatriateToUpdate.AgreementDetails.Count > 0)
                        {
                            expatriateToUpdate.AgreementDetails.LastOrDefault().CeilingBreach = expatriateToUpdate.CeilingValue - expatriateToUpdate.AgreementDetails.LastOrDefault().MonthlyPayment;
                        }

                        db.Entry(expatriateToUpdate).State = EntityState.Modified;
                        db.Entry(FamilyToDelete).State = EntityState.Deleted;
                        db.SaveChanges();
                        break;
                }

                ExpatriateHistory expatriateHistory = dbHelper.CreateExpatriateHistory(expatriateToUpdate);
                db.ExpatriateHistorys.Add(expatriateHistory);
                db.SaveChanges();

                return RedirectToAction("Details", "Expatriate",
                            new System.Web.Routing.RouteValueDictionary {
                                { "id", FamilyToDelete.ExpatriateID },
                                { "saveChangesError", true }});

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
    }
}
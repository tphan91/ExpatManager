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
    public class AuditController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /AgentDetail/

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

            var agentDetails = from s in db.AgentDetails
                               select s;

            if (!String.IsNullOrEmpty(search))
            {
                agentDetails = agentDetails.Where(s =>
                    s.AgentName.Contains(search.ToUpper())
                    || s.BranchOffice.ToUpper().Contains(search.ToUpper())
                    || s.Email.ToUpper().Contains(search.ToUpper())
                    || s.TelNo.ToUpper().Contains(search.ToUpper())
                    || s.ContactName.ToUpper().Contains(search.ToUpper())
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

            switch (sortOrder)
            {
                case "Sort1 desc":
                    agentDetails = agentDetails.OrderByDescending(s => s.AgentName);
                    break;
                case "Sort2":
                    agentDetails = agentDetails.OrderBy(s => s.BranchOffice);
                    break;
                case "Sort2 desc":
                    agentDetails = agentDetails.OrderByDescending(s => s.BranchOffice);
                    break;
                case "Sort3":
                    agentDetails = agentDetails.OrderBy(s => s.Address1);
                    break;
                case "Sort3 desc":
                    agentDetails = agentDetails.OrderByDescending(s => s.Address1);
                    break;
                case "Sort4":
                    agentDetails = agentDetails.OrderBy(s => s.Email);
                    break;
                case "Sort4 desc":
                    agentDetails = agentDetails.OrderByDescending(s => s.Email);
                    break;
                case "Sort5":
                    agentDetails = agentDetails.OrderBy(s => s.ContactName);
                    break;
                case "Sort5 desc":
                    agentDetails = agentDetails.OrderByDescending(s => s.ContactName);
                    break;
                case "Sort6":
                    agentDetails = agentDetails.OrderBy(s => s.TelNo);
                    break;
                case "Sort6 desc":
                    agentDetails = agentDetails.OrderByDescending(s => s.TelNo);
                    break;
                default:
                    agentDetails = agentDetails.OrderBy(s => s.AgentName);
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);

            int pageIndex = (pageNo ?? 1);

            return View(agentDetails.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /AgentDetail/Details/5

        public ViewResult Details(int id)
        {
            AgentDetail agentdetail = db.AgentDetails.Find(id);
            return View(agentdetail);
        }

        //
        // GET: /AgentDetail/Create

        public ActionResult Create()
        {
            AgentDetail agentdetail = new AgentDetail();
            agentdetail.CreateBy = HttpContext.User.Identity.Name;
            agentdetail.CreateDateTime = DateTime.Now;
            agentdetail.ModifiedBy = HttpContext.User.Identity.Name;
            agentdetail.ModifiedDateTime = DateTime.Now;
            agentdetail.Status = true;
            return View(agentdetail);
        }

        //
        // POST: /AgentDetail/Create

        [HttpPost]
        public ActionResult Create(AgentDetail agentdetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    agentdetail.CreateBy = HttpContext.User.Identity.Name;
                    agentdetail.CreateDateTime = DateTime.Now;
                    agentdetail.ModifiedBy = HttpContext.User.Identity.Name;
                    agentdetail.ModifiedDateTime = DateTime.Now;
                    agentdetail.Status = true;
                    db.AgentDetails.Add(agentdetail);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(agentdetail);
        }

        //
        // GET: /AgentDetail/Edit/5

        public ActionResult Edit(int id)
        {
            AgentDetail agentdetail = db.AgentDetails.Find(id);
            return View(agentdetail);
        }

        //
        // POST: /AgentDetail/Edit/5

        [HttpPost]
        public ActionResult Edit(AgentDetail agentdetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    agentdetail.ModifiedDateTime = DateTime.Now;
                    agentdetail.ModifiedBy = HttpContext.User.Identity.Name;
                    db.Entry(agentdetail).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("Index", "AgentDetail");
        }

        //
        // GET: /AgentDetail/Delete/5

        public ActionResult Delete(int id)
        {
            AgentDetail agentdetail = db.AgentDetails.Find(id);

            return View(agentdetail);
        }

        //
        // POST: /AgentDetail/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            AgentDetail AgentDetailToDelete = new AgentDetail() { AgentDetailID = id };

            try
            {
                db.Entry(AgentDetailToDelete).State = EntityState.Deleted;
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

            return RedirectToAction("Index", "AgentDetail");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
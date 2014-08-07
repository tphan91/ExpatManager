using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using ExpatManager.Models;
using ExpatManager.DAL;
using PagedList;
using System.Web.Configuration;

namespace ExpatManager.Controllers
{
    public class LandlordBankDetailController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /LandlordBankDetails/
        public ViewResult Index(string sortOrder, string currentFilter, string search, int? pageNo)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.AccountNameSortParm = String.IsNullOrEmpty(sortOrder) ? "AccountName desc" : "";
            ViewBag.BankNameSortParm = sortOrder == "BankName" ? "BankName desc" : "BankName";
            ViewBag.BankAccountNoSortParm = sortOrder == "BankAccountNo" ? "BankAccountNo desc" : "BankAccountNo";
            ViewBag.SortCodeSortParm = sortOrder == "SortCode" ? "SortCode desc" : "SortCode";
            ViewBag.ReferenceSortParm = sortOrder == "Reference" ? "Reference desc" : "Reference";

            if (Request.HttpMethod == "GET")
            {
                search = currentFilter;
            }
            else
            {
                pageNo = 1;
            }

            ViewBag.CurrentFilter = search;

            var landlordBankDetails = from s in db.LandlordBankDetails
                                      select s;

            if (!String.IsNullOrEmpty(search))
            {
                landlordBankDetails = landlordBankDetails.Where(s => s.AccountName.ToUpper().Contains(search.ToUpper())
                    || s.BankName.ToUpper().Contains(search.ToUpper())
                    || s.BankAccountNo.Contains(search.ToUpper())
                    || s.SortCode.Contains(search.ToUpper())
                    );
            }

            switch (sortOrder)
            {
                case "AccountName desc":
                    landlordBankDetails = landlordBankDetails.OrderByDescending(s => s.AccountName);
                    break;
                case "BankName":
                    landlordBankDetails = landlordBankDetails.OrderBy(s => s.BankName);
                    break;
                case "BankName desc":
                    landlordBankDetails = landlordBankDetails.OrderByDescending(s => s.BankName);
                    break;
                case "BankAccountNo":
                    landlordBankDetails = landlordBankDetails.OrderBy(s => s.BankAccountNo);
                    break;
                case "BankAccountNo desc":
                    landlordBankDetails = landlordBankDetails.OrderByDescending(s => s.BankAccountNo);
                    break;
                case "SortCode":
                    landlordBankDetails = landlordBankDetails.OrderBy(s => s.SortCode);
                    break;
                case "SortCode desc":
                    landlordBankDetails = landlordBankDetails.OrderByDescending(s => s.SortCode);
                    break;
                default:
                    landlordBankDetails = landlordBankDetails.OrderBy(s => s.AccountName);
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);
            int pageIndex = (pageNo ?? 1);
            
            return View(landlordBankDetails.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /LandlordBankDetails/Details/5

        public ViewResult Details(int id)
        {
            LandlordBankDetail landlordbankdetail = db.LandlordBankDetails.Find(id);
            return View(landlordbankdetail);
        }

        //
        // GET: /LandlordBankDetails/Create

        public ActionResult Create(int id)
        {
            LandlordBankDetail landlordbankdetail = new LandlordBankDetail();
            landlordbankdetail.AgentDetailID = id;
            landlordbankdetail.CreateBy = HttpContext.User.Identity.Name;
            landlordbankdetail.CreateDateTime = DateTime.Now;
            landlordbankdetail.ModifiedBy = HttpContext.User.Identity.Name;
            landlordbankdetail.ModifiedDateTime = DateTime.Now;
            landlordbankdetail.Status = true;
            AgentDetail agentdetails = db.AgentDetails.Find(id);
            landlordbankdetail.AgentDetails = agentdetails;
            landlordbankdetail.AgentDetails.AgentDetailID = agentdetails.AgentDetailID;

            return View(landlordbankdetail);
        }

        //
        // POST: /LandlordBankDetails/Create

        [HttpPost]
        public ActionResult Create(LandlordBankDetail landlordbankdetail, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //landlordbankdetail.AgreementDetailID = id;
                    landlordbankdetail.CreateBy = HttpContext.User.Identity.Name;
                    landlordbankdetail.CreateDateTime = DateTime.Now;
                    landlordbankdetail.ModifiedBy = HttpContext.User.Identity.Name;
                    landlordbankdetail.ModifiedDateTime = DateTime.Now;
                    landlordbankdetail.AgentDetailID = id;
                    AgentDetail agentdetail = db.AgentDetails.Find(id);
                    landlordbankdetail.AgentDetails = agentdetail;
                    db.LandlordBankDetails.Add(landlordbankdetail);

                    //agentdetail.LandlordBankDetails = landlordbankdetail.LandlordBankDetailID;
                    //agentdetail.ShowBankCreateLink = false;
                    db.Entry(agentdetail).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details",
                                "AgentDetail",
                                new System.Web.Routing.RouteValueDictionary {
                                        { "id", id },
                                        { "saveChangesError", true }});
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(landlordbankdetail);
        }

        //
        // GET: /LandlordBankDetails/Edit/5

        public ActionResult Edit(int id)
        {
            LandlordBankDetail landlordbankdetail = db.LandlordBankDetails.Find(id);
            return View(landlordbankdetail);
        }

        //
        // POST: /LandlordBankDetails/Edit/5

        [HttpPost]
        public ActionResult Edit(LandlordBankDetail landlordbankdetail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var landlordbankdetailToUpdate = db.LandlordBankDetails
                .Include(i => i.AgentDetails)
                .Where(i => i.LandlordBankDetailID == landlordbankdetail.LandlordBankDetailID)
                .Single();

                    landlordbankdetailToUpdate.ModifiedDateTime = DateTime.Now;
                    landlordbankdetailToUpdate.ModifiedBy = HttpContext.User.Identity.Name;
                    UpdateModel(landlordbankdetailToUpdate, "", null, new string[] { "AgreementDetails" });

                    db.SaveChanges();

                    return RedirectToAction("Details", "AgentDetail",
                    new System.Web.Routing.RouteValueDictionary {
                                        { "id", landlordbankdetailToUpdate.AgentDetailID },
                                        { "saveChangesError", true }});
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(landlordbankdetail);
        }

        //
        // GET: /LandlordBankDetails/Delete/5

        public ActionResult Delete(int id)
        {
            LandlordBankDetail landlordbankdetail = db.LandlordBankDetails.Find(id);
            return View(landlordbankdetail);
        }

        //
        // POST: /LandlordBankDetails/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                LandlordBankDetail LandlordBankDetailToDelete = new LandlordBankDetail() { LandlordBankDetailID = id };
                db.Entry(LandlordBankDetailToDelete).State = EntityState.Deleted;
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
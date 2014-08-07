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
    public class ParkingController : Controller
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

            var agreementDetails = from s in db.AgreementDetails
                          select s;

            if (!String.IsNullOrEmpty(search))
            {
                agreementDetails = agreementDetails.Where(s =>
                    s.Expatriates.FirstName.ToUpper().Contains(search.ToUpper())
                    || s.Expatriates.LastName.ToUpper().Contains(search.ToUpper())
                    );
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm1 = String.IsNullOrEmpty(sortOrder) ? "Sort1 desc" : "";
            ViewBag.SortParm2 = sortOrder == "Sort2" ? "Sort2 desc" : "Sort2";
            ViewBag.SortParm3 = sortOrder == "Sort3" ? "Sort3 desc" : "Sort3";
            ViewBag.SortParm4 = sortOrder == "Sort4" ? "Sort4 desc" : "Sort4";


            switch (sortOrder)
            {
                case "Sort1 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => (string.Concat(s.Expatriates.LastName, s.Expatriates.FirstName)));
                    break;
                case "Sort2":
                    agreementDetails = agreementDetails.OrderBy(s => s.Parkings.ParkingAmount);
                    break;
                case "Sort2 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.Parkings.ParkingAmount);
                    break;
                case "Sort3":
                    agreementDetails = agreementDetails.OrderBy(s => s.Parkings.ParkingPaidBy);
                    break;
                case "Sort3 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.Parkings.ParkingPaidBy);
                    break;
                case "Sort4":
                    agreementDetails = agreementDetails.OrderBy(s => s.Parkings.ParkingComment);
                    break;
                case "Sort4 desc":
                    agreementDetails = agreementDetails.OrderByDescending(s => s.Parkings.ParkingComment);
                    break;
                default:
                    agreementDetails = agreementDetails.OrderBy(s => (string.Concat(s.Expatriates.LastName, s.Expatriates.FirstName)));
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);
            int pageIndex = (pageNo ?? 1);

            return View(agreementDetails.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /Family/Details/5

        public ViewResult Details(int id)
        {
            AgreementDetail agreementDetails = db.AgreementDetails.Find(id);

            return View(agreementDetails);
        }

        //
        // GET: /Family/Create

        public ActionResult Create(int id)
        {
            Parking parking = new Parking();
            parking.CreateBy = HttpContext.User.Identity.Name;
            parking.CreateDateTime = DateTime.Now;
            parking.ModifiedBy = HttpContext.User.Identity.Name;
            parking.ModifiedDateTime = DateTime.Now;
            parking.Status = true;

            return View(parking);
        }

        //
        // POST: /Family/Create

        [HttpPost]
        public ActionResult Create(Parking parking, FormCollection formCollection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    parking.CreateBy = HttpContext.User.Identity.Name;
                    parking.CreateDateTime = DateTime.Now;
                    parking.ModifiedBy = HttpContext.User.Identity.Name;
                    parking.ModifiedDateTime = DateTime.Now;
                    parking.ParkingAmount = int.Parse(formCollection.GetValue("ParkingAmount").AttemptedValue);
                    parking.ParkingComment = formCollection.GetValue("ParkingComment").AttemptedValue;
                    parking.ParkingPaidBy = formCollection.GetValue("ParkingPaidBy").AttemptedValue;
                    parking.Status = true;
                    db.Parkings.Add(parking);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(parking);
        }

        //
        // GET: /Family/Edit/5

        public ActionResult Edit(int id)
        {
            Parking parking = db.Parkings.Find(id);
            return View(parking);
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
                    var parkingToUpdate = db.AgreementDetails
                     .Include(i => i.Parkings)
                     .Where(i => i.AgreementDetailID == id)
                     .Single();

                    parkingToUpdate.Parkings.ModifiedDateTime = DateTime.Now;
                    parkingToUpdate.Parkings.ModifiedBy = HttpContext.User.Identity.Name;
                    UpdateModel(parkingToUpdate, "", null, new string[] { "Parkings" });
                    int expatriateId = parkingToUpdate.ExpatriateID;
                    db.Entry(parkingToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Details", "Expatriate", new System.Web.Routing.RouteValueDictionary {
                        { "id", parkingToUpdate.ExpatriateID }});
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
            AgreementDetail agreementDetail = db.AgreementDetails.Find(id);

            return View(agreementDetail);
        }

        //
        // POST: /Family/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                AgreementDetail agreementDetail = db.AgreementDetails.Find(id);

                Parking ParkingToDelete = new Parking() { ParkingID = int.Parse(agreementDetail.ParkingID.ToString()) };
                db.Entry(ParkingToDelete).State = EntityState.Deleted;
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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using ExpatManager.Models;

namespace ExpatManager.Controllers
{
    public class PersonJobController : Controller
    {
        //private HRContext db = new HRContext();
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /PersonJob/

        public ViewResult Index()
        {
            return View(db.PersonJobs.ToList());
        }

        public ViewResult MissingPhotoList()
        {
            DirectoryInfo photoDirectory = null;
            FileInfo[] files = null;
            try
            {
                string photoPath = @"Y:\GB\images\StaffPic";
                photoDirectory = new DirectoryInfo(photoPath);
                files = photoDirectory.GetFiles();
            }
            catch (DirectoryNotFoundException exp)
            {
                throw new ArgumentException("Could not open the ftp directory", exp);
            }
            catch (IOException exp)
            {
                throw new ArgumentException("Failed to access directory", exp);
            }
                       // var results = from m in db.PersonJobs
                         // where m.PersonJobID.StartsWith(term) 
            //.Where(f => f.FullName.Contains())
            
            var photofiles = files.Where(f => f.Extension == ".jpg" || f.Extension == ".JPG")
                .OrderBy(f => f.Name)
                .Select(f => f.Name.PadRight(6))
                .ToArray();
            
            return View(photofiles);
        }

        //
        // GET: /PersonJob/Details/5

        public ViewResult Details(int id)
        {
            vPersonJob personjob = db.PersonJobs.Find(id);
            return View(personjob);
        }

        //
        // GET: /PersonJob/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PersonJob/Create

        [HttpPost]
        public ActionResult Create(vPersonJob personjob)
        {
            if (ModelState.IsValid)
            {
                db.PersonJobs.Add(personjob);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personjob);
        }

        //
        // GET: /PersonJob/Edit/5

        public ActionResult Edit(int id)
        {
            vPersonJob personjob = db.PersonJobs.Find(id);
            return View(personjob);
        }

        //
        // POST: /PersonJob/Edit/5

        [HttpPost]
        public ActionResult Edit(vPersonJob personjob)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personjob).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personjob);
        }

        //
        // GET: /PersonJob/Delete/5

        public ActionResult Delete(int id)
        {
            vPersonJob personjob = db.PersonJobs.Find(id);
            return View(personjob);
        }

        //
        // POST: /PersonJob/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            vPersonJob personjob = db.PersonJobs.Find(id);
            db.PersonJobs.Remove(personjob);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /*
        [HttpPost]
        public ActionResult FindNames(string term)
        {
            var results = from m in db.PersonJobs
                          where m.FullName.StartsWith(term)
                          select new { label = m.FullName, id = m.PersonJobID };
            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);



        }

        [HttpPost]
        public JsonResult Search(string term)
        {
            var results = from m in db.PersonJobs
                          where m.FullName.StartsWith(term)
                          select new { label = m.FullName, id = m.PersonJobID };

            return Json(results, JsonRequestBehavior.AllowGet);
        }
         */

        public ActionResult Search(string term)
        {
            var results = from m in db.PersonJobs
                          where m.FirstName.StartsWith(term) ||
                                m.LastName.StartsWith(term)
                          select new { label = m.FirstName, id = m.PersonJobID };
            return View();
        }

        public ActionResult AutoComplete(string key)
        {
            if (key == null)
            {
                key = string.Empty;
            }

            var results = from m in db.PersonJobs
                          where m.FirstName.StartsWith(key) ||
                                m.LastName.StartsWith(key)
                          select new { label = m.FirstName, id = m.PersonJobID };
            results.Select(x => x.label).Take(10);
            return View(results);
        }

        public ActionResult AutoCompleteData(string term)
        {
            var results = from m in db.PersonJobs
                          where m.FirstName.StartsWith(term) ||
                                m.LastName.StartsWith(term)
                          select new { label = string.Concat(m.FirstName, " ", m.LastName, " / ", m.PersonJobID), id = m.PersonJobID };
            results.Select(x => x.label).Take(10);

            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
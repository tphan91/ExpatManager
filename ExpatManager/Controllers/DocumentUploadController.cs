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
    public class DocumentUploadController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /DocumentUpload/

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

            var documentUploads = from s in db.DocumentUploads
                                   select s;

            if (!String.IsNullOrEmpty(search))
            {
                documentUploads = documentUploads.Where(s =>
                    SqlFunctions.StringConvert((double)s.DocumentUploadID).Contains(search.ToUpper())
                    || s.FileName.ToUpper().Contains(search.ToUpper())
                    || s.FileType.ToUpper().Contains(search.ToUpper())
                    || SqlFunctions.StringConvert((double)s.FileSize).Contains(search.ToUpper())
                    || s.DocumentDescription.ToUpper().Contains(search.ToUpper())
                    );
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortParm1 = String.IsNullOrEmpty(sortOrder) ? "Sort1 desc" : "";
            ViewBag.SortParm2 = sortOrder == "Sort2" ? "Sort2 desc" : "Sort2";
            ViewBag.SortParm3 = sortOrder == "Sort3" ? "Sort3 desc" : "Sort3";
            ViewBag.SortParm4 = sortOrder == "Sort4" ? "Sort4 desc" : "Sort4";
            ViewBag.SortParm5 = sortOrder == "Sort5" ? "Sort4 desc" : "Sort5";

            switch (sortOrder)
            {
                case "Sort1 desc":
                    documentUploads = documentUploads.OrderByDescending(s => s.FileName);
                    break;
                case "Sort2":
                    documentUploads = documentUploads.OrderBy(s => s.FileType);
                    break;
                case "Sort2 desc":
                    documentUploads = documentUploads.OrderByDescending(s => s.FileType);
                    break;
                case "Sort3":
                    documentUploads = documentUploads.OrderBy(s => s.FileSize);
                    break;
                case "Sort3 desc":
                    documentUploads = documentUploads.OrderByDescending(s => s.FileSize);
                    break;
                case "Sort4":
                    documentUploads = documentUploads.OrderBy(s => s.DocumentDescription);
                    break;
                case "Sort4 desc":
                    documentUploads = documentUploads.OrderByDescending(s => s.DocumentDescription);
                    break;
                case "Sort5":
                    documentUploads = documentUploads.OrderBy(s => s.ModelName);
                    break;
                case "Sort5 desc":
                    documentUploads = documentUploads.OrderByDescending(s => s.ModelName);
                    break;
                default:
                    documentUploads = documentUploads.OrderBy(s => s.FileName);
                    break;
            }

            int pageSize = Helper.StringExtensions.TryToParseInt(WebConfigurationManager.AppSettings["PageListPageSize"]);
            int pageIndex = (pageNo ?? 1);

            return View(documentUploads.ToPagedList(pageIndex, pageSize));
        }

        //
        // GET: /DocumentUpload/Details/5

        public ViewResult Details(int id)
        {
            DocumentUpload documentupload = db.DocumentUploads.Find(id);
            return View(documentupload);
        }

        //
        // GET: /DocumentUpload/Create

        public ActionResult Create()
        {
            DocumentUpload documentupload = new DocumentUpload();
            documentupload.CreateBy = HttpContext.User.Identity.Name;
            documentupload.CreateDateTime = DateTime.Now;
            documentupload.Status = true;
            return View();
        }

        //
        // POST: /DocumentUpload/Create

        [HttpPost]
        public ActionResult Create(DocumentUpload documentupload, HttpPostedFileBase fileUpload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var fileName = this.Server.MapPath("~/DocumentUpload/" + System.IO.Path.GetFileName(fileUpload.FileName) + Guid.NewGuid().ToString());
                    fileUpload.SaveAs(fileName);

                    documentupload.FileName = System.IO.Path.GetFileName(fileUpload.FileName) + Guid.NewGuid().ToString();
                    documentupload.FileSize = fileUpload.ContentLength;
                    documentupload.FileType = fileUpload.ContentType;
                    documentupload.CreateBy = HttpContext.User.Identity.Name;
                    documentupload.CreateDateTime = DateTime.Now;

                    db.DocumentUploads.Add(documentupload);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(documentupload);
        }

        //
        // GET: /DocumentUpload/Edit/5

        public ActionResult Edit(int id)
        {
            DocumentUpload documentupload = db.DocumentUploads.Find(id);
            return View(documentupload);
        }

        //
        // POST: /DocumentUpload/Edit/5

        [HttpPost]
        public ActionResult Edit(DocumentUpload documentupload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(documentupload).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(documentupload);
        }

        //
        // GET: /DocumentUpload/Delete/5

        public ActionResult Delete(int id)
        {
            DocumentUpload documentupload = db.DocumentUploads.Find(id);
            return View(documentupload);
        }

        //
        // POST: /DocumentUpload/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                DocumentUpload DocumentUploadToDelete = new DocumentUpload() { DocumentUploadID = id };
                db.Entry(DocumentUploadToDelete).State = EntityState.Deleted;
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
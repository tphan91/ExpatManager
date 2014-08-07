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
    public class ExpatriateDocumentUploadController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /AgreementDetailDocumentUpload/

        public ViewResult Index()
        {
            return View(db.ExpatriateDocumentUploads.ToList());
        }

        //
        // GET: /AgreementDetailDocumentUpload/Details/5

        public ViewResult Details(int id)
        {
            ExpatriateDocumentUpload expatriatedocumentupload = db.ExpatriateDocumentUploads.Find(id);
            return View(expatriatedocumentupload);
        }

        //
        // GET: /AgreementDetailDocumentUpload/Create

        public ActionResult Create(int Id)
        {
            ExpatriateDocumentUpload documentUpload = new ExpatriateDocumentUpload();
            documentUpload.ExpatriateID = Id;
            documentUpload.CreateBy = HttpContext.User.Identity.Name;
            documentUpload.CreateDateTime = DateTime.Now;
            documentUpload.ModifiedBy = HttpContext.User.Identity.Name;
            documentUpload.ModifiedDateTime = DateTime.Now;
            documentUpload.FileName = "Dummy";
            documentUpload.Status = true;
            Expatriate Expatriates = db.Expatriates.Find(Id);
            documentUpload.Expatriates = Expatriates;

            return View(documentUpload);
        }

        //
        // POST: /AgreementDetailDocumentUpload/Create

        [HttpPost]
        public ActionResult Create(ExpatriateDocumentUpload expatriateDocumentUpload, int Id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExpatriateDocumentUpload documentUpload = new ExpatriateDocumentUpload();
                    documentUpload.ExpatriateID = Id;
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
                            db.ExpatriateDocumentUploads.Add(documentUpload);
                            db.SaveChanges();
                            uploadedCount++;
                        }
                    }
                    return RedirectToAction("Details", "Expatriate",
                            new System.Web.Routing.RouteValueDictionary {
                                { "id", Id },
                                { "saveChangesError", true }});
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(expatriateDocumentUpload);
        }

        //
        // GET: /AgreementDetailDocumentUpload/Edit/5

        public ActionResult Edit(int id)
        {
            ExpatriateDocumentUpload expatriatedocumentupload = db.ExpatriateDocumentUploads.Find(id);
            return View(expatriatedocumentupload);
        }

        //
        // POST: /AgreementDetailDocumentUpload/Edit/5

        [HttpPost]
        public ActionResult Edit(ExpatriateDocumentUpload expatriateDocumentUpload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expatriateDocumentUpload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expatriateDocumentUpload);
        }

        //
        // GET: /AgreementDetailDocumentUpload/Delete/5

        public ActionResult Delete(int id)
        {
            ExpatriateDocumentUpload expatriateDocumentUpload = db.ExpatriateDocumentUploads.Find(id);
            return View(expatriateDocumentUpload);
        }

        //
        // POST: /AgreementDetailDocumentUpload/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int Id)
        {
            ExpatriateDocumentUpload expatriateDocumentUpload = db.ExpatriateDocumentUploads.Find(Id);
            int ExpatriateID = expatriateDocumentUpload.ExpatriateID;
            db.Entry(expatriateDocumentUpload).State = EntityState.Detached;
            try
            {
                ExpatriateDocumentUpload expatriateDocumentUploadToDelete = new ExpatriateDocumentUpload() { ExpatriateDocumentUploadID = Id };
                db.Entry(expatriateDocumentUploadToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                return RedirectToAction("Delete",
                    new System.Web.Routing.RouteValueDictionary {
                        { "id", Id },
                        { "saveChangesError", true }
                    });
            }
            return RedirectToAction("Details", "Expatriate",
                        new System.Web.Routing.RouteValueDictionary {
                                { "id", ExpatriateID },
                                { "saveChangesError", true }});
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
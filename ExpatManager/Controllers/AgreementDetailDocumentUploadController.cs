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
    public class AgreementDetailDocumentUploadController : Controller
    {
        private ExpatriateManagementContext db = new ExpatriateManagementContext();

        //
        // GET: /AgreementDetailDocumentUpload/

        public ViewResult Index()
        {
            return View(db.AgreementDetailDocumentUploads.ToList());
        }

        //
        // GET: /AgreementDetailDocumentUpload/Details/5

        public ViewResult Details(int id)
        {
            AgreementDetailDocumentUpload agreementdetaildocumentupload = db.AgreementDetailDocumentUploads.Find(id);
            return View(agreementdetaildocumentupload);
        }

        //
        // GET: /AgreementDetailDocumentUpload/Create

        public ActionResult Create(int Id)
        {
            AgreementDetailDocumentUpload documentUpload = new AgreementDetailDocumentUpload();
            documentUpload.AgreementDetailID = Id;
            documentUpload.CreateBy = HttpContext.User.Identity.Name;
            documentUpload.CreateDateTime = DateTime.Now;
            documentUpload.ModifiedBy =  null;
            documentUpload.ModifiedDateTime = null;
            documentUpload.FileName = "Dummy";
            documentUpload.DocumentType = Models.Enums.EnumDocumentType.Other;
            documentUpload.DocumentTypeId = 0;
            documentUpload.Status = true;

            AgreementDetail AgreementDetails = db.AgreementDetails.Find(Id);
            documentUpload.AgreementDetails = AgreementDetails;

            return View(documentUpload);
        }

        //
        // POST: /AgreementDetailDocumentUpload/Create

        [HttpPost]
        public ActionResult Create(AgreementDetailDocumentUpload agreementdetaildocumentupload, int Id, string FileName, bool SecondChecker)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AgreementDetailDocumentUpload documentUpload = new AgreementDetailDocumentUpload();
                    documentUpload.AgreementDetailID = Id;
                    documentUpload.CreateBy = HttpContext.User.Identity.Name;
                    documentUpload.CreateDateTime = DateTime.Now;
                    documentUpload.ModifiedBy = null;
                    documentUpload.ModifiedDateTime = null;
                    documentUpload.SecondChecker = SecondChecker;
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
                            db.AgreementDetailDocumentUploads.Add(documentUpload);
                            db.SaveChanges();
                            uploadedCount++;
                        }
                    }
                    return RedirectToAction("Details", "AgreementDetail",
                            new System.Web.Routing.RouteValueDictionary {
                                { "id", Id },
                                { "saveChangesError", true }});
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(agreementdetaildocumentupload);
        }

        //
        // GET: /AgreementDetailDocumentUpload/Edit/5

        public ActionResult Edit(int id)
        {
            AgreementDetailDocumentUpload agreementdetaildocumentupload = db.AgreementDetailDocumentUploads.Find(id);
            return View(agreementdetaildocumentupload);
        }

        //
        // POST: /AgreementDetailDocumentUpload/Edit/5

        [HttpPost]
        public ActionResult Edit(AgreementDetailDocumentUpload agreementdetaildocumentupload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agreementdetaildocumentupload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agreementdetaildocumentupload);
        }

        //
        // GET: /AgreementDetailDocumentUpload/Delete/5

        public ActionResult Delete(int id)
        {
            AgreementDetailDocumentUpload agreementdetaildocumentupload = db.AgreementDetailDocumentUploads.Find(id);
            return View(agreementdetaildocumentupload);
        }

        //
        // POST: /AgreementDetailDocumentUpload/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int Id)
        {
            AgreementDetailDocumentUpload agreementDetailDocumentUpload = db.AgreementDetailDocumentUploads.Find(Id);
            int AgreementDetailID = agreementDetailDocumentUpload.AgreementDetailID;
            db.Entry(agreementDetailDocumentUpload).State = EntityState.Detached;
            try
            {
                AgreementDetailDocumentUpload agreementDetailDocumentUploadToDelete = new AgreementDetailDocumentUpload() { AgreementDetailDocumentUploadID = Id};
                db.Entry(agreementDetailDocumentUploadToDelete).State = EntityState.Deleted;
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
            return RedirectToAction("Details", "AgreementDetail",
                        new System.Web.Routing.RouteValueDictionary {
                                { "id", AgreementDetailID },
                                { "saveChangesError", true }});


        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
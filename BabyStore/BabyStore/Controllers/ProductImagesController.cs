using BabyStore.DAL;
using BabyStore.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BabyStore.Controllers
{
    public class ProductImagesController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: ProductImages
        public ActionResult Index()
        {
            return View(db.ProductImages.ToList());
        }

        // GET: ProductImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // GET: ProductImages/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Upload(HttpPostedFileBase file)
        //{
        //    //check the user has entered a file
        //    if (file != null)
        //    {
        //        //check if the file is valid
        //        if (ValidateFile(file))
        //        {
        //            try
        //            {
        //                SaveFileToDisk(file);
        //            }
        //            catch (Exception)
        //            {
        //                ModelState.AddModelError("FileName", "Sorry an error occurred saving the file to disk, please try again");
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("FileName", "The file must be gif, jpeg, jpg or png and less than 2MB in size");
        //        }
        //    }
        //    else
        //    {
        //        //if the user has not entered a file return an error message
        //        ModelState.AddModelError("FileName", "Please choose a file");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        db.ProductImages.Add(new ProductImage { FileName = file.FileName });
        //        try
        //        {
        //            db.SaveChanges();
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            SqlException innerException = ex.InnerException.InnerException as SqlException;
        //            if (innerException != null && innerException.Number == 2601)
        //            {
        //                ModelState.AddModelError("FileName", "The file " + file.FileName + "already exists in the system. Please delete it and try again if you wish to re-add it");
        //            }
        //            else
        //            {
        //                ModelState.AddModelError("FileName", "Sorry an error has occurred saving to database, please try again");
        //            }
        //            return View();
        //        }
        //        return RedirectToAction("Index");
        //    }

        //    return View();
        //}

        //POST: ProductImages/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            bool allValid = true;
            string inValidFiles = "";
            db.Database.Log = sql => Trace.WriteLine(sql);

            //check that the user has entered a file
            if (files[0] != null)
            {
                //check if user has entered less than ten files
                if (files.Length <= 10)
                {
                    //check thay are all valid
                    foreach (var file in files)
                    {
                        if (!ValidateFile(file))
                        {
                            allValid = false;
                            inValidFiles += ", " + file.FileName;
                        }
                    }
                    //check if they are all valid and save to disk
                    if (allValid)
                    {
                        foreach (var file in files)
                        {
                            try
                            {
                                SaveFileToDisk(file);
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("FileName",
                                    "Sorry an error ocurred saving the files to disk, please try again");
                            }
                        }
                    }
                    //else add an error listing out the invalid files
                    else
                    {
                        ModelState.AddModelError("FileName",
                            "All files must be gif, jpeg, jpg or png and less than 2MB in size. The following files" +
                            inValidFiles + " are not valid");
                    }
                }
                //the user has entered more than 10 files
                else
                {
                    ModelState.AddModelError("FileName", "Please only upload up to ten files at a time");
                }
            }
            else
            {
                //if user has not entered a file return an error message
                ModelState.AddModelError("FileName", "Please choose a file");
            }

            //Check modelstate validation
            if (ModelState.IsValid)
            {
                bool duplicates = false;
                bool otherDbError = false;
                string duplicateFiles = "";

                foreach (var file in files)
                {
                    //try and save each file
                    var productToAdd = new ProductImage { FileName = file.FileName };
                    try
                    {
                        db.ProductImages.Add(productToAdd);
                        db.SaveChanges();
                    }
                    //if there is an exception check if it caused by a duplicate file
                    catch (DbUpdateException ex)
                    {
                        SqlException innerException = ex.InnerException.InnerException as SqlException;
                        if (innerException != null && innerException.Number == 2601)
                        {
                            duplicateFiles += ", " + file.FileName;
                            duplicates = true;
                            db.Entry(productToAdd).State = EntityState.Detached;
                        }
                        else
                        {
                            otherDbError = true;
                        }
                    }
                }
                //add a list of duplicates files to the error message
                if (duplicates)
                {
                    ModelState.AddModelError("FileName", "All files uploaded except the files" + duplicateFiles +
                        ", which already exist in the system." +
                        " Please delete them and try again if you wish to re-add them");
                    return View();
                }
                else if (otherDbError)
                {
                    ModelState.AddModelError("FileName", "Sorry an error has occurred saving to the databse, please try again");
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        // GET: ProductImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FileName")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            string[] allowedFileTypes = { ".gif", ".png", ".jpeg", ".jpg" };

            return (file.ContentLength > 0 && file.ContentLength < 2097152) && allowedFileTypes.Contains(fileExtension);
        }

        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);

            //Normal Image size
            if (img.Width > 190)
                img.Resize(190, img.Height);
            img.Save(Constants.ProductImagePath + file.FileName);

            //Thumbnails Image size
            if (img.Width > 100)
                img.Resize(100, img.Height);
            img.Save(Constants.ProductThumbnailPath + file.FileName);
        }
    }
}

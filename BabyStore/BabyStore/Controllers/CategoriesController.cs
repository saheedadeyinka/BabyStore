﻿using BabyStore.DAL;
using BabyStore.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BabyStore.Controllers
{
    [System.Runtime.InteropServices.Guid("71F257D0-0BCA-42FE-81BA-EA776998102C")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Categories
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Categories.OrderBy(c => c.Name).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "Name", "RowVersion" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var categoryToUpdate = db.Categories.Find(id);

            if (categoryToUpdate == null)
            {
                Category deletedCategory = new Category();
                TryUpdateModel(deletedCategory, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to Save your changes because the category has been deleted by another user");
                return View(deletedCategory);
            }

            if (TryUpdateModel(categoryToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(categoryToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exEntry = ex.Entries.Single();
                    var currentUIValues = (Category)exEntry.Entity;
                    var databaseCategory = exEntry.GetDatabaseValues();

                    if (databaseCategory == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to Save your changes because the category has been deleted by another user");
                    }
                    else
                    {
                        var databaseCategoryValues = (Category)databaseCategory.ToObject();

                        if (databaseCategoryValues.Name != currentUIValues.Name)
                        {
                            ModelState.AddModelError("Name", "Current value in database: " + databaseCategoryValues.Name);
                        }
                        ModelState.AddModelError(string.Empty,
                            "The record has been modidified by another user after you loaded the screen." +
                            "Your changes have not yet been saved. The new values in the database are shown below." +
                            "If you want to overwrite these values with changes then click save otherwise go back to the categories page.");
                        categoryToUpdate.RowVersion = databaseCategoryValues.RowVersion;
                    }
                }
            }
            return View(categoryToUpdate);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id, bool? deletionError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                if (deletionError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (deletionError.GetValueOrDefault())
            {
                ModelState.AddModelError(string.Empty,
                            "The category you attempted to delete has been modidified by another user after you loaded it." +
                            "The deletion has not been performed. The current values in the database are shown above." +
                            "If you still want to delete this record click the Delete button again, otherwise go back to the categories page.");
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Category category)
        {
            try
            {
                db.Entry(category).State = EntityState.Deleted;
                var products = db.Products.Where(p => p.CategoryId == category.Id);

                foreach (var product in products)
                {
                    product.CategoryId = null;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { deletionError = true, id = category.Id });
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

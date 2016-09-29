using BabyStore.DAL;
using BabyStore.Models;
using BabyStore.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BabyStore.Controllers
{
    public class ProductsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Products
        //public ActionResult Index(string category, string search)
        //{
        //    var products = db.Products.Include(p => p.Category);

        //    if (!String.IsNullOrEmpty(category))
        //        products = products.Where(p => p.Category.Name == category);

        //    if (!String.IsNullOrEmpty(search))
        //    {
        //        products = products.Where(p => p.Name.Contains(search) ||
        //                                       p.Description.Contains(search) ||
        //                                       p.Category.Name.Contains(search));
        //        ViewBag.Search = search;
        //    }

        //    var categories = products
        //        .OrderBy(p => p.Category.Name)
        //        .Select(p => p.Category.Name)
        //        .Distinct();

        //    if (!String.IsNullOrEmpty(category))
        //    {
        //        products = products.Where(p => p.Category.Name == category);
        //    }

        //    ViewBag.Category = new SelectList(categories);

        //    return View(products.ToList());
        //}

        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            //Instantiate a new viewModel
            ProductIndexViewModel viewModel = new ProductIndexViewModel();

            //select the product
            var products = db.Products.Include(p => p.Category);

            //perform the search and save the search string a the viewModel
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) ||
                                                p.Description.Contains(search) ||
                                                p.Category.Name.Contains(search));

                viewModel.Search = search;
            }

            //Group search results into categories and count how many items in each category
            viewModel.CategoryWithCounts = from matchingProducts in products
                                           where
                                               matchingProducts.CategoryId != null
                                           group matchingProducts by
                                               matchingProducts.Category.Name
                                            into
                                                categoryGroup
                                           select new CategoryWithCount()
                                           {
                                               CategoryName = categoryGroup.Key,
                                               ProductCount = categoryGroup.Count()
                                           };
            //viewModel.CategoryWithCounts =
            //    products.Where(matchingProducts => matchingProducts.CategoryId != null)
            //        .GroupBy(matchingProducts => matchingProducts.Category.Name)
            //        .Select(categoryGroup => new CategoryWithCount()
            //        {
            //            CategoryName = categoryGroup.Key,
            //            ProductCount = categoryGroup.Count()
            //        });

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
                viewModel.Category = category;
            }

            //sort the result by price
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;

            }

            //viewModel.Products = products;
            const int PagedItems = 3;
            int currentPage = (page ?? 1);
            viewModel.Products = products.ToPagedList(currentPage, PagedItems);
            viewModel.SortBy = sortBy;

            viewModel.Sorts = new Dictionary<string, string>
            {
                {"Price low to high", "price_lowest"},
                {"Price high to low", "price_highest"},
            };

            return View(viewModel);
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
    }
}

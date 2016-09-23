using BabyStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BabyStore.ViewModels
{
    public class ProductIndexViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public string Search { get; set; }
        public IEnumerable<CategoryWithCount> CategoryWithCounts { get; set; }
        public string Category { get; set; }

        public IEnumerable<SelectListItem> CategoryFilterItems
        {
            get
            {
                var allCategories = CategoryWithCounts.Select(cc => new SelectListItem
                {
                    Value = cc.CategoryName,
                    Text = cc.CategoryNameWithCount
                });
                return allCategories;
            }
        }
    }

    public class CategoryWithCount
    {
        public int ProductCount { get; set; }
        public string CategoryName { get; set; }

        public string CategoryNameWithCount => CategoryName + " (" + ProductCount.ToString() + ")";
    }
}
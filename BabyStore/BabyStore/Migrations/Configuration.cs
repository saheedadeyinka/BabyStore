using BabyStore.Models;
using System.Collections.Generic;
using System.Linq;

namespace BabyStore.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BabyStore.DAL.StoreContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BabyStore.DAL.StoreContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var categories = new List<Category>
            {
                new Category {Name = "Clothes"},
                new Category {Name = "Play and Toys"},
                new Category {Name = "Feeding"},
                new Category {Name = "Medicine"},
                new Category {Name = "Travel"},
                new Category {Name = "Sleeping"}

            };
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Sleep Suit",
                    Description = "For sleeping or general wear",
                    Price = 4.99M,
                    CategoryId = categories.Single(c => c.Name == "Clothes").Id
                },
                new Product
                {
                    Name = "Vest",
                    Description = "For sleeping or general wear",
                    Price = 2.99M,
                    CategoryId = categories.Single(c => c.Name == "Clothes").Id
                },
                new Product
                {
                    Name = "Orange and Yellow Lion",
                    Description = "Makes a squeaking noise",
                    Price = 1.99M,
                    CategoryId = categories.Single(c => c.Name == "Play and Toys").Id
                },
                new Product
                {
                    Name = "Blue Rabbit",
                    Description = "Baby comforter",
                    Price = 2.99M,
                    CategoryId = categories.Single(c => c.Name == "Play and Toys").Id
                },
                new Product
                {
                    Name = "3 Pack of Bottles",
                    Description = "For a leak free drink everytime",
                    Price = 24.99M,
                    CategoryId = categories.Single(c => c.Name == "Feeding").Id
                },
                new Product
                {
                    Name = "3 Pack of Bibs",
                    Description = "Keep your baby dry when feeding",
                    Price = 8.99M,
                    CategoryId = categories.Single(c => c.Name == "Feeding").Id
                },
                new Product
                {
                    Name = "Powered Baby Milk",
                    Description = "Nutritional and Tasty",
                    Price = 9.99M,
                    CategoryId = categories.Single(c => c.Name == "Feeding").Id
                },
                new Product
                {
                    Name = "Pack of 70 Disposable Nappies",
                    Description = "Dry and secure nappies with snug fit",
                    Price = 19.99M,
                    CategoryId = categories.Single(c => c.Name == "Feeding").Id
                },
                new Product
                {
                    Name = "Colic Medicine",
                    Description = "For helping with baby colic pains",
                    Price = 4.99M,
                    CategoryId = categories.Single(c => c.Name == "Medicine").Id
                },
                new Product
                {
                    Name = "Reflux Medicine",
                    Description = "Helps to prevent milk regurgitation and sickness",
                    Price = 4.99M,
                    CategoryId = categories.Single(c => c.Name == "Medicine").Id
                },
                new Product
                {
                    Name = "Black Pram and Pushchair System",
                    Description = "Convert fron pram to pushchair, with raincover",
                    Price = 299.99M,
                    CategoryId = categories.Single(c => c.Name == "Travel").Id
                },
                new Product
                {
                    Name = "Car Seat",
                    Description = "For safe car travel",
                    Price = 49.99M,
                    CategoryId = categories.Single(c => c.Name == "Travel").Id
                },
                new Product
                {
                    Name = "Moses Basket",
                    Description = "Plastic moses basket",
                    Price = 75.99M,
                    CategoryId = categories.Single(c => c.Name == "Sleeping").Id
                },
                new Product
                {
                    Name = "Crib",
                    Description = "Wooden crib",
                    Price = 35.99M,
                    CategoryId = categories.Single(c => c.Name == "Sleeping").Id
                },
                new Product
                {
                    Name = "Cot Bed",
                    Description = "Converts from cot into bed for older children",
                    Price = 149.99M,
                    CategoryId = categories.Single(c => c.Name == "Sleeping").Id
                },
                new Product
                {
                    Name = "Circus Crib Bale",
                    Description = "Contains sheet, duvet and bumper",
                    Price = 29.99M,
                    CategoryId = categories.Single(c => c.Name == "Sleeping").Id
                },
                new Product
                {
                    Name = "Love Crib Bale",
                    Description = "Contains sheet, duvet and bumper",
                    Price = 35.99M,
                    CategoryId = categories.Single(c => c.Name == "Sleeping").Id
                }
            };

            products.ForEach(c => context.Products.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();
        }
    }
}


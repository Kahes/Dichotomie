using Dichotomie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DichotomieWeb.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();

            // USER
            var password = "Azerty0!";
            var user1 = "user1@test.com";
            var user2 = "user2@test.com";
            var users = new ApplicationUser[]
                {
                    new ApplicationUser { UserName = user1, Email = user1 },
                    new ApplicationUser { UserName = user2, Email = user2 }
                };
            if (!context.Users.Any())
            {
                foreach (ApplicationUser user in users)
                {
                    userManager.CreateAsync(user, password);
                }
            }

            // CATEGORIES

            //context.Categories.RemoveRange(context.Categories);
            //context.SaveChanges();

            var categoryOne = "Category1";
            var categoryTwo = "Category2";
            var subCategoryOne = "SubCategory1";
            var subCategoryTwo = "SubCategory2";
            var subCategoryThree = "SubCategory3";
            var subCategoryFour = "SubCategory4";
            var categories = new Category[]
                {
                    new Category { Name = categoryOne },
                    new Category { Name = categoryTwo }
                };
            var subCategories = new Category[]
                {
                    new Category { Name = categoryOne + subCategoryOne , ParentCategory= categories[0]},
                    new Category { Name = categoryOne + subCategoryTwo , ParentCategory = categories[0]},
                    new Category { Name = categoryOne + subCategoryThree , ParentCategory = categories[0]},
                    new Category { Name = categoryTwo + subCategoryOne , ParentCategory = categories[1]},
                    new Category { Name = categoryTwo + subCategoryTwo , ParentCategory = categories[1]},
                    new Category { Name = categoryTwo + subCategoryThree , ParentCategory = categories[1]},
                    new Category { Name = categoryTwo + subCategoryFour , ParentCategory = categories[1]},
                };
            if (!context.Categories.Any())
            {
                foreach (Category category in categories)
                {
                    context.Categories.Add(category);
                }
                foreach (Category subcategory in subCategories)
                {
                    context.Categories.Add(subcategory);
                }
            }

            // TOPIC

            //context.Topics.RemoveRange(context.Topics);
            //context.SaveChanges();

            var topics = new Topic[]
                {
                    new Topic
                    {
                        Category = subCategories[0],
                        User = users[0],
                        Pin = 0,
                        Title = categoryOne + subCategoryOne + "Title1",
                        CurrencyUsed = 0,
                        MainContent = categoryOne + subCategoryOne + "MainContent1",
                        Rating = 0,
                        State = 0,
                        TradeSystem = "Po",
                        CreationDate = DateTime.Now,
                        ModificationDate = DateTime.Now,
                    },
                };
            if (!context.Topics.Any())
            {
                foreach (Topic topic in topics)
                {
                    context.Topics.Add(topic);
                }
            }
            context.SaveChanges();
        }
    }
}

using Dichotomie.Models;
using Microsoft.AspNetCore.Identity;
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

            if (!context.Users.Any())
            {
                var users = new ApplicationUser[]
                {
                    new ApplicationUser { UserName = "philippe.douelle@epitech.eu", Email = "philippe.douelle@epitech.eu" },
                    new ApplicationUser { UserName = "charles.pamart@epitech.eu", Email = "charles.pamart@epitech.eu" }
                };
                foreach (ApplicationUser user in users)
                {
                    userManager.CreateAsync(user, "Azerty0!");
                }
                context.SaveChanges();
            }

            if (!context.Categories.Any())
            {
                var categoryWow = "World of Warcraft";
                var categoryGW = "Guild Wars";
                var categories = new Category[]
                {
                    new Category { Name = categoryWow },
                    new Category { Name = categoryGW }
                };
                foreach (Category category in categories)
                {
                    context.Categories.Add(category);
                }
                context.SaveChanges();

                var subCategoryPVP = "PVP";
                var subCategoryPVE = "PVE";
                var subCategories = new Category[]
                {
                    new Category { Name = subCategoryPVP , ParentCategoryId = context.Categories.Where(q => q.Name == categoryWow).FirstOrDefault().CategoryId},
                    new Category { Name = subCategoryPVE , ParentCategoryId = context.Categories.Where(q => q.Name == categoryWow).FirstOrDefault().CategoryId},
                    new Category { Name = "Voleur" , ParentCategoryId = context.Categories.Where(q => q.Name == categoryWow).FirstOrDefault().CategoryId},
                    new Category { Name = subCategoryPVP , ParentCategoryId = context.Categories.Where(q => q.Name == categoryGW).FirstOrDefault().CategoryId},
                    new Category { Name = subCategoryPVE , ParentCategoryId = context.Categories.Where(q => q.Name == categoryGW).FirstOrDefault().CategoryId},
                    new Category { Name = "Mesmer" , ParentCategoryId = context.Categories.Where(q => q.Name == categoryGW).FirstOrDefault().CategoryId},
                    new Category { Name = "Necromancer" , ParentCategoryId = context.Categories.Where(q => q.Name == categoryGW).FirstOrDefault().CategoryId},
                };
                foreach (Category subcategory in subCategories)
                {
                    context.Categories.Add(subcategory);
                }
                context.SaveChanges();
            }
        }
    }
}

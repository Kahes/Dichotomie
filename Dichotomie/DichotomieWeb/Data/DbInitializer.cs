using Dichotomie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DichotomieWeb.Data
{
    public static class DbInitializer
    {
        private static int minUsers = 25;
        private static int maxUsers = 100;

        private static int minCategories = 2;
        private static int maxCategories = 4;

        private static int minSubCategories = 2;
        private static int maxSubCategories = 4;

        private static int minTopics = 1;
        private static int maxTopics = 10;

        private static int minReplies = 1;
        private static int maxReplies = 10;

        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();

            // USER
            if (!context.Users.Any())
            {
                var users = new List<ApplicationUser>();
                int randomNumberUser = new Random().Next(minUsers, maxUsers);
                for (var i = 0; i < randomNumberUser; i++)
                {
                    var email = $"user{i}@test.com";
                    users.Add(new ApplicationUser { UserName = email, Email = email });
                }
                var password = "Azerty0!";
                foreach (ApplicationUser user in users)
                {
                    userManager.CreateAsync(user, password).Wait();
                }
                context.SaveChanges();
            }

            // CATEGORIES
            if (!context.Categories.Any())
            {
                var categories = new List<Category>();
                var subCategories = new List<Category>();
                int randomNumberCategory = new Random().Next(minCategories, maxCategories);
                for (var i = 0; i < randomNumberCategory; i++)
                {
                    var category = new Category { Name = $"Category{i}" };
                    categories.Add(category);
                    int randomNumberSubCategory = new Random().Next(minSubCategories, maxSubCategories);
                    for (var j = 0; j < randomNumberSubCategory; j++)
                    {
                        var subCategory = new Category { Name = $"{category.Name}SubCategory{j}", ParentCategory = category };
                        subCategories.Add(subCategory);
                    }
                }
                foreach (var category in categories)
                {
                    context.Categories.Add(category);
                }
                foreach (var subcategory in subCategories)
                {
                    context.Categories.Add(subcategory);
                }
                context.SaveChanges();
            }

            // TOPICS
            if (!context.Topics.Any())
            {
                var topics = new List<Topic>();
                var categories = context.Categories
                    .Include(c => c.SubCategories)
                        .ThenInclude(t => t.Topics)
                            .ThenInclude(r => r.Replies)
                    .Where(c => c.ParentCategoryId == null)
                    .ToList();
                foreach (var category in categories)
                {
                    foreach (var subCategory in category.SubCategories)
                    {
                        int randomNumberTopic = new Random().Next(minTopics, maxTopics);
                        for (var i = 0; i < randomNumberTopic; i++)
                        {
                            var usersArray = context.Users.ToArray();
                            int randomNumberUser = new Random().Next(0, usersArray.Count() - 1);
                            var randomUser = usersArray[randomNumberUser];
                           
                            var topic = new Topic
                            {
                                Category = subCategory,
                                User = randomUser,
                                Pin = 0,
                                Title = $"{category.Name}{subCategory.Name}Title{i}",
                                CurrencyUsed = 0,
                                Rating = 0,
                                State = 0,
                                TradeSystem = "PO",
                                CreationDate = DateTime.Now,
                                ModificationDate = DateTime.Now
                            };
                            var reply = new Reply
                            {
                                MainContent = $"{category.Name}{subCategory.Name}MainContent{i}",
                                CreationDate = DateTime.Now,
                                ModificationDate = DateTime.Now,
                                Topic = topic,
                                User = randomUser,
                            };
                            topic.Replies = new List<Reply>();
                            topic.Replies.Add(reply);
                            topics.Add(topic);
                        }
                    }
                }
                foreach (var topic in topics)
                {
                    context.Topics.Add(topic);
                }
                context.SaveChanges();

                // REPLIES
                var replies = new List<Reply>();
                foreach (var topic in context.Topics)
                {
                    int randomNumberReply = new Random().Next(minReplies, maxReplies);
                    for (var i = 0; i < randomNumberReply; i++)
                    {
                        var usersArray = context.Users.ToArray();
                        int randomNumber = new Random().Next(0, usersArray.Count() - 1);
                        var randomUser = usersArray[randomNumber];
                        var reply = new Reply
                        {
                            CreationDate = DateTime.Now,
                            MainContent = $"{topic.Category.ParentCategory.Name}{topic.Category.Name}{topic.Title}MainContent{i}",
                            ModificationDate = DateTime.Now,
                            Topic = topic,
                            User = randomUser,
                        };
                        replies.Add(reply);
                    }
                }
                foreach (var reply in replies)
                {
                    context.Replies.Add(reply);
                }
                context.SaveChanges();
            }
        }
    }
}

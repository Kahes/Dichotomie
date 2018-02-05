using Dichotomie.Models;
using DichotomieWeb.Models;
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
        private static int minAdmins = 1;
        private static int maxAdmins = 2;

        private static int minModerators = 3;
        private static int maxModerators = 4;

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

        private static int minReputation = 10;
        private static int maxReputation = 25;

        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // USER
            if (!context.Users.Any())
            {
                string[] roleNames = { "Admin", "Moderator", "Member" };
                foreach (var roleName in roleNames)
                {
                    roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                }

                var admins = new List<ApplicationUser>();
                int randomNumberAdmin = new Random().Next(minAdmins, maxAdmins);
                for (var i = 0; i < randomNumberAdmin; i++)
                {
                    var email = $"admin{i}@test.com";
                    admins.Add(new ApplicationUser { UserName = email, Email = email });
                }

                var moderators = new List<ApplicationUser>();
                int randomNumberModerator = new Random().Next(minModerators, maxModerators);
                for (var i = 0; i < randomNumberModerator; i++)
                {
                    var email = $"moderator{i}@test.com";
                    moderators.Add(new ApplicationUser { UserName = email, Email = email });
                }

                var users = new List<ApplicationUser>();
                int randomNumberUser = new Random().Next(minUsers, maxUsers);
                for (var i = 0; i < randomNumberUser; i++)
                {
                    var email = $"user{i}@test.com";
                    users.Add(new ApplicationUser { UserName = email, Email = email });
                }

                var passwordAdmin = "Admin0!";
                foreach (ApplicationUser admin in admins)
                {
                    userManager.CreateAsync(admin, passwordAdmin).Wait();
                    userManager.AddToRoleAsync(admin, "Admin").Wait();
                }

                var passwordModerator = "Moderator0!";
                foreach (ApplicationUser moderator in moderators)
                {
                    userManager.CreateAsync(moderator, passwordModerator).Wait();
                    userManager.AddToRoleAsync(moderator, "Moderator").Wait();
                }

                var password = "Azerty0!";
                foreach (ApplicationUser user in users)
                {
                    userManager.CreateAsync(user, password).Wait();
                    userManager.AddToRoleAsync(user, "Member").Wait();
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
                                Pin = i < 2,
                                Title = $"{category.Name}{subCategory.Name}Title{i}",
                                CurrencyUsed = 0,
                                Rating = 0,
                                Close = false,
                                TradeSystem = "PO",
                                CreationDate = DateTime.Now.AddSeconds(i),
                                ModificationDate = DateTime.Now
                            };
                            var reply = new Reply
                            {
                                MainContent = $"{category.Name}{subCategory.Name}MainContent{i}",
                                CreationDate = DateTime.Now.AddSeconds(i),
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

                // REPUTATION
                if (!context.Reputations.Any())
                {
                    var reputations = new List<Reputation>();
                    var usersArrays = context.Users.ToArray();
                    int randomNumberReputation = new Random().Next(minReputation, maxReputation);
                    int randomNumberUserGenerator = new Random().Next(0, usersArrays.Count() - 1);
                    var randomUsers = usersArrays[randomNumberUserGenerator];
                    int randomNumberUserGeneratorBIS = new Random().Next(0, usersArrays.Count() - 1);
                    var randomUsersBIS = usersArrays[randomNumberUserGeneratorBIS];

                    for (var i = 0; i < randomNumberReputation; i++)
                    {
                        var reputation = new Reputation
                        {
                            User = randomUsers,
                            FromUser = randomUsersBIS,
                            MarkValue = new Random().Next(0, 5),
                        };
                        reputations.Add(reputation);
                    }
                    foreach(Reputation rep in reputations)
                    {
                        context.Reputations.Add(rep);
                    }
                    context.SaveChanges();
                }

                // NEWS
                if (!context.HomeNews.Any())
                {
                    var news = new List<HomeNews>();
                    for (var i = 0; i < 5; i++)
                    {
                        var newstoadd = new HomeNews
                        {
                            Title = $"TITLE{i}",
                            Content = $"This text is here to prove that this news is fully working. If you don't trust me just look at how awesome it is. A lot of informations in one simple text. I'm the number : {i}"
                        };
                        news.Add(newstoadd);
                    }
                    foreach (HomeNews newsfor in news)
                    {
                        context.HomeNews.Add(newsfor);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}

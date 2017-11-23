using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace DichotomieWeb.Pages.Forum.Topics
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Category Category { get; set; }

        [BindProperty]
        public Topic Topic { get; set; }

        [BindProperty]
        public Reply Reply { get; set; }

        public async Task<IActionResult> OnGetAsync(int categoryId)
        {
            Category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId == categoryId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
 
            Topic.Pin = false;
            Topic.Rating = 0;
            Topic.User = await _userManager.GetUserAsync(User);
            Topic.CreationDate = DateTime.Now;
            Topic.ModificationDate = DateTime.Now;
            
            Reply.User = Topic.User;
            Reply.Topic = Topic;
            Reply.CreationDate = DateTime.Now;
            Reply.ModificationDate = DateTime.Now;

            Topic.Replies = new List<Reply>();
            Topic.Replies.Add(Reply);

            _context.Topics.Add(Topic);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { categoryId = Topic.CategoryFK });
        }
    }
}
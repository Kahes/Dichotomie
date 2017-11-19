using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DichotomieWeb.Pages.Forum.Replies
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

        public Topic Topic { get; set; }

        [BindProperty]
        public Reply Reply { get; set; }

        public async Task<IActionResult> OnGetAsync(int topicId)
        {
            Topic = await _context.Topics.SingleOrDefaultAsync(t => t.TopicId == topicId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Reply.User = await _userManager.GetUserAsync(User);
            Reply.CreationDate = DateTime.Now;
            Reply.ModificationDate = DateTime.Now;

            _context.Replies.Add(Reply);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { topicId = Reply.TopicFK} );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Identity;

namespace DichotomieWeb.Pages.Forum.Replies
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Reply Reply { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reply = await _context.Replies
                .Include(r => r.Topic)
                .Include(r => r.User)
                .SingleOrDefaultAsync(m => m.ReplieId == id);

            if (Reply == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reply = await _context.Replies.FindAsync(id);

            if (Reply != null)
            {
                var moderator = await _userManager.GetUserAsync(User);
                Reply.MainContent = $"Your message has been delete by moderator : {moderator.UserName}";
                Reply.ModificationDate = DateTime.Now;
                _context.Replies.Update(Reply);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { topicId = Reply.TopicFK});
        }
    }
}

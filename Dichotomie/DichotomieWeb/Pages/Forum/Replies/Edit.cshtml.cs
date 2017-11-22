using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Authorization;

namespace DichotomieWeb.Pages.Forum.Replies
{
    [Authorize(Roles = "Admin, Moderator")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var reply = await _context.Replies.SingleOrDefaultAsync(r => r.ReplieId == Reply.ReplieId);
            if (reply == null)
            {
                return NotFound();
            }
            reply.MainContent = Reply.MainContent;
            reply.ModificationDate = DateTime.Now;
            _context.Replies.Update(reply);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index", new { topicId = reply.TopicFK});
        }
    }
}

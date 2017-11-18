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

namespace DichotomieWeb.Pages.Forum.Replies
{
    public class EditModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public EditModel(DichotomieWeb.Data.ApplicationDbContext context)
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

            Reply = await _context.Reply
                .Include(r => r.Topic)
                .Include(r => r.User).SingleOrDefaultAsync(m => m.ReplieId == id);

            if (Reply == null)
            {
                return NotFound();
            }
           ViewData["TopicFK"] = new SelectList(_context.Topic, "TopicId", "MainContent");
           ViewData["UserFK"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Reply).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReplyExists(Reply.ReplieId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ReplyExists(int id)
        {
            return _context.Reply.Any(e => e.ReplieId == id);
        }
    }
}

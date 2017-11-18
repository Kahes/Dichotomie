using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;

namespace DichotomieWeb.Pages.Forum.Replies
{
    public class DeleteModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public DeleteModel(DichotomieWeb.Data.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Reply = await _context.Reply.FindAsync(id);

            if (Reply != null)
            {
                _context.Reply.Remove(Reply);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

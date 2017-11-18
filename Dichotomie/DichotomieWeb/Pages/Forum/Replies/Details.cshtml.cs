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
    public class DetailsModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public DetailsModel(DichotomieWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}

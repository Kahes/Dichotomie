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
    public class IndexModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public IndexModel(DichotomieWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Reply> Reply { get;set; }

        public async Task OnGetAsync()
        {
            Reply = await _context.Reply
                .Include(r => r.Topic)
                .Include(r => r.User).ToListAsync();
        }
    }
}

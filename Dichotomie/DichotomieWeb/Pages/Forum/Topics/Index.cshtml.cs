using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;

namespace DichotomieWeb.Pages.Forum.Topics
{
    public class IndexModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public IndexModel(DichotomieWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Topic> Topics { get;set; }

        public async Task OnGetAsync(int categoryId)
        {
            Topics = await _context.Topic
                .Include(t => t.Replies)
                .Where(t => t.CategoryFK ==  categoryId)
                .ToListAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.EntityFrameworkCore;
using DichotomieWeb.Models;

namespace DichotomieWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Topic> Topics;
        public List<HomeNews> News;

        public async Task OnGetAsync()
        {
            Topics = await _context.Topics
                .Include(t => t.Replies)
                    .ThenInclude(r => r.User)
                .OrderByDescending(t => t.CreationDate)
                .Take(5)
                .ToListAsync();

            News = await _context.HomeNews.ToListAsync();
        }
    }
}

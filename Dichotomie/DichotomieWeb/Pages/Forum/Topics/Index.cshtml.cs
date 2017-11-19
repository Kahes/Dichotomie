using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Authorization;

namespace DichotomieWeb.Pages.Forum.Topics
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CategoryId { get; set; }
        public IList<Topic> Topics { get;set; }

        public async Task OnGetAsync(int categoryId)
        {
            CategoryId = categoryId;

            Topics = await _context.Topics
                .Include(t => t.Replies)
                    .ThenInclude(r => r.User)
                .Where(t => t.CategoryFK ==  categoryId)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();

            foreach (var topic in Topics)
            {
                topic.Replies = topic.Replies.OrderByDescending(r => r.CreationDate).ToList();
            }
        }
    }
}

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

namespace DichotomieWeb.Pages.Forum.Categories
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get;set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.Topics)
                        .ThenInclude(t => t.User)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();

            foreach(var category in Categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    subCategory.Topics = subCategory.Topics.OrderByDescending(t => t.CreationDate).ToList();
                }
            }
        }
    }
}

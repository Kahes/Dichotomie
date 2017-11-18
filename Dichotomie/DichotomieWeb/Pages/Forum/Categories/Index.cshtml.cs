using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;

namespace DichotomieWeb.Pages.Forum.Categories
{
    public class IndexModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public IndexModel(DichotomieWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get;set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Category
                .Include(c => c.SubCategories)
                    .ThenInclude(sc => sc.Topics)
                .Where(c => c.ParentCategoryId == null)
                .ToListAsync();
        }
    }
}

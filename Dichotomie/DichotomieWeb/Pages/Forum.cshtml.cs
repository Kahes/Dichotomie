using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DichotomieWeb.Data;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;

namespace DichotomieWeb.Pages
{
    [Authorize]
    public class ForumModel : PageModel
    {
        private ApplicationDbContext _context;

        public List<Category> _categories;

        public ForumModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            _categories = _context.Categories.Include(i => i.SubCategories).Where(w => w.ParentCategoryId == null).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DichotomieWeb.Pages.Forum.Categories
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Category Category { get; set; }
        public List<Category> SubCategories { get; set; }
        public List<SelectListItem> CategoriesSelectItem { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CategoriesSelectItem = new List<SelectListItem>();
            SubCategories = await _context.Categories
                .Where(c => c.ParentCategoryId != null)
                .ToListAsync();

            CategoriesSelectItem.Add(new SelectListItem { Value = null, Text = "" }); // Default SelectItem
            foreach (Category category in SubCategories)
            {
                CategoriesSelectItem.Add(new SelectListItem { Value = category.CategoryId.ToString(), Text = category.Name });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
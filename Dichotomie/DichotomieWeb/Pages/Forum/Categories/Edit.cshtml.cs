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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Category Category { get; set; }
        public List<Category> SubCategories { get; set; }
        public List<SelectListItem> CategoriesSelectItem { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _context.Categories
                .SingleOrDefaultAsync(m => m.CategoryId == id);

            if (Category == null)
            {
                return NotFound();
            }

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

            var category = await _context.Categories.SingleOrDefaultAsync(r => r.CategoryId == Category.CategoryId);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = Category.Name;
            _context.Categories.Update(category);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}
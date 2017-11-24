using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;

namespace DichotomieWeb.Pages.Forum.Topics
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Topic Topic { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Topic = await _context.Topics
                .Include(t => t.Category)
                    .ThenInclude(c => c.ParentCategory)
                .Include(t => t.User)
                .SingleOrDefaultAsync(m => m.TopicId == id);

            if (Topic == null)
            {
                return NotFound();
            }
            var categories = _context.Categories
                .Where(c => c.ParentCategory != null)
                .Where(c => c.ParentCategory == Topic.Category.ParentCategory);
            ViewData["CategoryFK"] = new SelectList(categories, "CategoryId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Topic.ModificationDate = DateTime.Now;

            _context.Attach(Topic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TopicExists(Topic.TopicId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { categoryId = Topic.CategoryFK});
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.TopicId == id);
        }
    }
}

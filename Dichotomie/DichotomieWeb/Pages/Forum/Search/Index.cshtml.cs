using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DichotomieWeb.Pages.Forum.Search
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Topic> Topics { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<SelectListItem> CategorySLI { get; set; }

        public async Task OnGetAsync(string searchString, int searchCategory)
        {
            Topics = await _context.Topics.ToListAsync();
            Categories = await _context.Categories.ToListAsync();
            CategorySLI = new List<SelectListItem>();

            IList<Category> MainCategory;

            MainCategory = Categories.Where(x => x.ParentCategory != null).ToList();

            CategorySLI.Add(new SelectListItem { Value = "0", Text = "" }); // Default SelectItem
            foreach (Category category in MainCategory)
            {
                CategorySLI.Add(new SelectListItem { Value = category.CategoryId.ToString(), Text = category.Name });
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchCategory != 0)
                    Topics = Topics.Where(x => x.Title.Contains(searchString) && x.CategoryFK == searchCategory).ToList();
                else
                    Topics = Topics.Where(x => x.Title.Contains(searchString)).ToList();
            }
        }
    }
}
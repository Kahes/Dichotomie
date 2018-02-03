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

        public async Task OnGetAsync(string searchString, int searchCategory, string searchFirstDate, string searchSecondDate)
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

            if (!String.IsNullOrEmpty(searchString) || searchCategory != 0 || searchFirstDate != null || searchSecondDate != null)
            {
                if (!String.IsNullOrEmpty(searchString))
                    Topics = Topics.Where(x => x.Title.Contains(searchString)).ToList();
                if (searchCategory != 0)
                    Topics = Topics.Where(x => x.CategoryFK == searchCategory).ToList();
                if (searchFirstDate != null && searchSecondDate == null)
                {
                    string[] Datetmp = searchFirstDate.Split('-'); 
                    DateTime FirstDate = new DateTime(Int32.Parse(Datetmp[0]), Int32.Parse(Datetmp[1]), Int32.Parse(Datetmp[2]));
                    Topics = Topics.Where(x => x.CreationDate >= FirstDate).ToList();
                }
                else if (searchFirstDate == null && searchSecondDate != null)
                {
                    string[] Datetmp = searchSecondDate.Split('-');
                    DateTime SecondDate = new DateTime(Int32.Parse(Datetmp[0]), Int32.Parse(Datetmp[1]), Int32.Parse(Datetmp[2]));
                    Topics = Topics.Where(x => x.CreationDate <= SecondDate).ToList();
                }
                else
                {
                    string[] Datetmp = searchFirstDate.Split('-');
                    string[] Datetmp1 = searchSecondDate.Split('-');
                    DateTime FirstDate = new DateTime(Int32.Parse(Datetmp[0]), Int32.Parse(Datetmp[1]), Int32.Parse(Datetmp[2]));
                    DateTime SecondDate = new DateTime(Int32.Parse(Datetmp1[0]), Int32.Parse(Datetmp1[1]), Int32.Parse(Datetmp1[2]));
                    Topics = Topics.Where(x => x.CreationDate >= FirstDate && x.CreationDate <= SecondDate).ToList();
                }
            }
        }
    }
}
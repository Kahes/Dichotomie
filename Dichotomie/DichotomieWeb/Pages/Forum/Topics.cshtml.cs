using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dichotomie.Models;
using DichotomieWeb.Data;

namespace DichotomieWeb.Pages.Forum
{
    public class TopicsModel : PageModel
    {
        private ApplicationDbContext _context;
        public List<Topic> _topics;

        public TopicsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int categoryId)
        {
            var category = _context.Categories.SingleOrDefault(s => s.CategoryId == categoryId);
            _topics = _context.Topics.Where(w => w.Category == category).ToList();
        }
    }
}
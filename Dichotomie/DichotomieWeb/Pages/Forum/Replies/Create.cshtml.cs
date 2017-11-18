using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Dichotomie.Models;
using DichotomieWeb.Data;

namespace DichotomieWeb.Pages.Forum.Replies
{
    public class CreateModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public CreateModel(DichotomieWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["TopicFK"] = new SelectList(_context.Topic, "TopicId", "MainContent");
        ViewData["UserFK"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Reply Reply { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Reply.Add(Reply);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
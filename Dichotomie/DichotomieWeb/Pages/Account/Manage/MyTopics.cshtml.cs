using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DichotomieWeb.Pages.Account.Manage
{
    public class MyTopicsModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyTopicsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Topic> Topics { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Topics = await _context.Topics
                .Include(t => t.Replies)
                    .ThenInclude(r => r.User)
                .OrderByDescending(t => t.CreationDate)
                .Where(x => x.UserFK == user.Id)
                .ToListAsync();
        }
    }
}
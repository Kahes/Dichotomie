using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dichotomie.Models;
using DichotomieWeb.Data;
using DichotomieWeb.Models;
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
        public IList<Reputation> Reputations { get; set; }
        public float averagenote { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Topics = await _context.Topics
                .Include(t => t.Replies)
                    .ThenInclude(r => r.User)
                .OrderByDescending(t => t.CreationDate)
                .Where(x => x.UserFK == user.Id)
                .ToListAsync();

            List<Reputation> Rep = new List<Reputation>();
            foreach (Topic topic in Topics)
            {
                Reputations = await _context.Reputations
                .Include(u => u.User)
                .Where(t => t.TopicFK == topic.TopicId)
                .ToListAsync();

                foreach (Reputation rep in Reputations)
                {
                    Rep.Add(rep);
                }
            }

            if (Reputations.Count > 0)
            {
                averagenote = 0;
                foreach (Reputation rep in Reputations)
                {
                    averagenote += rep.MarkValue;
                }
                averagenote = averagenote / Reputations.Count;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using DichotomieWeb.Models;
using Microsoft.AspNetCore.Identity;

namespace DichotomieWeb.Pages.Forum.Replies
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int TopicId { get; set; }
        public IList<Reply> Replies { get;set; }
        public IList<Topic> topic { get; set; }
        public IList<Reputation> Reputations { get; set; }
        public IList<SelectListItem> ReputationList { get; set; }
        public bool Voted { get; set; }
        public float averagenote { get; set; }

        public async Task OnGetAsync(int topicId, string votevalue)
        {
            TopicId = topicId;
            ReputationList = new List<SelectListItem>();
            Replies = await _context.Replies
                .Include(r => r.Topic)
                .Include(r => r.User)
                .Where(r => r.Topic.TopicId == topicId)
                .OrderBy(r => r.CreationDate)
                .ToListAsync();

            topic = await _context.Topics
                .Where(t => t.TopicId == TopicId)
                .ToListAsync();

            Reputations = await _context.Reputations
                .Include(u => u.User)
                .Where(t => t.TopicFK == TopicId && t.UserFK == _userManager.GetUserId(User))
                .ToListAsync();

            if (Reputations.Count > 0)
            {
                averagenote = 0;
                foreach (Reputation rep in Reputations)
                {
                    averagenote += rep.MarkValue;
                }
                averagenote = averagenote / Reputations.Count;
            }

            ReputationList.Add(new SelectListItem { Value = "no", Text = "" });
            for (int i = 0; i <= 5; i++)
            {
                ReputationList.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }

            if (votevalue == "no" || votevalue != null)
            {
                Reputation rep = new Reputation
                {
                    User = await _userManager.GetUserAsync(User),
                    MarkValue = Int32.Parse(votevalue),
                    Topic = topic.FirstOrDefault(),
                };
                _context.Reputations.Add(rep);
                _context.SaveChanges();
            }
        }
    }
}

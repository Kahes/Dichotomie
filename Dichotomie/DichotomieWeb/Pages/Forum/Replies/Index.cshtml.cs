using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Dichotomie.Models;
using DichotomieWeb.Data;

namespace DichotomieWeb.Pages.Forum.Replies
{
    public class IndexModel : PageModel
    {
        private readonly DichotomieWeb.Data.ApplicationDbContext _context;

        public IndexModel(DichotomieWeb.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public int TopicId { get; set; }
        public IList<Reply> Replies { get;set; }

        public async Task OnGetAsync(int topicId)
        {
            TopicId = topicId;
            Replies = await _context.Replies
                .Include(r => r.Topic)
                .Include(r => r.User)
                .Where(r => r.Topic.TopicId == topicId)
                .OrderByDescending(r => r.CreationDate)
                .ToListAsync();
        }
    }
}

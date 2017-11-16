using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using DichotomieWeb.Data;
using Microsoft.EntityFrameworkCore;

namespace DichotomieWeb.Pages
{
    [Authorize]
    public class ForumModel : PageModel
    {
        public ApplicationDbContext _context;

        public ForumModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
    }
}
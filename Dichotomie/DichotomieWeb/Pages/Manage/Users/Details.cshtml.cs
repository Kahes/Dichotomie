using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DichotomieWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace DichotomieWeb.Pages.Manage.Users
{
    [Authorize(Roles = "Admin, Moderator")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public ApplicationUser ApplicationUser { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Date)]
            public DateTime LockoutDate { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser = await _userManager.FindByIdAsync(id);
            if (ApplicationUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(ApplicationUser.Id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.LockoutEnd != null)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, Input.LockoutDate);
            }

            return RedirectToPage("./Details", new { id = ApplicationUser.Id });
        }
    }
}

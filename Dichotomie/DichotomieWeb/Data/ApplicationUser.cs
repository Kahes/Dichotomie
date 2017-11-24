using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Dichotomie.Models;

namespace DichotomieWeb.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public virtual List<Topic> Topics { get; set; }
        public virtual List<Reply> Replies { get; set; }
    }
}

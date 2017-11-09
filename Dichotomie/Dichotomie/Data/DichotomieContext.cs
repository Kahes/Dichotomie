using Dichotomie.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Dichotomie.Data
{
    public class DichotomieContext : DbContext
    {
        public DichotomieContext(DbContextOptions<DichotomieContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Reply> Replies { get; set; }
    }
}

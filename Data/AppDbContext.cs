using Microsoft.EntityFrameworkCore;
using MyWebApp.Models; // Ensure this directive is present

namespace MyWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}

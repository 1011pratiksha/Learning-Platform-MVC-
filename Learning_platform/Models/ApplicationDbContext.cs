using Microsoft.EntityFrameworkCore;

namespace Learning_platform.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SubCourse> SubCourses { get; set; }
        public DbSet<MasterCourse> MasterCourses { get; set; }
    }
}
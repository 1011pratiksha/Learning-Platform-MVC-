using Microsoft.EntityFrameworkCore;
using LearningPlatform.Models;

namespace LearningPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<MasterCourse> MasterCourses { get; set; }
        public DbSet<SubCourse> SubCourses { get; set; }

        // Topic table.
        public DbSet<Topic> Topics { get; set; }


        // Material table.
        public DbSet<Material> Materials { get; set; }


        // MCQ table.
        public DbSet<Mcq> Mcqs { get; set; }
    }
}

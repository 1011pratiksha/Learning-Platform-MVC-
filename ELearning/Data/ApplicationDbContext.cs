using ELearning.Models;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MasterCourse> MasterCourses { get; set; }
        public DbSet<SubCourse> SubCourses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<AdminSubscription> AdminSubscriptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MyCourses> MyCourses { get; set; }
        public DbSet<Mcq> Mcqs { get; set; }
        public DbSet<TopicProgress> TopicProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MyCourses composite primary key
            modelBuilder.Entity<MyCourses>()
                .HasKey(x => new { x.Sid, x.UserId });

            // SubCourse -> MasterCourse
            modelBuilder.Entity<SubCourse>()
                .HasOne<MasterCourse>()
                .WithMany()
                .HasForeignKey(x => x.Mid)
                .OnDelete(DeleteBehavior.Restrict);

            // Topic -> MasterCourse
            modelBuilder.Entity<Topic>()
                .HasOne<MasterCourse>()
                .WithMany()
                .HasForeignKey(x => x.Mid)
                .OnDelete(DeleteBehavior.Restrict);

            // Topic -> SubCourse
            modelBuilder.Entity<Topic>()
                .HasOne<SubCourse>()
                .WithMany()
                .HasForeignKey(x => x.Sid)
                .OnDelete(DeleteBehavior.Restrict);

            // Material -> MasterCourse
            modelBuilder.Entity<Material>()
                .HasOne(x => x.MasterCourse)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material -> SubCourse
            modelBuilder.Entity<Material>()
                .HasOne(x => x.SubCourse)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material -> Topic
            modelBuilder.Entity<Material>()
                .HasOne(x => x.Topic)
                .WithMany()
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            // AdminSubscription -> MasterCourse
            modelBuilder.Entity<AdminSubscription>()
                .HasOne<MasterCourse>()
                .WithMany()
                .HasForeignKey(x => x.Mid)
                .OnDelete(DeleteBehavior.Restrict);

            // AdminSubscription -> SubCourse
            modelBuilder.Entity<AdminSubscription>()
                .HasOne<SubCourse>()
                .WithMany()
                .HasForeignKey(x => x.Sid)
                .OnDelete(DeleteBehavior.Restrict);

            // MyCourses -> SubCourse
            modelBuilder.Entity<MyCourses>()
                .HasOne<SubCourse>()
                .WithMany()
                .HasForeignKey(x => x.Sid)
                .OnDelete(DeleteBehavior.Restrict);

            // MyCourses -> User
            modelBuilder.Entity<MyCourses>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal precision
            modelBuilder.Entity<SubCourse>()
                .Property(x => x.Samount)
                .HasPrecision(9, 2);

            modelBuilder.Entity<AdminSubscription>()
                .Property(x => x.SubAmount)
                .HasPrecision(9, 2);

            // Mcq -> Material
            modelBuilder.Entity<Mcq>()
                .HasOne(x => x.Material)
                .WithMany(x => x.Mcqs)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
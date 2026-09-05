using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace LearningAppMVC.Models
{
    public class SubscriptionContectcs : DbContext
    {
        public DbSet<Subscriptions> Subscriptions { get;set;  }
        public DbSet<MasterCourse> MasterCourses { get; set; }

        public DbSet<SubCourse> SubCourses { get; set; }
        public DbSet<SubscriptionSubCourse> SubscriptionSubCourses { get; set; }
    }
}
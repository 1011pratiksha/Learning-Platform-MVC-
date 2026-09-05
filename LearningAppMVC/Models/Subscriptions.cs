using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningAppMVC.Models
{
    public class Subscriptions
    {
        [Key]
        public int sub_id { get; set; }
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string sub_type { get; set; }

        public int mid { get; set; }

        public double sub_amount { get; set; }

        public string subStatus { get; set; }

        public string subThumbnail { get; set; }

        public virtual MasterCourse MasterCourse { get; set; }

        public virtual ICollection<SubscriptionSubCourse> SubscriptionSubCourses { get; set; }
    }
}
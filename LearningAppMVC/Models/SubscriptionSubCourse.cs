using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningAppMVC.Models
{
    public class SubscriptionSubCourse
    {
        [Key]
        public int id { get; set; }

        public int sub_id { get; set; }

        public int sid { get; set; }

        public virtual Subscriptions Subscription { get; set; }

        public virtual SubCourse SubCourse { get; set; }
    }
}
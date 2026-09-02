using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LearningAppMVC.Models
{
    public class Subscriptions
    {
        public int sub_id { get; set; }
        public string sub_type { get; set; }
        public int mid { get; set; }
        public int sid { get; set; }
        public double sub_amount { get; set; }
        public string subStatus { get; set; }
        public string subThumbnail { get; set; }

    }
}
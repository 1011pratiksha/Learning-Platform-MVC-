using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningAppMVC.Models
{
    public class SubCourse
    {
        [Key]
        public int sid { get; set; }
        public int mid { get; set; }
        public string sname { get; set; }
        public string sstatus { get; set; }
    }
}
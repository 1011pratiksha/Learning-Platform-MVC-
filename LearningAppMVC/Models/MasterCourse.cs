using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LearningAppMVC.Models
{

    public class MasterCourse
    {
        [Key]
        public int mid { get; set; }

        public string mname { get; set; }

        public string mstatus { get; set; }
    }
}
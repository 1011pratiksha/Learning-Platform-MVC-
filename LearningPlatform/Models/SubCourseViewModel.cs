using System;

namespace LearningPlatform.Models
{
    public class SubCourseViewModel
    {
        public int Id { get; set; }
        public string SubCourseName { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string MasterCourseName { get; set; }
    }
}
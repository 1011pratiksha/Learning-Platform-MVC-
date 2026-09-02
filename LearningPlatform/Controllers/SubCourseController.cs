using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;

namespace LearningPlatform.Controllers
{
    public class SubCourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var allSubCourses = _context.SubCourses
                .Select(s => new SubCourseViewModel
                {
                    Id = s.Id,
                    SubCourseName = s.SubCourseName,
                    Amount = s.Amount,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    CreatedBy = s.CreatedBy,
                    MasterCourseName = s.MasterCourse.CourseName
                })
                .OrderByDescending(s => s.CreatedAt)
                .ToList();

            ViewBag.MasterCourseList = new SelectList(
                _context.MasterCourses.Where(m => m.Status == true).ToList(),
                "Id",
                "CourseName"
            );

            return View(allSubCourses);
        }

        [HttpPost]
        public IActionResult Add(SubCourse model)
        {
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "Admin";

            _context.SubCourses.Add(model);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public IActionResult GetById(int id)
        {
            var subCourse = _context.SubCourses.Find(id);
            return Json(subCourse);
        }

        [HttpPost]
        public IActionResult Update(SubCourse model)
        {
            var subCourse = _context.SubCourses.Find(model.Id);

            if (subCourse == null)
            {
                return Json(new { success = false });
            }

            subCourse.MasterCourseId = model.MasterCourseId;
            subCourse.SubCourseName = model.SubCourseName;
            subCourse.Amount = model.Amount;
            subCourse.Status = model.Status;

            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var subCourse = _context.SubCourses.Find(id);

            if (subCourse == null)
            {
                return Json(new { success = false });
            }

            _context.SubCourses.Remove(subCourse);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
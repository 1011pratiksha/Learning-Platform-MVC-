using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace LearningPlatform.Controllers
{
    public class MasterCourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MasterCourseController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            var allCourses = _context.MasterCourses.OrderByDescending(c => c.CreatedAt).ToList();
            return View(allCourses);
        }

        [HttpPost]
        public IActionResult Add(MasterCourse model, IFormFile thumbnail)
        {
            if (thumbnail != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
                string folderPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    thumbnail.CopyTo(stream);
                }

                model.ThumbnailPath = "/uploads/" + fileName;
            }

            model.CreatedAt = DateTime.Now;
            model.CreatedBy = "Admin";

            _context.MasterCourses.Add(model);
            _context.SaveChanges();

            return Json(new { success = true });
        }

        public IActionResult GetById(int id)
        {
            var course = _context.MasterCourses.Find(id);
            return Json(course);
        }

        [HttpPost]
        public IActionResult Update(MasterCourse model, IFormFile thumbnail)
        {
            var course = _context.MasterCourses.Find(model.Id);

            if (course == null)
            {
                return Json(new { success = false });
            }

            course.CourseName = model.CourseName;
            course.Status = model.Status;

            if (thumbnail != null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
                string folderPath = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    thumbnail.CopyTo(stream);
                }

                course.ThumbnailPath = "/uploads/" + fileName;
            }

            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var course = _context.MasterCourses.Find(id);

            if (course == null)
            {
                return Json(new { success = false });
            }

            _context.MasterCourses.Remove(course);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
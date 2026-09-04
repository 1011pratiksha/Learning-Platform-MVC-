using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace LearningPlatform.Controllers
{
    public class TopicController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        // ApplicationDbContext is used to communicate with the database.
        // IWebHostEnvironment is used to access wwwroot for thumbnail upload.
        public TopicController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        
        // INDEX
        
        // Displays all topics in the Topic page
        // Master Course and Sub Course names are loaded using
        // navigation properties
        public IActionResult Index(string search)
        {
            var topics = _context.Topics
                .Include(t => t.MasterCourse)
                .Include(t => t.SubCourse)
                .AsQueryable();

            // If Admin enters something in Search,
            // filter the topics by Topic Name
            if (!string.IsNullOrEmpty(search))
            {
                topics = topics.Where(t => t.TopicName.Contains(search));
            }

            // Latest topics are displayed first
            var allTopics = topics
                .OrderByDescending(t => t.Id)
                .ToList();


            // Master Course dropdown
            // Only active Master Courses should be shown
            ViewBag.MasterCourseList = new SelectList(
                _context.MasterCourses
                    .Where(m => m.Status == true)
                    .ToList(),
                "Id",
                "CourseName"
            );

            return View(allTopics);
        }


        
        // ADD TOPIC
        //
        // Receives data from the Add Topic form.
        //
        // The thumbnail is received separately as IFormFile because
        // an uploaded file is not stored directly inside the Topic object
        [HttpPost]
        public IActionResult Add(Topic model, IFormFile thumbnail)
        {
            // Thumbnail upload
            if (thumbnail != null)
            {
                // Generate a unique file name so two uploaded files
                // do not accidentally have the same name
                string fileName = Guid.NewGuid() +
                                  Path.GetExtension(thumbnail.FileName);

                // Physical folder where the uploaded file will be stored
                string folderPath = Path.Combine(
                    _env.WebRootPath,
                    "uploads"
                );

                // Create the folder if it does not already exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Complete physical path of the uploaded file
                string fullPath = Path.Combine(
                    folderPath,
                    fileName
                );

                // Create the file and copy the uploaded file into it
                using (var stream = new FileStream(
                    fullPath,
                    FileMode.Create))
                {
                    thumbnail.CopyTo(stream);
                }

                // Store only the relative URL/path in the database
                // Example:
                // /uploads/image.jpg
                model.ThumbnailPath = "/uploads/" + fileName;
            }

            // Add the Topic to database
            _context.Topics.Add(model);

            // SaveChanges() executes the INSERT in SQL Server
            _context.SaveChanges();

            return Json(new { success = true });
        }


        
        // GET TOPIC BY ID
        // 
        // Used when Admin clicks Edit
        // Returns the selected Topic as JSON so JavaScript can
        // fill the Edit form
        public IActionResult GetById(int id)
        {
            var topic = _context.Topics.Find(id);

            return Json(topic);
        }


        
        // GET SUB COURSES
        
        // Used for the dependent dropdown
        //
        // Example:
        // Admin selects "Java" as Master Course
        // This method returns only the Sub Courses belonging to Java
        [HttpGet]
        public IActionResult GetSubCourses(int masterCourseId)
        {
            var subCourses = _context.SubCourses
                .Where(s =>
                    s.MasterCourseId == masterCourseId &&
                    s.Status == true)
                .Select(s => new
                {
                    id = s.Id,
                    name = s.SubCourseName
                })
                .ToList();

            return Json(subCourses);
        }


        //
        // UPDATE TOPIC
        // Receives updated Topic information from the Edit form
        [HttpPost]
        public IActionResult Update(Topic model, IFormFile thumbnail)
        {
            // Find the existing Topic using its primary key
            var topic = _context.Topics.Find(model.Id);

            if (topic == null)
            {
                return Json(new { success = false });
            }


            // Update the Master Course
            topic.MasterCourseId = model.MasterCourseId;

            // Update the Sub Course
            topic.SubCourseId = model.SubCourseId;

            // Update Topic Name
            topic.TopicName = model.TopicName;

            // Update Video URL
            topic.VideoUrl = model.VideoUrl;

            // Update Status
            topic.Status = model.Status;


            // 
            // UPDATE THUMBNAIL
            
            // Only replace the old thumbnail when Admin selects a
            // new file. Otherwise, the previous thumbnail remains.
            if (thumbnail != null)
            {
                string fileName = Guid.NewGuid() +
                                  Path.GetExtension(thumbnail.FileName);

                string folderPath = Path.Combine(
                    _env.WebRootPath,
                    "uploads"
                );

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath = Path.Combine(
                    folderPath,
                    fileName
                );

                using (var stream = new FileStream(
                    fullPath,
                    FileMode.Create))
                {
                    thumbnail.CopyTo(stream);
                }

                topic.ThumbnailPath = "/uploads/" + fileName;
            }

            // Save all modified values.
            _context.SaveChanges();

            return Json(new { success = true });
        }


        
        // DELETE TOPIC
        // 
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var topic = _context.Topics.Find(id);

            if (topic == null)
            {
                return Json(new { success = false });
            }


            // 
            // CHECK RELATED MATERIALS
            // 
            // A Topic can have one or more Materials
            //
            // Since MCQs now belong to Material, we only need to check
            // whether Materials exist for this Topic
            //
            // If a Material exists, its MCQs are associated through
            // that Material
            //
            // Therefore, we don't allow the Topic to be deleted until
            // its related Materials are handled
            if (_context.Materials.Any(m => m.TopicId == id))
            {
                return Json(new
                {
                    success = false,
                    message = "This Topic has Material records. Delete the related Material first."
                });
            }


            // Remove the Topic when no related Material exists
            _context.Topics.Remove(topic);

            // Execute DELETE operation in SQL Server
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
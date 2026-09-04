using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LearningPlatform.Controllers
{
    public class MaterialController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        // ApplicationDbContext is used to work with the database.
        // IWebHostEnvironment is used to access wwwroot for
        // uploading assignment files
        public MaterialController(
            ApplicationDbContext context,
            IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        // INDEX 

        // Displays all Materials on the Material page.
        //
        // The page require:
        // Master course name
        // Sub course name
        // Topic name
        // Assignment
        // MCQ count
        //
        // These are loaded using EF Core navigation properties.


        // -- Index --
        // Displays all Materials and also loads the active
        // Master Courses for the filter/add-material dropdown.
        public IActionResult Index()
        {
            var materials = _context.Materials
                .Include(m => m.MasterCourse)
                .Include(m => m.SubCourse)
                .Include(m => m.Topic)
                .Include(m => m.Mcqs)
                .OrderByDescending(m => m.Id)
                .ToList();


            // Load active Master Courses for the dropdown
            //
            // "Id" is the value submitted to the controller
            // "CourseName" is the text displayed to Admin
            ViewBag.MasterCourseList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.MasterCourses
                    .Where(m => m.Status == true)
                    .ToList(),
                "Id",
                "CourseName"
            );


            return View(materials);
        }
        

        // GET SUB COURSES
        
        // Used by the Master Course -> Sub Course dependent dropdown.
        //
        // Example:
        // Admin selects:
        // Master Course = Java
        //
        // This method returns only the active Sub Courses
        // belonging to Java.
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


        
        // GET TOPICS
       
        // Used by the Sub Course -> Topic dependent dropdown
        //
        // Admin first selects Master Course,
        // then Sub Course
        //
        // We then return only the active Topics belonging
        // to the selected Sub Course
        [HttpGet]
        public IActionResult GetTopics(int subCourseId)
        {
            var topics = _context.Topics
                .Where(t =>
                    t.SubCourseId == subCourseId &&
                    t.Status == true)
                .Select(t => new
                {
                    id = t.Id,
                    name = t.TopicName
                })
                .ToList();

            return Json(topics);
        }


        
        // ADD MATERIAL
        
        // Saves:
        //
        // 1] Material record
        // 2] Assignment file path
        // 3] Multiple MCQ records
        //
        // The MCQs come from the Add Material form.
        [HttpPost]
        public IActionResult Add(
            Material model,
            IFormFile assignmentFile,
            List<Mcq> mcqs)
        {
            
            // ASSIGNMENT FILE UPLOAD
            // 
            if (assignmentFile != null)
            {
                // Generate a unique name so uploaded files
                // don't overwrite each other
                string fileName =
                    Guid.NewGuid() +
                    Path.GetExtension(assignmentFile.FileName);

                // Store assignment files inside:
                // wwwroot/uploads/assignments
                string folderPath = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "assignments"
                );

                // Create the folder if it doesn't exist
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Complete physical path of the file.
                string fullPath = Path.Combine(
                    folderPath,
                    fileName
                );

                // Copy uploaded file to the server.
                using (var stream = new FileStream(
                    fullPath,
                    FileMode.Create))
                {
                    assignmentFile.CopyTo(stream);
                }

                // Store only the relative path in the database.
                //
                // Example:
                // /uploads/assignments/abc123.pdf
                model.Assignment =
                    "/uploads/assignments/" + fileName;
            }


            
            // SAVE MATERIAL
            
            // First save the Material because we need its generated
            // Material ID for the MCQ records.
            _context.Materials.Add(model);

            _context.SaveChanges();


            
            // SAVE MCQs
            
            // One Material can contain multiple MCQs.
            //
            // After Material is saved:
            // model.Id contains the generated material_id
            if (mcqs != null && mcqs.Count > 0)
            {
                foreach (var mcq in mcqs)
                {
                    // Connect this question to the newly created
                    // Material
                    mcq.MaterialId = model.Id;

                    _context.Mcqs.Add(mcq);
                }

                // Save all MCQs together
                _context.SaveChanges();
            }


            return Json(new { success = true });
        }


        
        // GET MATERIAL BY ID
        
        // Used when Admin clicks Edit
        //
        // We return the Material together with its MCQs so that
        // JavaScript can populate the edit modal
        public IActionResult GetById(int id)
        {
            var material = _context.Materials
                .Include(m => m.Mcqs)
                .FirstOrDefault(m => m.Id == id);

            if (material == null)
            {
                return Json(new { success = false });
            }


            // Return only the values needed by the Edit modal
            var result = new
            {
                id = material.Id,
                masterCourseId = material.MasterCourseId,
                subCourseId = material.SubCourseId,
                topicId = material.TopicId,
                assignment = material.Assignment,

                mcqs = material.Mcqs.Select(m => new
                {
                    id = m.Id,
                    question = m.Question,
                    option1 = m.Option1,
                    option2 = m.Option2,
                    option3 = m.Option3,
                    option4 = m.Option4,
                    answer = m.Answer
                }).ToList()
            };

            return Json(result);
        }


        
        // UPDATE MATERIAL
        
        // Updates:
        //
        // 1. Material information
        // 2. Assignment file if a new file is selected
        // 3. Existing MCQs
        // 4. New MCQs
        //
        // For simplicity, the existing MCQs are removed and the
        // submitted MCQ list is inserted again
        //
        // This works well for an Admin form where the complete
        // question list is submitted together
        [HttpPost]
        public IActionResult Update(
            Material model,
            IFormFile assignmentFile,
            List<Mcq> mcqs)
        {
            var material = _context.Materials
                .Include(m => m.Mcqs)
                .FirstOrDefault(m => m.Id == model.Id);

            if (material == null)
            {
                return Json(new { success = false });
            }


            
            // UPDATE BASIC MATERIAL INFORMATION
            
            material.MasterCourseId =
                model.MasterCourseId;

            material.SubCourseId =
                model.SubCourseId;

            material.TopicId =
                model.TopicId;


            
            // UPDATE ASSIGNMENT
            
            // If no new file is selected, keep the existing file.
            if (assignmentFile != null)
            {
                string fileName =
                    Guid.NewGuid() +
                    Path.GetExtension(assignmentFile.FileName);

                string folderPath = Path.Combine(
                    _env.WebRootPath,
                    "uploads",
                    "assignments"
                );

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fullPath =
                    Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(
                    fullPath,
                    FileMode.Create))
                {
                    assignmentFile.CopyTo(stream);
                }

                material.Assignment =
                    "/uploads/assignments/" + fileName;
            }


            
            // UPDATE Mcqs
            // 
            // Remove the existing questions first.
            if (material.Mcqs != null &&
                material.Mcqs.Count > 0)
            {
                _context.Mcqs.RemoveRange(material.Mcqs);
            }


            // Add the current MCQs submitted by Admin
            if (mcqs != null && mcqs.Count > 0)
            {
                foreach (var mcq in mcqs)
                {
                    // Connect each question to this Material
                    mcq.MaterialId = material.Id;

                    _context.Mcqs.Add(mcq);
                }
            }


            // Save material + Mcq changes
            _context.SaveChanges();

            return Json(new { success = true });
        }


        
        // DELETE MATERIAL
        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var material = _context.Materials
                .Include(m => m.Mcqs)
                .FirstOrDefault(m => m.Id == id);

            if (material == null)
            {
                return Json(new { success = false });
            }


            
            // DELETE RELATED MCQs FIRST
            
            // Our database relationship uses cascade delete for:
            //
            // Material -> Mcq
            //
            // Removing the Material therefore removes its MCQs.
            //
            // We still explicitly remove them here because it makes
            // the controller behavior easier to understand and
            // keeps the operation clear.
            if (material.Mcqs != null &&
                material.Mcqs.Count > 0)
            {
                _context.Mcqs.RemoveRange(material.Mcqs);
            }


            // Remove Material.
            _context.Materials.Remove(material);

            // Execute DELETE operations.
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}

using ELearning.Data;
using ELearning.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ELearning.Controllers
{
    public class MyCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyCoursesController(ApplicationDbContext context)
        {
            _context = context;
        }


        // MY COURSES
        public async Task<IActionResult> Index(int userId = 1)
        {
            var courses = await _context.MyCourses
                .Where(mc => mc.UserId == userId)
                .Join(
                    _context.SubCourses,
                    mc => mc.Sid,
                    sc => sc.Sid,
                    (mc, sc) => new { mc, sc }
                )
                .Join(
                    _context.MasterCourses,
                    x => x.sc.Mid,
                    master => master.Mid,
                    (x, master) => new MyCourseViewModel
                    {
                        Sid = x.sc.Sid,
                        UserId = x.mc.UserId,
                        Sname = x.sc.Sname,
                        Sstatus = x.sc.Sstatus,
                        Samount = x.sc.Samount,
                        Mname = master.Mname,
                        Mthumbnail = master.Mthumbnail
                    }
                )
                .ToListAsync();

            return View(courses);
        }


        // WATCH VIDEO
        public async Task<IActionResult> WatchVideo(
            int sid,
            int userId = 1,
            int? tid = null)
        {

            // GET COURSE
            var course = await _context.SubCourses
                .Where(sc => sc.Sid == sid)
                .Join(
                    _context.MasterCourses,
                    sc => sc.Mid,
                    mc => mc.Mid,
                    (sc, mc) => new
                    {
                        sc.Sid,
                        sc.Sname,
                        mc.Mname,
                        mc.Mthumbnail
                    }
                )
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }


            // GET ALL TOPICS
            var topics = await _context.Topics
                .Where(t => t.Sid == sid)
                .OrderBy(t => t.Tid)
                .ToListAsync();

            if (topics.Count == 0)
            {
                return NotFound("No topics found for this course.");
            }


            // GET USER TOPIC PROGRESS
            var progress = await _context.TopicProgress
                .Where(p =>
                    p.UserId == userId &&
                    p.Sid == sid)
                .ToListAsync();


            // CREATE TOPIC STATUS
            var topicItems = new List<TopicItemViewModel>();

            for (int i = 0; i < topics.Count; i++)
            {
                var topic = topics[i];

                var currentProgress = progress
                    .FirstOrDefault(p => p.Tid == topic.Tid);


                // Check whether current topic is completed
                bool isCompleted =
                    currentProgress != null &&
                    currentProgress.McqPassed;


                bool isUnlocked;


                // FIRST TOPIC
                if (i == 0)
                {
                    // Topic 1 is always unlocked
                    isUnlocked = true;
                }
                else
                {
                    // OTHER TOPICS
                    // Previous topic must have 3/3 MCQ passed
                    var previousTopic = topics[i - 1];

                    var previousProgress = progress
                        .FirstOrDefault(p =>
                            p.Tid == previousTopic.Tid);

                    isUnlocked =
                        previousProgress != null &&
                        previousProgress.McqPassed;
                }


                topicItems.Add(
                    new TopicItemViewModel
                    {
                        Topic = topic,
                        IsUnlocked = isUnlocked,
                        IsCompleted = isCompleted
                    }
                );
            }


            // SELECT CURRENT TOPIC
            int currentTopicId;


            if (tid.HasValue)
            {
                var selectedTopic = topicItems
                    .FirstOrDefault(x =>
                        x.Topic.Tid == tid.Value);


                // PREVENT ACCESS TO LOCKED TOPIC
                if (selectedTopic == null ||
                    !selectedTopic.IsUnlocked)
                {
                    return RedirectToAction(
                        "WatchVideo",
                        new
                        {
                            sid = sid,
                            userId = userId
                        });
                }


                currentTopicId = tid.Value;
            }
            else
            {
                // OPEN FIRST UNLOCKED TOPIC
                currentTopicId = topicItems
                    .First(x => x.IsUnlocked)
                    .Topic
                    .Tid;
            }


            // GET MATERIAL FOR CURRENT TOPIC
            var material = await _context.Materials
                .Where(m =>
                    m.SubCourseId == sid &&
                    m.TopicId == currentTopicId)
                .FirstOrDefaultAsync();


            // GET MCQs FOR CURRENT TOPIC
            var mcqs = new List<Mcq>();

            if (material != null)
            {
                mcqs = await _context.Mcqs
                    .Where(m => m.MaterialId == material.Id)
                    .OrderBy(m => m.Id)
                    .Take(3)
                    .ToListAsync();
            }


            // CHECK CERTIFICATE
            bool certificateUnlocked =
                topicItems.All(x => x.IsCompleted);


            // CREATE VIEW MODEL
            var model = new WatchVideoViewModel
            {
                Sid = course.Sid,

                UserId = userId,

                CurrentTopicId = currentTopicId,

                Sname = course.Sname,

                Mname = course.Mname,

                Mthumbnail = course.Mthumbnail,

                TopicItems = topicItems,

                Mcqs = mcqs,

                CertificateUnlocked = certificateUnlocked
            };


            return View(model);
        }


        // DOWNLOAD ASSIGNMENT
        public async Task<IActionResult> DownloadAssignment(int sid)
        {
            var material = await _context.Materials
                .Where(m => m.SubCourseId == sid)
                .FirstOrDefaultAsync();

            if (material == null ||
                string.IsNullOrEmpty(material.Assignment))
            {
                return NotFound("Assignment not found.");
            }


            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                material.Assignment.TrimStart('/')
            );


            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Assignment file not found.");
            }


            var fileBytes =
                await System.IO.File.ReadAllBytesAsync(filePath);


            return File(
                fileBytes,
                "application/pdf",
                Path.GetFileName(filePath)
            );
        }


        // SUBMIT ASSIGNMENT
        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(
            int sid,
            IFormFile assignmentFile)
        {
            if (assignmentFile == null ||
                assignmentFile.Length == 0)
            {
                TempData["Message"] =
                    "Please select an assignment file.";

                return RedirectToAction(
                    "WatchVideo",
                    new
                    {
                        sid = sid
                    });
            }


            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "submissions"
            );


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            var fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(
                    assignmentFile.FileName
                );


            var filePath =
                Path.Combine(
                    folderPath,
                    fileName
                );


            using (var stream =
                   new FileStream(
                       filePath,
                       FileMode.Create))
            {
                await assignmentFile.CopyToAsync(stream);
            }


            TempData["Message"] =
                "Assignment submitted successfully.";


            return RedirectToAction(
                "WatchVideo",
                new
                {
                    sid = sid
                });
        }


        // SUBMIT MCQ
        [HttpPost]
        public async Task<IActionResult> SubmitMcq(
            int sid,
            int userId,
            int tid,
            List<string> answers)
        {
            // CHECK TOPIC
            var topic = await _context.Topics
                .FirstOrDefaultAsync(t =>
                    t.Tid == tid &&
                    t.Sid == sid);


            if (topic == null)
            {
                return NotFound("Topic not found.");
            }


            // GET ALL TOPICS
            var topics = await _context.Topics
                .Where(t => t.Sid == sid)
                .OrderBy(t => t.Tid)
                .ToListAsync();


            int topicIndex =
                topics.FindIndex(
                    t => t.Tid == tid
                );


            if (topicIndex == -1)
            {
                return NotFound();
            }


            // CHECK PREVIOUS TOPIC
            if (topicIndex > 0)
            {
                int previousTopicId =
                    topics[topicIndex - 1].Tid;


                bool previousPassed =
                    await _context.TopicProgress
                        .AnyAsync(p =>
                            p.UserId == userId &&
                            p.Sid == sid &&
                            p.Tid == previousTopicId &&
                            p.McqPassed);


                // USER CANNOT SUBMIT LOCKED TOPIC
                if (!previousPassed)
                {
                    return RedirectToAction(
                        "WatchVideo",
                        new
                        {
                            sid = sid,
                            userId = userId
                        });
                }
            }


            // GET MATERIAL FOR CURRENT TOPIC
            var material = await _context.Materials
                .Where(m =>
                    m.SubCourseId == sid &&
                    m.TopicId == tid)
                .FirstOrDefaultAsync();


            if (material == null)
            {
                return NotFound(
                    "Material not found for this topic."
                );
            }


            // GET MCQs
            var mcqs = await _context.Mcqs
                .Where(m =>
                    m.MaterialId == material.Id)
                .OrderBy(m => m.Id)
                .Take(3)
                .ToListAsync();


            if (mcqs.Count == 0)
            {
                TempData["McqResult"] =
                    "No MCQ questions found.";


                return RedirectToAction(
                    "WatchVideo",
                    new
                    {
                        sid = sid,
                        userId = userId,
                        tid = tid
                    });
            }


            // CALCULATE SCORE
            int score = 0;


            for (int i = 0;
                 i < mcqs.Count;
                 i++)
            {
                if (answers != null &&
                    i < answers.Count)
                {
                    if (string.Equals(
                        answers[i]?.Trim(),
                        mcqs[i].Answer?.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        score++;
                    }
                }
            }


            // STORE SCORE
            TempData["McqScore"] =
                score;

            TempData["McqTotal"] =
                mcqs.Count;


            // PASS ONLY WITH 3/3
            if (score == 3 &&
                mcqs.Count == 3)
            {
                TempData["McqResult"] =
                    "Congratulations! You scored 3/3. Next topic is unlocked.";


                TempData["McqPassed"] =
                    true;


                // GET EXISTING PROGRESS
                var existingProgress =
                    await _context.TopicProgress
                        .FirstOrDefaultAsync(p =>
                            p.UserId == userId &&
                            p.Sid == sid &&
                            p.Tid == tid);


                // CREATE PROGRESS
                if (existingProgress == null)
                {
                    var topicProgress =
                        new TopicProgress
                        {
                            UserId = userId,

                            Sid = sid,

                            Tid = tid,

                            McqPassed = true,

                            CompletedAt =
                                DateTime.Now
                        };


                    _context.TopicProgress.Add(
                        topicProgress
                    );
                }
                else
                {
                    // UPDATE EXISTING PROGRESS
                    existingProgress.McqPassed =
                        true;

                    existingProgress.CompletedAt =
                        DateTime.Now;
                }


                await _context.SaveChangesAsync();
            }
            else
            {
                // FAILED TEST
                TempData["McqResult"] =
                    "You scored "
                    + score
                    + "/3. You must score 3/3 to unlock the next topic.";

                TempData["McqPassed"] =
                    false;
            }


            // RETURN TO CURRENT TOPIC
            return RedirectToAction(
                "WatchVideo",
                new
                {
                    sid = sid,
                    userId = userId,
                    tid = tid
                });
        }
       
public async Task<IActionResult> Certificate(int sid, int userId)
        {
            // Get user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Get course and sub-course
            var course = await _context.SubCourses
                .Where(sc => sc.Sid == sid)
                .Join(
                    _context.MasterCourses,
                    sc => sc.Mid,
                    mc => mc.Mid,
                    (sc, mc) => new
                    {
                        sc.Sid,
                        sc.Sname,
                        mc.Mname
                    }
                )
                .FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound("Course not found.");
            }

            // Get all topics
            var topics = await _context.Topics
                .Where(t => t.Sid == sid)
                .ToListAsync();

            // Check whether all topics are completed
            var completedTopics = await _context.TopicProgress
                .Where(p =>
                    p.UserId == userId &&
                    p.Sid == sid &&
                    p.McqPassed)
                .CountAsync();

            if (topics.Count == 0 || completedTopics < topics.Count)
            {
                return Forbid();
            }

            var model = new CertificateViewModel
            {
                UserName = user.UserName,
                CourseName = course.Mname,
                SubCourseName = course.Sname,
                CompletionDate = DateTime.Now
            };

            return PartialView("_Certificate", model);
        }


    }
}


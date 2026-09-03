    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Learning_platform.Models;
    using Razorpay.Api;


    namespace Learning_platform.Controllers
    {
        public class UserController : Controller
        {
            private readonly ApplicationDbContext _context;

            public UserController(ApplicationDbContext context)
            {
                _context = context;
            }
            public IActionResult Dashboard()
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                return View();
            }
            public IActionResult Profile()
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                return View(user);
            }
            [HttpGet]
            public IActionResult EditProfile()
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var model = new EditProfile
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserEmail = user.UserEmail
                };

                return View(model);
            }

            [HttpPost]
            public IActionResult EditProfile(EditProfile model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = _context.Users.FirstOrDefault(u => u.UserId == model.UserId);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                user.UserName = model.UserName;
                user.UserEmail = model.UserEmail;

                _context.SaveChanges();

                return RedirectToAction("Profile");
            }
            public IActionResult AllCourses()
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var courses = _context.SubCourses
                    .Include(c => c.MasterCourse)
                    .ToList();

                var masterCourses = _context.MasterCourses.ToList();

                var cart = HttpContext.Session.GetString("Cart");

                int cartCount = 0;

                if (!string.IsNullOrEmpty(cart))
                {
                    cartCount = cart.Split(',').Length;
                }

                ViewBag.CartCount = cartCount;
                ViewBag.MasterCourses = masterCourses;

                return View(courses);
        
            }
            public IActionResult SubCourses(int mid)
            {
                var courses = _context.SubCourses
                    .Include(c => c.MasterCourse)
                    .Where(c => c.Mid == mid)
                    .ToList();

                return View(courses);
            }


            [HttpPost]
            public IActionResult AddToCart(int sid)
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var cart = HttpContext.Session.GetString("Cart");

                if (string.IsNullOrEmpty(cart))
                {
                    cart = sid.ToString();
                }
                else
                {
                    var cartItems = cart.Split(',').ToList();

                    if (!cartItems.Contains(sid.ToString()))
                    {
                        cartItems.Add(sid.ToString());
                    }

                    cart = string.Join(",", cartItems);
                }

                HttpContext.Session.SetString("Cart", cart);

                return RedirectToAction("AllCourses");
            }
            public IActionResult Cart()
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var cart = HttpContext.Session.GetString("Cart");

                if (string.IsNullOrEmpty(cart))
                {
                    return View(new List<SubCourse>());
                }

                var sidList = cart
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                var courses = _context.SubCourses
                    .Include(c => c.MasterCourse)
                    .Where(c => sidList.Contains(c.Sid))
                    .ToList();

                return View(courses);
            }
            [HttpPost]
            public IActionResult CreateOrder()
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var cart = HttpContext.Session.GetString("Cart");

                if (string.IsNullOrEmpty(cart))
                {
                    return RedirectToAction("Cart");
                }

                var sidList = cart
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();

                var courses = _context.SubCourses
                    .Where(c => sidList.Contains(c.Sid))
                    .ToList();

                decimal totalAmount = courses.Sum(c => c.SAmount);

                string keyId = "rzp_test_Kl7588Yie2yJTV";
                string keySecret = "6dN9Nqs7M6HPFMlL45AhaTgp";

                RazorpayClient razorpayClient = new RazorpayClient(keyId, keySecret);

                Dictionary<string, object> options = new Dictionary<string, object>();

                options.Add("amount", (int)(totalAmount * 100));
                options.Add("currency", "INR");
                options.Add("receipt", "order_" + DateTime.Now.Ticks);
                options.Add("payment_capture", 1);

                Razorpay.Api.Order order = razorpayClient.Order.Create(options);

                string orderId = order["id"].ToString();

                ViewBag.KeyId = keyId;
                ViewBag.OrderId = orderId;
                ViewBag.Amount = (int)(totalAmount * 100);

                return View("Payment");
            }
        }
    }

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LearningAppMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string name,string amount )
        {
            if (name == "")
            {
                ModelState.AddModelError("Name", "Please Entre Subscription Name");
            }
            if (amount == "")
            {
                ModelState.AddModelError("amount", "Please Entre Amount");
            }
            if(ModelState.IsValid == true)
            {
                ViewData["SussessMessage"] = "<script> alert('Form Submited')</script>";
                ModelState.Clear();
            }
            return View();
        }
    }
}
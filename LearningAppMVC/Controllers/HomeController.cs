using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LearningAppMVC.Models;

namespace LearningAppMVC.Controllers
{
    public class HomeController : Controller
    {

        SubscriptionContectcs db = new SubscriptionContectcs();
        // GET: Grid
        public ActionResult Index()
        {
            var data = db.Subscriptions.ToList();
            return View(data);
        }
        public ActionResult Modal()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Modal(Subscriptions s)
        {
            if (ModelState.IsValid == true)
            {
                db.Subscriptions.Add(s);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    // TempData["InsertMessage"] = "<script>alert('Data Inserted')</script>";
                    TempData["InsertMessage"] = "Data Inserted";
                    //ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
            return View(s);
        }

        public ActionResult Edit(int id)
        {
            var row = db.Subscriptions.Where(model => model.sub_id == id).FirstOrDefault();
            return View(row);
        }
        [HttpPost]
        public ActionResult Edit(Subscriptions s)
        {
            if (ModelState.IsValid == true)
            {
                db.Entry(s).State = EntityState.Modified;
                int a = db.SaveChanges();
                if (a > 0)
                {
                    //ViewBag.UpdateMessage = "<script>alert('Data Updated')</script>";
                    TempData["UpdatetMessage"] = "Data Updated";
                    //ModelState.Clear();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            var SubIdRow = db.Subscriptions.Where(model => model.sub_id == id).FirstOrDefault();
            return View(SubIdRow);
        }
        [HttpPost]
        public ActionResult Delete(Subscriptions s)
        {
            if (ModelState.IsValid == true)
            {
                db.Entry(s).State = EntityState.Deleted;
                int a = db.SaveChanges();
                if (a > 0)
                {
                    //ViewBag.DeleteMessage = "<script>alert('Data Deleted')</script>";
                    TempData["DeleteMessage"] = "Data Deleted";
                    //ModelState.Clear();
                }
                return RedirectToAction("Index");
            }
            return View();
        }  
    }
}
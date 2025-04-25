using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using JobMedia.Models;

namespace JobMedia.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Login page.";

            return View();
        }

        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    model.IsAdmin = false;
                    model.IsDeleted = false;
                    db.User.Add(model);

                    // Log the SQL being executed
                    db.Database.Log = message => System.Diagnostics.Debug.WriteLine(message);

                    int rows = db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Rows affected: " + rows);

                    // Verify the model state after save
                    System.Diagnostics.Debug.WriteLine("User ID after save: " + model.Id);

                    TempData["Success"] = "Signup successful!";
                    return RedirectToAction("Login", "Home");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Save error: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while saving.");
                }
            }

            return View(model);
        }

    }
}
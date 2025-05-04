using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using JobMedia.Helpers;
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.Message = "Login page.";

            return View();
        }

        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDto login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = db.User.FirstOrDefault(u => u.Email == login.Email && !u.IsDeleted);
            if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                string role = "Employer";

                if (user.IsAdmin)
                    role = "Admin";
                else if (user.IsRecruiter)
                    role = "Recruiter";
                else if (user.IsEmployer)
                    role = "Employer";

                // Use FormsAuthentication instead of JWT for built-in auth
                FormsAuthentication.SetAuthCookie(user.Email, false);

                TempData["Success"] = "Login successful!";

                if (role == "Recruiter")
                    return RedirectToAction("Recruiter", "Recruiter");
                else if (role == "Admin")
                    return RedirectToAction("Admin", "Admin");

                return RedirectToAction("Employer", "Employer");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(login);

        }

        //public ActionResult Logout()
        //{
        //    Session.Clear(); 
        //    return RedirectToAction("Login", "Home");
        //}

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); 
            return RedirectToAction("Login", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

                    var user = db.User.FirstOrDefault(u => u.Email == model.Email && !u.IsDeleted);

                    if (user != null)
                    {
                        System.Diagnostics.Debug.WriteLine("User mail already registered ");
                        TempData["Error"] = "This email is already registered. Please log in or use a different email.";
                        return View(model); 
                    }

                    db.User.Add(model);
                    db.SaveChanges();

                    TempData["Success"] = "Signup successful! Please log in.";
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
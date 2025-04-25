using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
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
        public ActionResult Login()
        {
            ViewBag.Message = "Login page.";

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]

        //public ActionResult Login(LoginDto login)
        //{
        //    ViewBag.Message = "Login page.";

        //    return RedirectToAction("Recruiter", "Recruiter");
        //    //return View(login);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDto login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = db.User.FirstOrDefault(u => u.Email == login.Email && !u.IsDeleted);

            if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                string role="Employer";

                if (user.IsAdmin)
                    role = "Admin";
                else if (user.IsRecruiter)
                    role = "Recruiter";
                else if (user.IsEmployer)
                    role = "Employer";
                string token = JwtHelper.GenerateToken(user.Email, role);

                System.Diagnostics.Debug.WriteLine("Generated token: " + token);


                Response.Cookies.Add(new HttpCookie("jwtToken")
                {
                    Value = token,
                    Expires = DateTime.Now.AddHours(1),
                    HttpOnly = true
                });

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

        public ActionResult Logout()
        {
            Session.Clear(); 
            return RedirectToAction("Login", "Home");
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

                    
                    db.Database.Log = message => System.Diagnostics.Debug.WriteLine(message);

                    int rows = db.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("Rows affected: " + rows);

                   
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
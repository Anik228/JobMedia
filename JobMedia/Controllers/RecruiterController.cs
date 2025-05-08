using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobMedia.Models;

namespace JobMedia.Controllers
{
    [Authorize]
    public class RecruiterController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Recruiter()
        {
            return View();
        }

        public ActionResult RecruiterPostForm()
        {
            return PartialView("RecruiterPostForm");
        }

        public ActionResult AllJobs()
        {
            return PartialView("AllJobs");
        }

        public ActionResult Profile()
        {
            string userEmail = User.Identity.Name;

          
            var user = db.User.FirstOrDefault(u => u.Email == userEmail && !u.IsDeleted);

            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }

           
            var profile = new User
            {
                Name = user.Name,
                Email = user.Email,
                Gender = user.Gender,
              
            };

        

            return PartialView(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile(UpdateProfile updatedUser)
        {
            if (!ModelState.IsValid)
            {
               
                foreach (var key in ModelState.Keys)
                {
                    var error = ModelState[key].Errors.FirstOrDefault();
                    if (error != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }
                // return View("Profile", updatedUser);
                return RedirectToAction("Recruiter", "Recruiter");
            }

            var existingUser = db.User.FirstOrDefault(u => u.Email == updatedUser.Email);
            if (existingUser == null)
            {
                return HttpNotFound();
            }

            existingUser.Name = updatedUser.Name;
            //existingUser.Gender = updatedUser.Gender;          

            int affectedRows = db.SaveChanges();

            if (affectedRows > 0)
            {
                TempData["Success"] = "Profile updated successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to update profile. Please try again.";
            }


            // return PartialView(profile);

            return RedirectToAction("Recruiter", "Recruiter");
        }
    }
}
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobMedia.Controllers
{
    [Authorize]
    public class RecruiterController : Controller
    {
        
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
            return PartialView("Profile");
        }
    }
}
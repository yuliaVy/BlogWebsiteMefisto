using Initilal_YV_Assesment2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Initilal_YV_Assesment2.Controllers
{
    public class HomeController : Controller
    {
        //create an instance of the database 
        private MefistoDbContext context = new MefistoDbContext();
        public ActionResult Index()
        {
            // 1. Fetch the latest 3 posts, ordered by newest first
            var latestPosts = context.Posts
                .OrderByDescending(p => p.DatePosted)
                .Take(3)
                .ToList();

            // 2. Create the ViewModel
            var model = new HomeViewModel
            {
                LatestPosts = latestPosts,
                Login = new LoginViewModel() // Initialize empty login form
            };

            return View(model);
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
    }
}
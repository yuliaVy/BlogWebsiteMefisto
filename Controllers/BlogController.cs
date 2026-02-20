using Initilal_YV_Assesment2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Initilal_YV_Assesment2.Controllers
{
    public class BlogController : Controller
    {
        //create an instance of the database 
        private MefistoDbContext context = new MefistoDbContext();
        //An Index page in Blog Returns the list of posts alongside list of categories
        //when user clicks on specialized button, they can preview detailed post page with comments left by users
        // GET: Blog
        public ActionResult Index(int? categoryId)
        {
            //get categories from the database
            var categories = context.Categories.ToList();
            //get posts from the db, with user who created they and category
            var posts = context.Posts.Include(p => p.User).Include("Category").AsQueryable();
            //if category been selected, then only display posts from selected category
            if (categoryId != null)
            {
                posts = posts.Where(p => p.CategoryId == categoryId);
            }
            //pass categories and posts to the view model
            var vm = new BlogViewModel
            {
                Categories = categories,
                Posts = posts.OrderByDescending(p => p.DatePosted).ToList()
            };
            //return the view
            return View(vm);
        }

        public ActionResult Details(int id)
        {
            bool isSuspended = false;
            //check for user suspension
            if (User.Identity.IsAuthenticated)
            {
                //get current logged-in user Id
                var userId = User.Identity.GetUserId();
                //find it in the databse
                var loggedInuser = context.Users.Find(userId);

                if (loggedInuser.SuspendedUntil.HasValue && loggedInuser.SuspendedUntil.Value > DateTime.Now)
                {
                    isSuspended = true;
                    //Pass the date so we can show it in the alert
                    ViewBag.SuspendedUntil = loggedInuser.SuspendedUntil.Value.ToShortDateString();
                }
            }
            ViewBag.IsSuspended = isSuspended;

            //search the Post Table in the db
            //find post by Id
            //return post
            var post = context.Posts.Find(id);

            //using the foreign key UserId from the post instance
            //find the user who created the post
            var user = context.Users.Find(post.UserId);

            //using the foreign key CategoryId from the post instance
            //find the category that the post belongs to
            var category = context.Categories.Find(post.CategoryId);

            var comments = context.Comments.Include(p => p.User).Where(p => p.Post.PostId == id).ToList();

            //create a new View Model which will store user details and posts created by this user
            var vm = new BlogDetailsViewModel
            {
                Post = post,
                Category = category,
                Comments = comments
            };

            //send the post model to the Details view
            return View(vm);
        }


        //add comment method must ensure the user is logged in first, and then diplay post information
        [Authorize] //forces the login check automatically
        public ActionResult AddComment(int postId)
        {
            // 1.Fetch the post so we can show its Title and Content on the page
            var post = context.Posts.Find(postId);

            if (post == null)
            {
                return HttpNotFound();
            }

            // 2. Pass the Post object to the View using ViewBag
            ViewBag.Post = post;

            // 3. Return the view (The Model will be a new empty Comment)
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AddComment([Bind(Include = "CommentId, CommentText, PostId, UserId")] Comment comment, int postId)
        {
            // another Security Check
            var userId = User.Identity.GetUserId();
            var user = context.Users.Find(userId);

            if (user.SuspendedUntil.HasValue && user.SuspendedUntil.Value > DateTime.Now)
            {
                // If they try to hack the form, just stop them.
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, "Your account is suspended.");
            }

            //if the post passed as a parametr to the edid action is not null then post will be updated in the database
            if (ModelState.IsValid)
            {
                //record the new date when the change was made
                comment.DatePosted = DateTime.Now;
                //get the id of the user that is logged in the system and assign it as a foreign key in the post
                comment.UserId = User.Identity.GetUserId();

                comment.PostId = postId;
                //add the new post to the db
                context.Comments.Add(comment);
                //save the changes to the database
                context.SaveChanges();
                // 4. Redirect back to the "Details" page so they can see their new comment
                return RedirectToAction("Details", new { id = postId });
            }
            
            // If there was an error (like empty content), show the form again
            // We need to keep the PostId so the form knows where to submit next time
            ViewBag.Post = context.Posts.Find(postId);
            return View(comment);
        }
    }
}
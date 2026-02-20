using Initilal_YV_Assesment2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Initilal_YV_Assesment2.Controllers
{
    [Authorize(Roles = "Staff, Admin")] // Only Staff can see this
    public class StaffController : Controller
    {
        //create an instance of the database 
        private MefistoDbContext context = new MefistoDbContext();

        // GET: Staff/Dashboard
        public ActionResult Dashboard(int? categoryId)
        {
            var currentUserId = User.Identity.GetUserId();
            //get categories from the database
            var categories = context.Categories.ToList();
            //get posts from the db, with user who created they and category
            var posts = context.Posts.Include(p => p.User).Include("Category").Where(p => p.UserId == currentUserId).AsQueryable();
            //if category been selected, then only display posts from selected category
            if (categoryId != null)
            {
                posts = posts.Where(p => p.CategoryId == categoryId);
            }
            //pass categories and posts to the view model
            var vm = new BlogViewModel
            {
                Categories = categories,
                Posts = posts.OrderByDescending(p => p.DateEdited).ToList()
            };
            //return the view
            return View(vm);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            // Fetch categories for the dropdown list
            // "CategoryId" is the value saved, "Name" is the text shown
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name");

            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Content,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                // fill the system fields
                post.UserId = User.Identity.GetUserId(); // The logged-in Staff/Admin
                post.DatePosted = DateTime.Now;
                post.DateEdited = DateTime.Now;

                // Save to DB
                context.Posts.Add(post);
                context.SaveChanges();

                // Redirect to Dashboard to see the new post
                return RedirectToAction("Dashboard");
            }

            // If validation fails (e.g. empty fields), reload the categories so dropdown still works
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }

        public ActionResult Details(int id)
        {
            var post = context.Posts
                .Include(p => p.User)
                .Include(p => p.Category)
                .FirstOrDefault(p => p.PostId == id);

            var comments = context.Comments
                .Include(c => c.User)
                .Where(c => c.PostId == id)
                .OrderByDescending(c => c.DatePosted)
                .ToList();

            var viewModel = new BlogDetailsViewModel
            {
                Post = post,
                Category = post.Category,
                Comments = comments
            };

            return View(viewModel);
        }

        // GET: Member/Edit/5
        //this mothod returns the edit form to the browser 
        //together with an instance of a post, so the user can make changes
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            //find post by Id in the post table
            Post post = context.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            //get a list of all the categories from Categories table
            //and send the list to the view using a viewBag
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);

            //also send the post to the Edit View
            //where user can change the details of the post
            return View(post);
        }

        // POST: Member/Edit/5
        //this method gets the edited details of a post and have to update them in the Db
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Content,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                // Find existing post from database
                var postToEdit = context.Posts.Find(post.PostId);

                if (postToEdit == null)
                {
                    return HttpNotFound();
                }

                // Update only editable fields
                postToEdit.Title = post.Title;
                postToEdit.Content = post.Content;
                postToEdit.CategoryId = post.CategoryId;

                // Update metadata
                postToEdit.DateEdited = DateTime.Now;

                context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            //othervise, if the post parameter IS null, then we send the list of categories back to the edif form
            //to prevent crashing
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);

            //return the post to the edit form
            return View(post);
        }

        [HttpPost] //this is important
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            //find post by id in Posts table
            Post post = context.Posts.Find(id);

            //remove post from Posts table
            context.Posts.Remove(post);

            //save changes in the database
            context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id, int postId)
        {
            var comment = context.Comments.Find(id);

            if (comment != null)
            {
                context.Comments.Remove(comment);
                context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = postId });
        }


    }
}
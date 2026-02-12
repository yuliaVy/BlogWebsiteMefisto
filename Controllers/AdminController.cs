using Initilal_YV_Assesment2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Initilal_YV_Assesment2.Controllers
{
    [Authorize(Roles = "Admin")] //this will only allow registered Admins to access the AdminController
    public class AdminController : AccountController
    {
        //create an instance of the database 
        private MefistoDbContext context = new MefistoDbContext();

        // GET: Admin
        [Authorize(Roles = "Admin")] //only Admins can call the index action
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")] //only Admins can call the index action
        //GET:Users
        //this method returns all users with member status 
        public ActionResult ViewAllUsers()
        {
            //find out roleId of "member"
            var roleId = context.Roles.FirstOrDefault(r => r.Name == "Member")?.Id;

            // Filter users who belong to that role ID
            var usersInRole = context.Users
                .Where(u => u.Roles.Any(r => r.RoleId == roleId))
                .ToList();

            return View(usersInRole);
        }

        //GET: User(member)/Details
        //this method returns a paticular member details alongside comments, which this user has left
        public ActionResult UserDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //find a commemnts left by user in comments table
            var comments = context.Comments
                .Where(c => c.UserId.Equals(id))
                .ToList();
            //find member in Members table
            var member = context.Users.Find(id);

            //checking if user suspension is expired
            if (member.IsSuspended && member.SuspendedUntil < DateTime.Now)
            {
                member.IsSuspended = false;
                member.SuspendedUntil = null;
                context.SaveChanges();
            }

            //create a new View Model which will store user details and comment left by this user
            var vm = new MemberDetailsViewModel
            {
                User = member,
                Comments = comments
            };

            //if post doesn't exist then return a not found error message
            if (member == null)
            {
                return HttpNotFound();
            }

            //othervise send the member details and comments to the details view
            return View(vm);
        }

        //method which suspends a user
        [HttpPost]
        public ActionResult SuspendUser(string userId)
        {
            //find a member in the database
            var member = context.Users.Find(userId);

            //if member doesn't exist - display error
            if (member == null)
                return HttpNotFound();

            //change suspended status to true
            member.IsSuspended = true;
            //set up the date when suspension ends
            member.SuspendedUntil = DateTime.Now.AddDays(30);

            //save changes
            context.SaveChanges();
            //return the same view but with updated status
            return RedirectToAction("UserDetails", new { id = userId });
        }

        [HttpPost]
        // Ensure only Admins can promote users
        [Authorize(Roles = "Admin")]
        public ActionResult PromoteToStaff(string userId)
        {
            var user = context.Users.Find(userId);

            if (user == null)
                return HttpNotFound();

            // Remove Member role
            UserManager.RemoveFromRole(userId, "Member");

            // Add Staff role
            UserManager.AddToRole(userId, "Staff");

            // complete new fields
            user.BecomeEmployee = DateTime.Now;

            var validationErrors = context.GetValidationErrors();

            context.SaveChanges();
           
            return RedirectToAction("ViewAllUsers");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Ensure only Admins can delete user comments
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComment(int id)
        {
            // find the comment in the db
            var comment = context.Comments.Find(id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            // store the UserID so we know where to go back to!
            var userId = comment.UserId;

            // remove the comment
            context.Comments.Remove(comment);
            context.SaveChanges();

            // 4. Redirect back to the specific User Details page
            return RedirectToAction("UserDetails", new { id = userId });
        }

        //method which returns list of employees
        [Authorize(Roles = "Admin")] //only Admins can call the index action
        //GET:Users
        //this method returns all users with member status 
        public ActionResult ViewStaff()
        {
            //find out roleId of "member"
            var roleId = context.Roles.FirstOrDefault(r => r.Name == "Staff")?.Id;

            //Filter users who belong to that role ID
            var usersInRole = context.Users
                .Where(u => u.Roles.Any(r => r.RoleId == roleId))
                .ToList();

            return View(usersInRole);
        }

        //GET: User(staff)/Details
        //this method returns a paticular member details alongside comments, which this user has left
        [Authorize(Roles = "Admin")]
        public ActionResult StaffDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //find posts created by user in posts table
            var posts = context.Posts
                .Where(p => p.UserId.Equals(id)).Include(p => p.Category)
                .ToList();
            //find staff in Members table
            var staff = context.Users.Find(id);

            //create a new View Model which will store user details and posts created by this user
            var vm = new StaffDetailsViewModel
            {
                User = staff,
                Posts = posts
            };

            //if staff doesn't exist then return a not found error message
            if (staff == null)
            {
                return HttpNotFound();
            }

            //othervise send the staff details and posts to the details view
            return View(vm);
        }

        //method which promote staff member to an admin role
        [HttpPost]
        // Ensure only Admins can promote users
        [Authorize(Roles = "Admin")]
        public ActionResult PromoteToAdmin(string userId)
        {
            var user = context.Users.Find(userId);

            if (user == null)
                return HttpNotFound();

            // Remove Member role
            UserManager.RemoveFromRole(userId, "Staff");

            // Add to the Admin role
            UserManager.AddToRole(userId, "Admin");

            context.SaveChanges();

            return RedirectToAction("ViewStaff");
        }

        //method which deletes a post
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Ensure only Admins can delete user comments
        [Authorize(Roles = "Admin")]
        public ActionResult DeletePost(int id)
        {
            // find the comment in the db
            var post = context.Posts.Find(id);

            if (post == null)
            {
                return HttpNotFound();
            }

            // store the UserID so we know where to go back to!
            var userId = post.UserId;

            // remove the comment
            context.Posts.Remove(post);
            context.SaveChanges();

            // 4. Redirect back to the specific User Details page
            return RedirectToAction("StaffDetails", new { id = userId });
        }


        //method which returns list of categories
        [Authorize(Roles = "Admin")] //only Admins can call the index action
        //GET:Categories
        public ActionResult ViewAllCategories()
        {
            //return the ViewAllCategoriew view that displays a list of categories
            return View(context.Categories.ToList());
        }


        //method which allows admin to create a new category
        // GET: Categories/Create
        public ActionResult CreateCategory()
        {
            return View();
        }
        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "CategoryId, Name")] Category cat)
        {
            if (ModelState.IsValid)
            {
                context.Categories.Add(cat);
                context.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }
            return View(cat);

        }

        //method which allows admin to Edit a category
        // GET: Categories/Edit/5
        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Category cat = context.Categories.Find(id);
            if (cat == null)
            {
                return HttpNotFound();
            }
            return View(cat);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "CategoryId, Name")] Category cat)
        {
            if (ModelState.IsValid)
            {
                context.Entry(cat).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("ViewAllCategories");
            }
            return View(cat);
        }

        //method to deletes an existing category
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Ensure only Admins can delete user comments
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteCategory(int id)
        {
            //find a category in the caegory table by id
            var category = context.Categories.Find(id);
 
            //if category doesn't exist then return a not found error message
            if (category == null)
            {
                return HttpNotFound();
            }
            // remove the comment
            context.Categories.Remove(category);
            context.SaveChanges();

            //and display the values stored in the properties
            return RedirectToAction("ViewAllCategories");
        }

    }
}

using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Initilal_YV_Assesment2.Models
{
    public class HomeViewModel
    {
        // Holds the list of announcements from the DB
        public List<Post> LatestPosts { get; set; }

        // Holds the login data
        public LoginViewModel Login { get; set; }
    }

    public class BlogViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Post> Posts { get; set; }
    }

    //view model which can display details of current post
    public class BlogDetailsViewModel
    {
        public Post Post { get; set; }
        public Category Category { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
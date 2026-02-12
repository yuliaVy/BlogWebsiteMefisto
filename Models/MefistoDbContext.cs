using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Initilal_YV_Assesment2.Models
{
    public class MefistoDbContext : IdentityDbContext<User>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories{ get; set; }

        public MefistoDbContext()
            : base("MefistoConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public static MefistoDbContext Create()
        {
            return new MefistoDbContext();
        }
    }
}
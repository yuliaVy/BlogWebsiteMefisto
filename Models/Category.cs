using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Initilal_YV_Assesment2.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string Name { get; set; }

        //navigational property
        public List<Post> Posts { get; set; }
    }
}
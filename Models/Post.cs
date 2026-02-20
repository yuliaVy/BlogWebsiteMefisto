using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Initilal_YV_Assesment2.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name = "Date Posted")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")] //formats as a Short DateTime
        public DateTime DatePosted { get; set; }

        [Display(Name = "Date Edited")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:d}")] //formats as a Short DateTime
        public DateTime? DateEdited { get; set; }


        //Navigational property
        [ForeignKey("Category")]
        [Required(ErrorMessage = "Category is required!")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //navigational property
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
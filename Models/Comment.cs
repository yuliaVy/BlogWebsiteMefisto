using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Initilal_YV_Assesment2.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        
        [Required(ErrorMessage = "Comment field can't be blank!")]
        [DataType(DataType.MultilineText)]
        public string CommentText { get; set; }

        [Display(Name = "Date Posted")]
        [DataType(DataType.DateTime)]
        public DateTime DatePosted { get; set; }

        //navigational property

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
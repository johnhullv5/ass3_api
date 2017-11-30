using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _300858525_Jiang__ASS.Models
{
    // [START book]
    [Bind(Include = "Title, Director, PublishedDate, Description, ImageUrl")]
    public class Movie
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Director { get; set; }

        [Display(Name = "Date Published")]
        [DataType(DataType.Date)]
        public DateTime? PublishedDate { get; set; }

        public string ImageUrl { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string CreatedById { get; set; }

        public List<Comments> comments { get; set; }

        public bool addComment(String content)
        {
            
            Comments a = new Comments();
            a.Author = "Hao";
            a.content = content;
            comments.Add(a);
            return true;
        }
    }
}

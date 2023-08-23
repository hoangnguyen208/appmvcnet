using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace appmvcnet.Models.Blog
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public int PostId { set; get; }

        [Required]
        public string Title { set; get; }

        public string Description { set; get; }

        public string Slug { set; get; }

        public string Content { set; get; }

        public bool Published { set; get; }

        public List<PostCategory> PostCategories { get; set; }

        public string AuthorId { set; get; }

        [ForeignKey("AuthorId")]
        public AppUser Author { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateUpdated { set; get; }
    }
}
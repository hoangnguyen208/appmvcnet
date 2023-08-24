using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace appmvcnet.Models.Product
{
    [Table("Product")]
    public class ProductModel
    {
        [Key]
        public int ProductID { set; get; }

        [Required]
        public string Title { set; get; }

        public string Description { set; get; }

        public string Slug { set; get; }

        public string Content { set; get; }

        public bool Published { set; get; }

        public string AuthorId { set; get; }

        [ForeignKey("AuthorId")]
        public AppUser Author { set; get; }

        public DateTime DateCreated { set; get; }

        public DateTime DateUpdated { set; get; }

        public double Price { set; get; }

        public List<ProductCategoryProduct> ProductCategoryProducts { get; set; }

        public List<ProductPhoto> Photos { get; set; }
    }
}
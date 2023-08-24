using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace appmvcnet.Models.Product
{
    [Table("CategoryProduct")]
    public class CategoryProduct
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        public string Slug { set; get; }

        public ICollection<CategoryProduct> CategoryChildren { get; set; }

        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public CategoryProduct ParentCategory { set; get; }

        public void ChildCategoryIDs(ICollection<CategoryProduct> childcates, List<int> lists)
        {
            if (childcates == null)
                childcates = this.CategoryChildren;

            foreach (CategoryProduct category in childcates)
            {
                lists.Add(category.Id);
                ChildCategoryIDs(category.CategoryChildren, lists);

            }
        }

        public List<CategoryProduct> ListParents()
        {
            List<CategoryProduct> li = new List<CategoryProduct>();
            var parent = this.ParentCategory;
            while (parent != null)
            {
                li.Add(parent);
                parent = parent.ParentCategory;

            }
            li.Reverse();
            return li;
        }

    }
}
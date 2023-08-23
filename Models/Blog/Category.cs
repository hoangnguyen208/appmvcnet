using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace appmvcnet.Models.Blog
{
    [Table("Category")]
    public class Category
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        public string Description { get; set; }

        [Required]
        public string Slug { set; get; }

        public ICollection<Category> CategoryChildren { get; set; }

        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public Category ParentCategory { set; get; }


        public void ChildCategoryIDs(ICollection<Category> childcates, List<int> lists)
        {
            if (childcates == null)
                childcates = this.CategoryChildren;

            foreach (Category category in childcates)
            {
                lists.Add(category.Id);
                ChildCategoryIDs(category.CategoryChildren, lists);

            }
        }

        public List<Category> ListParents()
        {
            List<Category> li = new List<Category>();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Models.Blog;
using Microsoft.AspNetCore.Mvc;

namespace appmvcnet.Views.Shared.Components.CategorySidebar
{
    [ViewComponent]
    public class CategorySidebar : ViewComponent
    {
        public class CategorySidebarData
        {
            public List<Category> Categories { get; set; }
            public int Level { get; set; }
            public string CategorySlug { get; set; }
        }

        public IViewComponentResult Invoke(CategorySidebarData data)
        {
            return View(data);
        }
    }
}
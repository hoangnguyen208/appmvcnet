using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Models.Blog;

namespace appmvcnet.Areas.Blog.Models
{
    public class CreatePostModel : Post {
        public int[] CategoryIDs { get; set; }
    }
}
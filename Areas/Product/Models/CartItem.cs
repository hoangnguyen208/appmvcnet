using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Models.Product;

namespace appmvcnet.Areas.Product.Models
{
    public class CartItem
    {
        public int quantity { set; get; }
        public ProductModel product { set; get; }
    }
}
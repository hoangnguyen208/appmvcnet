using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Models.Product;

namespace appmvcnet.Areas.Product.Models
{
    public class CreateProductModel : ProductModel {
        public int[] CategoryIDs { get; set; }
    }
}
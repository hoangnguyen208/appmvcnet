using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Models;

namespace appmvcnet.Services
{
    public class ProductService : List<Product>
    {
        public ProductService()
        {
            this.AddRange(new Product[] {
                new Product() { Id = 1, Name = "Iphone", Price = 1000 },
                new Product() { Id = 2, Name = "Samsung", Price = 800 }
            });
        }
    }
}
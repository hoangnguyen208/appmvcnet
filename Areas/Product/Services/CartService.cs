using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appmvcnet.Areas.Product.Models;
using Newtonsoft.Json;

namespace appmvcnet.Areas.Product.Services
{
    public class CartService
    {
        public const string CARTKEY = "cart";

        private readonly IHttpContextAccessor _context;

        private readonly HttpContext HttpContext;

        public CartService(IHttpContextAccessor context)
        {
            _context = context;
            HttpContext = context.HttpContext;
        }


        public List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        public void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        public void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

    }
}
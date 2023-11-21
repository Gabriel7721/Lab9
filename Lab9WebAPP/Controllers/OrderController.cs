using Lab9WebAPP.Helper;
using Microsoft.AspNetCore.Mvc;
using Lab9Lib;
using Newtonsoft.Json;


namespace Lab9WebAPP.Controllers
{
    public class OrderController : Controller
    {
        private readonly string url = "https://localhost:7250/api/Products";
        private HttpClient httpClient = new HttpClient();
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectJson<List<Oder>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.subtotal = cart!.Sum(item => item.Product!.Price * item.Quantity);
            return View();
        }
        public int checkProduct(int id)
        {
            var cart = SessionHelper.GetObjectJson<List<Oder>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart!.Count; i++)
            {
                if (cart[i].Product!.ProductId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        public IActionResult Remove(int id)
        {
            var cart = SessionHelper.GetObjectJson<List<Oder>>(HttpContext.Session, "cart");
            int index = checkProduct(id);

            //cach 1:
            // xóa theo kiểu tăng hoặc giảm số lượng sản phẩm trong giỏ hàng 
            // nếu số lượng sản phẩm trong giỏ hàng lớn hơn 1 thì giảm số lượng sản phẩm đi 1
            // nếu số lượng sản phẩm trong giỏ hàng bằng 1 thì xóa sản phẩm đó khỏi giỏ hàng
            if (cart![index].Quantity > 1)
            {
                cart[index].Quantity--;
                SessionHelper.SetObjectJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                cart.RemoveAt(index);
                SessionHelper.SetObjectJson(HttpContext.Session, "cart", cart);
            }

            // cach 2:
            //cart.RemoveAt(index); // System.ArgumentOutOfRangeException: 'Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')' - khi xóa sản phẩm trong giỏ hàng mà giỏ hàng đang rỗng thì sẽ bị lỗi này 
            //SessionHelper.SetObjectJson(HttpContext.Session, "cart", cart);

            // cach 3:
            //if(index != null)
            //{
            //    cart.RemoveAt(index);
            //    SessionHelper.SetObjectJson(HttpContext.Session, "cart", cart);
            //}
            //else
            //{
            //    cart = new List<Oder>();
            //    SessionHelper.SetObjectJson(HttpContext.Session, "cart", cart);
            //}

            return RedirectToAction("Index");
        }

        public IActionResult Buy(int id)
        {
            var model = JsonConvert.DeserializeObject<IEnumerable<Product>>(httpClient.GetStringAsync(url).Result);
            List<Oder> cart;

            if (SessionHelper.GetObjectJson<List<Oder>>(HttpContext.Session, "cart") == null)
            {

                cart = new List<Oder>();
                cart.Add(new Oder // InvalidOperationException: Sequence contains no matching element - khi chưa có sản phẩm nào trong giỏ hàng mà đã nhấn vào nút mua hàng thì sẽ bị lỗi này 
                // cách khắc phục là thêm điều kiện if(model != null) trước khi thêm sản phẩm vào giỏ hàng
                
                {
                    Product = model!.Single(p => p.ProductId.Equals(id)),
                    Quantity = 1,
                    OderDate = DateTime.Now 

                });
                SessionHelper.SetObjectJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                cart = SessionHelper.GetObjectJson<List<Oder>>(HttpContext.Session, "cart");

                int index = checkProduct(id);
                if (index == -1)
                {
                    cart.Add(new Oder
                    {
                        Product = model.Single(p => p.ProductId.Equals(id)),
                        Quantity = 1,
                        OderDate = DateTime.Now
                    });
                }
                else
                {
                    cart[index].Quantity++;
                }
                SessionHelper.SetObjectJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Success(int id)
        {
            ViewBag.msg = "Your order has been placed successfully!";
            SessionHelper.SetObjectJson(HttpContext.Session, "cart", null!);
            return View();
        }

        // Update quantity of product in cart
    }
}

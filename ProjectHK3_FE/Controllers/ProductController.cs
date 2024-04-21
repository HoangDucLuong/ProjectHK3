using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE.Models;

namespace ProjectHK3_FE.Controllers
{
    public class ProductController : Controller
    {
        public async Task<ActionResult> Index(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:7283/api/SanPham/GetSanPham/" + id;
                //images/women-clothes-img.png

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    Product product = JsonConvert.DeserializeObject<Product>(responseData);

                    //ViewBag.ApiData = responseData;
                    return View(product);

                }
                else
                {
                    return View(new Product());

                }
            }
        }

		[HttpPost]
		public IActionResult AddToCart(int id)
		{
			// Xử lý logic thêm sản phẩm có id vào giỏ hàng
			// Ví dụ: 
			// 1. Thêm sản phẩm vào giỏ hàng
			// 2. Chuyển hướng người dùng đến trang giỏ hàng

			return RedirectToAction("Index", "Product"); // Chuyển hướng đến action "Cart" trong controller "Shopping"
		}

	}
}

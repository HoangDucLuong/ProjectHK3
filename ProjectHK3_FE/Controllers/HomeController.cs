using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectHK3_FE.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

        public async Task<ActionResult> Index()
		{
			// Khởi tạo HttpClient
			using (HttpClient client = new HttpClient())
            {
                //                                @RenderSection("ProductList", required: false)

                // Địa chỉ của API bạn muốn gọi
                string apiUrl = "https://localhost:7283/api/TaiKhoanMatKhau/GetTaiKhoanMatKhau";
                //images/women-clothes-img.png

                // Gọi API và nhận dữ liệu trả về
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc nội dung JSON từ response
                    string responseData = await response.Content.ReadAsStringAsync();

                    List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(responseData);

                    // Truyền dữ liệu nhận được từ API vào ViewBag
                    //ViewBag.ApiData = responseData;
                    return View(productList);

                }
                else
                {
                    // Xử lý lỗi nếu có
                    //ViewBag.ApiData = "Error calling the API";
                    return View(new List<Product>());

                }
            }

		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}

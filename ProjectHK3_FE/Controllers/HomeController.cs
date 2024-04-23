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
			using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:7283/api/SanPham/GetSanPhams";
                //images/women-clothes-img.png

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(responseData);

                    //ViewBag.ApiData = responseData;
                    return View(productList);

                }
                else
                {
                    //ViewBag.ApiData = "Error calling the API";
                    return View(new List<Product>());

                }
            }

		}

		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}
		public IActionResult About()
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

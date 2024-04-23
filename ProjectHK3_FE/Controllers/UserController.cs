using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProjectHK3_FE.Models;
using System.Security.Claims;
using System.Text;

namespace ProjectHK3_FE.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Login()
		{
			return View(new List<Product>());
		}
		public IActionResult Register()
		{
			return View(new List<Product>());
		}

        public async Task<IActionResult> Logout()
        {
            using (var client = new HttpClient())
            {
                // Đặt URL của API
                string apiUrl = "https://localhost:7283/api/Auth/Logout/logout";

                //var json = $"{{\"username\":\"{username}\", \"password\":\"{password}\"}}";
                //var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, null);

                // Xác định liệu request có thành công không
                if (response.IsSuccessStatusCode)
                {
                    // Đọc và parse dữ liệu JSON từ response
                    string responseData = await response.Content.ReadAsStringAsync();

                    HttpContext.Session.Remove("Username");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Content("Error: " + response.StatusCode.ToString());
                }
            }
        }

        [HttpPost]
		public async Task<ActionResult> SendLogin(string username, string password)
		{
			// Gọi API và nhận dữ liệu trả về
			using (var client = new HttpClient())
			{
				// Đặt URL của API
				string apiUrl = "https://localhost:7283/api/Auth/Login/login";

				var json = $"{{\"username\":\"{username}\", \"password\":\"{password}\"}}";
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				//client.DefaultRequestHeaders.Add("Content-Type", "application/json");

				// Gửi POST request đến API và nhận dữ liệu JSON
				HttpResponseMessage response = await client.PostAsync(apiUrl, content);

				// Xác định liệu request có thành công không
				if (response.IsSuccessStatusCode)
				{
					// Đọc và parse dữ liệu JSON từ response
					string responseData = await response.Content.ReadAsStringAsync();

					HttpContext.Session.SetString("Username", username);
                    TempData["LoginSuccess"] = true;

                    return RedirectToAction("Index", "Home");
                    //return Content(responseData, "application/json");
					
				}
				else
				{
					// Trả về lỗi nếu request không thành công
					return Content("Error: " + response.StatusCode.ToString() + " uname " + username + "  pass " + password);
				}
			}
		}

		[HttpPost]
		public async Task<ActionResult> SendRegister(string username, string password)
		{
			// Gọi API và nhận dữ liệu trả về
			using (var client = new HttpClient())
			{
				// Đặt URL của API
				string apiUrl = "https://localhost:7283/api/Auth/Register/register";

				var json = $"{{\"username\":\"{username}\", \"password\":\"{password}\"}}";
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				//client.DefaultRequestHeaders.Add("Content-Type", "application/json");

				// Gửi POST request đến API và nhận dữ liệu JSON
				HttpResponseMessage response = await client.PostAsync(apiUrl, content);

				// Xác định liệu request có thành công không
				if (response.IsSuccessStatusCode)
				{
					// Đọc và parse dữ liệu JSON từ response
					string responseData = await response.Content.ReadAsStringAsync();

					HttpContext.Session.SetString("Username", username);

					return RedirectToAction("Index", "Home");

				}
				else
				{
					return Content("Error: " + response.StatusCode.ToString() + " uname " + username + "  pass " + password);
				}
			}
		}

    }
}

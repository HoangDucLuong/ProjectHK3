using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE_Admin.Models;
using System;
using System.Text;

namespace ProjectHK3_FE_Admin.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Login()
		{
			return View();
		}
		public IActionResult Register()
		{
			return View();
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

					return RedirectToAction("Index", "TaiKhoanMatKhau");
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
			using (var client = new HttpClient())
			{
				string apiUrl = "https://localhost:7283/api/Auth/Login/login";
                string getTaikhoanMatKhauUrl = "https://localhost:7283/api/TaiKhoanMatKhau/GetTaiKhoanMatKhau";

                var json = $"{{\"username\":\"{username}\", \"password\":\"{password}\"}}";
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await client.PostAsync(apiUrl, content);

				if (response.IsSuccessStatusCode)
				{
					string responseData = await response.Content.ReadAsStringAsync();

					HttpContext.Session.SetString("Username", username);

                    TempData["LoginSuccess"] = true;

                    HttpResponseMessage tkmkResponse = await client.GetAsync(getTaikhoanMatKhauUrl);
					if (tkmkResponse.IsSuccessStatusCode)
					{
                        string tkmkResponseData = await tkmkResponse.Content.ReadAsStringAsync();

                        List<TaiKhoanMatKhau> listTaikhoanMatkhau = JsonConvert.DeserializeObject<List<TaiKhoanMatKhau>>(tkmkResponseData);
                        foreach (var tkmk in listTaikhoanMatkhau)
                            if (tkmk.taiKhoan == username)
                            {
                                HttpContext.Session.SetInt32("Role", tkmk.role);
								break;
                            }

                        return RedirectToAction("Index", "TaiKhoanMatKhau");
                    }
                    else
					{
                        HttpContext.Session.SetInt32("Role", 0);
                        return RedirectToAction("Index", "TaiKhoanMatKhau");
                    }

                }
				else
				{
					// Trả về lỗi nếu request không thành công
					return Content("Error: " + response.StatusCode.ToString() + " uname " + username + "  pass " + password);
				}
			}
		}
	}
}

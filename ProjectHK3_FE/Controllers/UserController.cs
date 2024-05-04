using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE.Models;
using System.Net.Http;
using System.Reflection.Metadata;
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
		public async Task<ActionResult> Cart()
        {
			using (HttpClient client = new HttpClient())
			{
				string apiUrl = "https://localhost:7283/api/DonDatHang";

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					string responseData = await response.Content.ReadAsStringAsync();

					List<DonHang> listDonHang = JsonConvert.DeserializeObject<List<DonHang>>(responseData);

					HttpResponseMessage sanPhamResponse = await client.GetAsync("https://localhost:7283/api/SanPham/GetSanPhams");
					if (sanPhamResponse.IsSuccessStatusCode)
					{
						string productJson = await sanPhamResponse.Content.ReadAsStringAsync();
						List<Product> products = JsonConvert.DeserializeObject<List<Product>>(productJson);
						foreach (var donHang in listDonHang)
							foreach (var product in products)
								if (donHang.maSoSanPham == product.maSoSanPham)
								{
									donHang.tenSanPham = product.tenSanPham;
									donHang.donGia = product.gia;
								}

					}
					return View(listDonHang);

				}
				else
				{
					return View(new List<DonHang>());

				}
			}
		}

		public async Task<ActionResult> Payment(string masosp, string tensp, int soluong, int magiaohang, double dongia)
		{
			using (HttpClient client = new HttpClient())
			{
				string apiUrl = "https://localhost:7283/api/KhachHang/GetKhachHangs";
				string donhangUrl = "https://localhost:7283/api/DonDatHang";

				var userEmail = HttpContext.Session.GetString("Username");
				int maKH = 0;
				string dateDatHang = "2024-05-02T15:37:36.221Z";

				HttpResponseMessage response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					string responseData = await response.Content.ReadAsStringAsync();

					List<KhachHang> listKhachHang = JsonConvert.DeserializeObject<List<KhachHang>>(responseData);
					//Context.Session.GetString("Username");

					foreach (var khachhang in listKhachHang)
						if (khachhang.email == userEmail)
						{
							maKH = khachhang.maKhachHang;
						}

					var json = $"{{\"maKhachHang\":\"{maKH}\", \"maSoSanPham\":\"{masosp}\", \"soLuongMua\":{soluong}, \"ngayDat\":\"{dateDatHang}\", \"thanhToan\":\"{masosp}\", \"maLoaiGiaoHang\":\"{magiaohang}\"}}";
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					// Gửi POST request đến API và nhận dữ liệu JSON
					HttpResponseMessage donhangResponse = await client.PostAsync(apiUrl, content);

					// Xác định liệu request có thành công không
					if (response.IsSuccessStatusCode)
					{
						// Đọc và parse dữ liệu JSON từ response
						//string donhangResponseData = await response.Content.ReadAsStringAsync();

						return View(new DonHang("000", maKH, masosp, soluong, dateDatHang, magiaohang, tensp, dongia));

					}
					else
					{
						return Content("Error: " + response.StatusCode.ToString() + " maso: " + masosp + "  ten: " + tensp);
					}

				}
				else
				{
					return Content("Error: " + response.Content.ToString() +"\r\n" 
						+ response.StatusCode.ToString() +" uEmail:" + userEmail + " maKH: " + maKH + " maso: " + masosp + "  ten: " + tensp +" sluong: " + soluong+"  maGH: " + magiaohang +"  date: " + dateDatHang);

					//return View(new List<DonHang>());

				}
			}
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

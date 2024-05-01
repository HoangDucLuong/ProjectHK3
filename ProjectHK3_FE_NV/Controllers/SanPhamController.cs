using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE_NV.Models;
using System.Text;

namespace ProjectHK3_FE_NV.Controllers
{
    public class SanPhamController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7283/api");
        private readonly HttpClient _client;

        public SanPhamController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<SanPhamViewModel> list = new List<SanPhamViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/SanPham/GetSanPhams").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(data);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SanPhamViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/SanPham/PostSanPham", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Loại sản phẩm đã được tạo thành công.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                SanPhamViewModel model = new SanPhamViewModel();
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/SanPham/GetSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<SanPhamViewModel>(data);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, SanPhamViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Trả về View với model nếu có lỗi
                    return View(model);
                }

                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/SanPham/PutSanPham", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Sản phẩm đã được cập nhật.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Nếu không thành công, thêm thông báo lỗi vào TempData
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi cập nhật sản phẩm.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                // Nếu có lỗi trong quá trình xử lý, thêm thông báo lỗi vào TempData
                TempData["errorMessage"] = ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/SanPham/GetSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    SanPhamViewModel model = JsonConvert.DeserializeObject<SanPhamViewModel>(data);
                    return View(model);
                }
                else
                {
                    TempData["errorMessage"] = "Không tìm thấy sản phẩm để xóa.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/SanPham/DeleteSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Sản phẩm đã được xóa.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi xóa sản phẩm.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}

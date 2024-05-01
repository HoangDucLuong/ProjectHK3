using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE_NV.Models;
using System.Text;

namespace ProjectHK3_FE_NV.Controllers
{
    public class LoaiSanPhamController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7283/api");
        private readonly HttpClient _client;

        public LoaiSanPhamController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<LoaiSanPhamViewModel> list = new List<LoaiSanPhamViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/LoaiSanPham/GetLoaiSanPhams").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<LoaiSanPhamViewModel>>(data);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(LoaiSanPhamViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/LoaiSanPham/PostLoaiSanPham", content).Result;
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
                LoaiSanPhamViewModel model = new LoaiSanPhamViewModel();
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/LoaiSanPham/GetLoaiSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<LoaiSanPhamViewModel>(data);
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
        public IActionResult Edit(int id, LoaiSanPhamViewModel model)
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
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/LoaiSanPham/PutLoaiSanPham", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Loại sản phẩm đã được cập nhật.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Nếu không thành công, thêm thông báo lỗi vào TempData
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi cập nhật nhân viên.";
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
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/LoaiSanPham/GetLoaiSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    LoaiSanPhamViewModel model = JsonConvert.DeserializeObject<LoaiSanPhamViewModel>(data);
                    return View(model);
                }
                else
                {
                    TempData["errorMessage"] = "Không tìm thấy loại sản phẩm để xóa.";
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
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/LoaiSanPham/DeleteLoaiSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Loại sản phẩm đã được xóa.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi xóa loại sản phẩm.";
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

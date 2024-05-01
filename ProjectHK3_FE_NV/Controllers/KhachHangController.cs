using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE_NV.Models;
using System.Text;

namespace ProjectHK3_FE_NV.Controllers
{
    public class KhachHangController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7283/api");
        private readonly HttpClient _client;

        public KhachHangController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<KhachHangViewModel> list = new List<KhachHangViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/KhachHang/GetKhachHangs").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<KhachHangViewModel>>(data);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KhachHangViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/KhachHang/PostKhachHang", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Khách hàng đã được tạo thành công.";
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
                KhachHangViewModel model = new KhachHangViewModel();
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/KhachHang/GetKhachHang/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<KhachHangViewModel>(data);
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
        public IActionResult Edit(int id, KhachHangViewModel model)
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
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/KhachHang/PutKhachHang", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Khách hàng đã được cập nhật.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Nếu không thành công, thêm thông báo lỗi vào TempData
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi cập nhật khách hàng.";
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
                // Gửi yêu cầu GET để lấy thông tin của khách hàng cần xóa
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/KhachHang/GetKhachHang/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    KhachHangViewModel model = JsonConvert.DeserializeObject<KhachHangViewModel>(data);
                    return View(model);
                }
                else
                {
                    TempData["errorMessage"] = "Không tìm thấy khách hàng để xóa.";
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
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/KhachHang/DeleteKhachHang/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Khách hàng đã được xóa.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi xóa khách hàng.";
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

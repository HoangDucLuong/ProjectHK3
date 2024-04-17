using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE_Admin.Models;
using System.Text;

namespace ProjectHK3_FE_Admin.Controllers
{
    public class CodeSanPhamController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7283/api");
        private readonly HttpClient _client;

        public CodeSanPhamController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<CodeSanPhamViewModel> list = new List<CodeSanPhamViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/CodeSanPham/GetCodeSanPhams").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<CodeSanPhamViewModel>>(data);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CodeSanPhamViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/CodeSanPham/PostCodeSanPham", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Nhân viên đã được tạo thành công.";
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
                CodeSanPhamViewModel model = new CodeSanPhamViewModel();
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/CodeSanPham/GetCodeSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<CodeSanPhamViewModel>(data);
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
        public IActionResult Edit(int id, CodeSanPhamViewModel model)
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
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/CodeSanPham/PutCodeSanPham", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Nhân viên đã được cập nhật.";
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
                // Gửi yêu cầu GET để lấy thông tin của nhân viên cần xóa
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/CodeSanPham/GetCodeSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    CodeSanPhamViewModel model = JsonConvert.DeserializeObject<CodeSanPhamViewModel>(data);
                    return View(model);
                }
                else
                {
                    TempData["errorMessage"] = "Không tìm thấy nhân viên để xóa.";
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
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/CodeSanPham/DeleteCodeSanPham/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Nhân viên đã được xóa.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi xóa nhân viên.";
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

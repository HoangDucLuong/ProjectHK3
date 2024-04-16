using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE_Admin.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;

namespace ProjectHK3_FE_Admin.Controllers
{
    public class TaiKhoanMatKhauController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7283/api");
        private readonly HttpClient _client;

        public TaiKhoanMatKhauController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<TaiKhoanMatKhauViewModel> list = new List<TaiKhoanMatKhauViewModel>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/TaiKhoanMatKhau/GetTaiKhoanMatKhau").Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<TaiKhoanMatKhauViewModel>>(data);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaiKhoanMatKhauViewModel model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "/TaiKhoanMatKhau/PostTaiKhoanMatKhau", content).Result; 
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Tai khoan da duoc tao.";
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
                TaiKhoanMatKhauViewModel model = new TaiKhoanMatKhauViewModel();
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "/TaiKhoanMatKhau/GetTaiKhoanMatKhau/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    model = JsonConvert.DeserializeObject<TaiKhoanMatKhauViewModel>(data);
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
        public IActionResult Edit(int id, TaiKhoanMatKhauViewModel model)
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
                HttpResponseMessage response = _client.PutAsync($"{_client.BaseAddress}/TaiKhoanMatKhau/PutTaiKhoanMatKhau", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Tài khoản đã được cập nhật.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Nếu không thành công, thêm thông báo lỗi vào TempData
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi cập nhật tài khoản.";
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
                // Gửi yêu cầu GET để lấy thông tin của tài khoản cần xóa
                HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/TaiKhoanMatKhau/GetTaiKhoanMatKhau/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    TaiKhoanMatKhauViewModel model = JsonConvert.DeserializeObject<TaiKhoanMatKhauViewModel>(data);
                    return View(model);
                }
                else
                {
                    TempData["errorMessage"] = "Không tìm thấy tài khoản để xóa.";
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
                HttpResponseMessage response = _client.DeleteAsync($"{_client.BaseAddress}/TaiKhoanMatKhau/DeleteTaiKhoanMatKhau/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Tài khoản đã được xóa.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Đã xảy ra lỗi khi xóa tài khoản.";
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

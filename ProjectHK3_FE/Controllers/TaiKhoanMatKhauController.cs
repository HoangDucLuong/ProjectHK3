using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace ProjectK3_Client.Controllers
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
	}
}

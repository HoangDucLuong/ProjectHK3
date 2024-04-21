using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE.Models;

namespace ProjectHK3_FE.Controllers
{
    public class ProductController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:7283/api/TaiKhoanMatKhau/GetTaiKhoanMatKhau";
                //images/women-clothes-img.png

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(responseData);

                    //ViewBag.ApiData = responseData;
                    return View(productList[0]);

                }
                else
                {
                    return View(new List<Product>());

                }
            }
        }
    }
}

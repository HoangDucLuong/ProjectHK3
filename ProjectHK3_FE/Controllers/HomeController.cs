using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectHK3_FE.Models;
using ProjectHK3_FE.Others;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace ProjectHK3_FE.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

        public async Task<ActionResult> Index()
		{
			using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://localhost:7283/api/SanPham/GetSanPhams";
                //images/women-clothes-img.png

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    List<Product> productList = JsonConvert.DeserializeObject<List<Product>>(responseData);

                    //ViewBag.ApiData = responseData;
                    return View(productList);

                }
                else
                {
                    //ViewBag.ApiData = "Error calling the API";
                    return View(new List<Product>());

                }
            }

		}

		public IActionResult Privacy()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}
		public IActionResult About()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}



        public ActionResult Payment(string donationAmount, string donorID, string charityActivity)
        {
            //string url = ConfigurationManager.AppSettings["Url"];
            //string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            //string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            //string hashSecret = ConfigurationManager.AppSettings["HashSecret"];



            string url = "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
	        string returnUrl = "https://localhost:7283/api/ThanhToan";
	        string tmnCode = "JITYBPC1";
	        string hashSecret = "UHPETABJAMBLLTBVQJICHJRGOIQCMDSR";


			PayLib pay = new PayLib();

            // Lấy số tiền đóng góp từ form
            string amount = "0";
            if (donationAmount == "other")
            {
                // Nếu người dùng chọn "Other", lấy số tiền từ ô nhập
                amount = Request.Form["otherAmount"];
            }
            else
            {
                // Ngược lại, lấy số tiền từ lựa chọn
                amount = donationAmount;
            }
            amount = "100000";
            // Thêm dữ liệu vào đối tượng PayLib
            pay.AddRequestData("vnp_Version", "2.1.0");
            pay.AddRequestData("vnp_Command", "pay");
            pay.AddRequestData("vnp_TmnCode", tmnCode);
            pay.AddRequestData("vnp_Amount", "1000000");//(Convert.ToInt32(amount) * 100 * 24000).ToString()); // Số tiền cần chuyển đổi thành đơn vị của VNPAY
            pay.AddRequestData("vnp_BankCode", "");
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", "VND");
            pay.AddRequestData("vnp_IpAddr", "***");//Request.Headers["X-Forwarded-For"]);//Util.GetIpAddress());

			pay.AddRequestData("vnp_Locale", "vn");
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang");
            pay.AddRequestData("vnp_OrderType", "other");
            pay.AddRequestData("vnp_ReturnUrl", returnUrl);
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
			DateTime newTime = DateTime.Now.AddHours(1);

			pay.AddRequestData("vnp_ExpireDate", newTime.ToString("yyyyMMddHHmmss"));



			string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            //return Content("MyUrl: " + url);
            return Redirect(paymentUrl);
        }

        /*
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
        */

    }
}

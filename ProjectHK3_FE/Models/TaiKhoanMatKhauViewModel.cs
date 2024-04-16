////using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ProjectHK3_FE.Models
{
	public class TaiKhoanMatKhauViewModel
	{

		public int MaTaiKhoan { get; set; }
		[Required]
		public string TaiKhoan { get; set; } 
		[Required]
		public string MatKhau { get; set; }
		[Required]
		public int Role { get; set; }
	}
}

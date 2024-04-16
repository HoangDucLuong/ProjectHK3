using System.ComponentModel.DataAnnotations;

namespace ProjectHK3_FE_Admin.Models
{
    public class TaiKhoanMatKhauViewModel
    {
        public int MaTaiKhoan { get; set; }
        [Required]
        public string? TaiKhoan { get; set; }
        [Required]
        public string? MatKhau { get; set; }
        public int Role { get; set; }
    }
}

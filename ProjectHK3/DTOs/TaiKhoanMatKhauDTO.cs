using System.Text.Json.Serialization;

namespace ProjectHK3.DTOs
{
    public class TaiKhoanMatKhauDTO
    {
        public int MaTaiKhoan { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string Role { get; set; }
    }
}


namespace ProjectHK3.DTOs
{
    public class TaiKhoanMatKhauDTO
    {
        public int MaTaiKhoan { get; set; }

        public required string TaiKhoan { get; set; }

        public required string MatKhau { get; set; }

        public int Role { get; set; }
    }
}

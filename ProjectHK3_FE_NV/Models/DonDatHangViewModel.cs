namespace ProjectHK3_FE_Admin.Models
{
    public class DonDatHangViewModel
    {
        public string? MaDonHang { get; set; }
        public int? MaKhachHang { get; set; }
        public string? MaSoSanPham { get; set; }
        public int SoLuongMua { get; set; }
        public DateTime NgayDat { get; set; }
        public string? ThanhToan { get; set; }
        public int MaLoaiGiaoHang { get; set; }
    }
}

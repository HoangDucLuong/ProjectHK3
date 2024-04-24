namespace ProjectHK3.DTOs
{
    public class DonDatHangDTO
    {
        public string MaDonHang { get; set; }
        public int? MaKhachHang { get; set; }
        public string? MaSoSanPham { get; set; }
        public int SoLuongMua { get; set; }
        public DateTime NgayDat { get; set; }
        public string ThanhToan { get; set; }
        public int MaLoaiGiaoHang { get; set; }
    }
}


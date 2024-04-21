namespace ProjectHK3.DTOs
{
    public class DonDatHangDTO
    {
        public string MaDonHang { get; set; }
        public int? MaKhachHang { get; set; }
        public int? MaSanPham { get; set; }
        public int LoaiGiaoHang { get; set; }
        public DateTime NgayDat { get; set; }
        public string ThanhToan { get; set; }
        public int MaLoaiGiaoHang { get; set; }
    }
}

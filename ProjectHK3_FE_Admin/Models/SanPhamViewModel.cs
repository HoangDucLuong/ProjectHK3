namespace ProjectHK3_FE_Admin.Models
{
    public class SanPhamViewModel
    {
        public int MaSanPham { get; set; }
        public string? MaSoSanPham { get; set; }
        public string? TenSanPham { get; set; }
        public string? MoTaSanPham { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public int? MaLoai { get; set; }
    }
}

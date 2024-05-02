namespace ProjectHK3_FE.Models
{
    public class DonHang
    {
        public DonHang(string maDH, int maKH, string masoSP, int sluong, string ngay, int maloaiGH, string tenSP, double dongia)
        {
            maDonHang = maDH;
            maKhachHang = maKH;
            maSoSanPham = masoSP;
            soLuongMua = sluong;
            ngayDat = ngay;
            thanhToan = "1";
            maLoaiGiaoHang = maloaiGH;
            tenSanPham = tenSP;
            donGia = dongia;
		}
        public string maDonHang { get; set; }
        public int maKhachHang { get; set; }
        public string maSoSanPham { get; set; }
        public int soLuongMua { get; set; }
        public string ngayDat { get; set; }
        public string thanhToan { get; set; }
        public int maLoaiGiaoHang { get; set; }
        public string tenSanPham { get; set; }
		public double donGia { get; set; }
	}
}

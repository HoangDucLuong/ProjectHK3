using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class DonDatHang
{
    public string MaDonHang { get; set; } = null!;

    public int? MaKhachHang { get; set; }

    public int? MaSanPham { get; set; }

    public int SoLuongSanPham { get; set; }

    public int LoaiGiaoHang { get; set; }

    public DateTime NgayDat { get; set; }

    public string ThanhToan { get; set; } = null!;

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}

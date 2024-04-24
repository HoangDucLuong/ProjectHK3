using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class DonDatHang
{
    public string MaDonHang { get; set; } = null!;

    public int? MaKhachHang { get; set; }

    public string? MaSoSanPham { get; set; }

    public int SoLuongMua { get; set; }

    public DateTime NgayDat { get; set; }

    public string ThanhToan { get; set; } = null!;

    public int MaLoaiGiaoHang { get; set; }

    public virtual KhachHang? MaKhachHangNavigation { get; set; }

    public virtual LoaiGiaoHang MaLoaiGiaoHangNavigation { get; set; } = null!;

    public virtual SanPham? MaSoSanPhamNavigation { get; set; }

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}

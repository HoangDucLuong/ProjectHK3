using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class KhachHang
{
    public int MaKhachHang { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public string? Email { get; set; }

    public string? SoDienThoai { get; set; }

    public int MaTaiKhoan { get; set; }

    public virtual ICollection<DonDatHang> DonDatHangs { get; set; } = new List<DonDatHang>();

    public virtual TaiKhoanMatKhau MaTaiKhoanNavigation { get; set; } = null!;

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}

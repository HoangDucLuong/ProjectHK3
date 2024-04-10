using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class NhanVien
{
    public int MaNhanVien { get; set; }

    public string TenNhanVien { get; set; } = null!;

    public int MaTaiKhoan { get; set; }

    public virtual TaiKhoanMatKhau MaTaiKhoanNavigation { get; set; } = null!;
}

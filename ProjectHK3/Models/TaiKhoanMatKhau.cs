using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class TaiKhoanMatKhau
{
    public int MaTaiKhoan { get; set; }

    public string TaiKhoan { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public int Role { get; set; }

    public virtual KhachHang? KhachHang { get; set; }

    public virtual NhanVien? NhanVien { get; set; }
}

﻿using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class ThanhToan
{
    public int MaThanhToan { get; set; }

    public string MaDonHang { get; set; } = null!;

    public int? MaKhachHang { get; set; }

    public string PhuongThucThanhToan { get; set; } = null!;

    public DateTime ThoiGian { get; set; }

    public virtual DonDatHang MaDonHangNavigation { get; set; } = null!;

    public virtual KhachHang? MaKhachHangNavigation { get; set; }
}

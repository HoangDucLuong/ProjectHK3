using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public string MaSoSanPham { get; set; } = null!;

    public string TenSanPham { get; set; } = null!;

    public int? MaLoai { get; set; }

    public string? MoTaSanPham { get; set; }

    public int SoLuong { get; set; }

    public decimal Gia { get; set; }

    public virtual ICollection<DonDatHang> DonDatHangs { get; set; } = new List<DonDatHang>();
}

using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public string MaSoSanPham { get; set; } = null!;

    public string TenSanPham { get; set; } = null!;

    public string? MoTaSanPham { get; set; }

    public decimal Gia { get; set; }

    public int? MaLoai { get; set; }

    public virtual ICollection<CodeSanPham> CodeSanPhams { get; set; } = new List<CodeSanPham>();

    public virtual ICollection<DonDatHang> DonDatHangs { get; set; } = new List<DonDatHang>();

    public virtual LoaiSanPham? MaLoaiNavigation { get; set; }
}

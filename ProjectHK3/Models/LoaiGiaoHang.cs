using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class LoaiGiaoHang
{
    public int MaLoaiGiaoHang { get; set; }

    public string TenLoaiGiaoHang { get; set; } = null!;

    public virtual ICollection<DonDatHang> DonDatHangs { get; set; } = new List<DonDatHang>();
}

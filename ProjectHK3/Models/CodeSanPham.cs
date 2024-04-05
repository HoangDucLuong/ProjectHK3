using System;
using System.Collections.Generic;

namespace ProjectHK3.Models;

public partial class CodeSanPham
{
    public int MaCode { get; set; }

    public int? MaSanPham { get; set; }

    public virtual SanPham? MaSanPhamNavigation { get; set; }
}

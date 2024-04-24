using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHK3.Models;
using ProjectHK3.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHK3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonDatHangController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public DonDatHangController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/DonDatHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonDatHangDTO>>> GetDonDatHangs()
        {
            var donDatHangs = await _context.DonDatHangs
                .Select(ddh => new DonDatHangDTO
                {
                    MaDonHang = ddh.MaDonHang,
                    MaKhachHang = ddh.MaKhachHang,
                    MaSoSanPham = ddh.MaSoSanPham,
                    SoLuongMua = ddh.SoLuongMua,
                    NgayDat = ddh.NgayDat,
                    ThanhToan = ddh.ThanhToan,
                    MaLoaiGiaoHang = ddh.MaLoaiGiaoHang
                }).ToListAsync();

            return donDatHangs;
        }

        // GET: api/DonDatHang
        [HttpGet("{id}")]
        public async Task<ActionResult<DonDatHangDTO>> GetDonDatHang(string id)
        {
            var donDatHang = await _context.DonDatHangs.FindAsync(id);

            if (donDatHang == null)
            {
                return NotFound();
            }

            var donDatHangDTO = new DonDatHangDTO
            {
                MaDonHang = donDatHang.MaDonHang,
                MaKhachHang = donDatHang.MaKhachHang,
                MaSoSanPham = donDatHang.MaSoSanPham,
                SoLuongMua = donDatHang.SoLuongMua,
                NgayDat = donDatHang.NgayDat,
                ThanhToan = donDatHang.ThanhToan,
                MaLoaiGiaoHang = donDatHang.MaLoaiGiaoHang
            };

            return donDatHangDTO;
        }

        // POST: api/DonDatHang
        [HttpPost]
        public async Task<ActionResult<DonDatHangDTO>> PostDonDatHang(DonDatHangDTO donDatHangDTO)
        {
            var donDatHang = new DonDatHang
            {
                MaDonHang = donDatHangDTO.MaDonHang,
                MaKhachHang = donDatHangDTO.MaKhachHang,
                MaSoSanPham = donDatHangDTO.MaSoSanPham,
                SoLuongMua = donDatHangDTO.SoLuongMua,
                NgayDat = donDatHangDTO.NgayDat,
                ThanhToan = donDatHangDTO.ThanhToan,
                MaLoaiGiaoHang = donDatHangDTO.MaLoaiGiaoHang
            };

            _context.DonDatHangs.Add(donDatHang);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDonDatHang), new { id = donDatHang.MaDonHang }, donDatHangDTO);
        }

        private bool DonDatHangExists(string id)
        {
            return _context.DonDatHangs.Any(e => e.MaDonHang == id);
        }
    }
}

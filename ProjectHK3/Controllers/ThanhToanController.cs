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
    public class ThanhToanController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public ThanhToanController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/ThanhToan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThanhToanDTO>>> GetThanhToans()
        {
            var thanhToans = await _context.ThanhToans
                .Select(tt => new ThanhToanDTO
                {
                    MaThanhToan = tt.MaThanhToan,
                    MaDonHang = tt.MaDonHang,
                    MaKhachHang = tt.MaKhachHang,
                    PhuongThucThanhToan = tt.PhuongThucThanhToan,
                    ThoiGian = tt.ThoiGian
                }).ToListAsync();

            return thanhToans;
        }

        // GET: api/ThanhToan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThanhToanDTO>> GetThanhToan(int id)
        {
            var thanhToan = await _context.ThanhToans.FindAsync(id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            var thanhToanDTO = new ThanhToanDTO
            {
                MaThanhToan = thanhToan.MaThanhToan,
                MaDonHang = thanhToan.MaDonHang,
                MaKhachHang = thanhToan.MaKhachHang,
                PhuongThucThanhToan = thanhToan.PhuongThucThanhToan,
                ThoiGian = thanhToan.ThoiGian
            };

            return thanhToanDTO;
        }

        // POST: api/ThanhToan
        [HttpPost]
        public async Task<ActionResult<ThanhToanDTO>> PostThanhToan(ThanhToanDTO thanhToanDTO)
        {
            var thanhToan = new ThanhToan
            {
                MaDonHang = thanhToanDTO.MaDonHang,
                MaKhachHang = thanhToanDTO.MaKhachHang,
                PhuongThucThanhToan = thanhToanDTO.PhuongThucThanhToan,
                ThoiGian = thanhToanDTO.ThoiGian
            };

            _context.ThanhToans.Add(thanhToan);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetThanhToan), new { id = thanhToan.MaThanhToan }, thanhToanDTO);
        }

        private bool ThanhToanExists(int id)
        {
            return _context.ThanhToans.Any(e => e.MaThanhToan == id);
        }
    }
}

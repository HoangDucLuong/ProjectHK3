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
    public class KhachHangController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public KhachHangController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/KhachHang
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHangDTO>>> GetKhachHangs()
        {
            var khachHangDTOs = await _context.KhachHangs
                .Select(kh => new KhachHangDTO
                {
                    MaKhachHang = kh.MaKhachHang,
                    TenKhachHang = kh.TenKhachHang,
                    DiaChi = kh.DiaChi,
                    Email = kh.Email,
                    SoDienThoai = kh.SoDienThoai,
                    MaTaiKhoan = kh.MaTaiKhoan // Lấy MaTaiKhoan từ cơ sở dữ liệu
                }).ToListAsync();

            return khachHangDTOs;
        }

        // GET: api/KhachHang/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KhachHangDTO>> GetKhachHang(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            if (khachHang == null)
            {
                return NotFound();
            }

            var khachHangDTO = new KhachHangDTO
            {
                MaKhachHang = khachHang.MaKhachHang,
                TenKhachHang = khachHang.TenKhachHang,
                DiaChi = khachHang.DiaChi,
                Email = khachHang.Email,
                SoDienThoai = khachHang.SoDienThoai,
                MaTaiKhoan = khachHang.MaTaiKhoan // Lấy MaTaiKhoan từ cơ sở dữ liệu
            };

            return khachHangDTO;
        }

        // POST: api/KhachHang
        [HttpPost]
        public async Task<IActionResult> PostKhachHang(KhachHangDTO khachHangDTO)
        {
            // Tạo mới đối tượng KhachHang từ DTO
            var khachHang = new KhachHang
            {
                TenKhachHang = khachHangDTO.TenKhachHang,
                DiaChi = khachHangDTO.DiaChi,
                Email = khachHangDTO.Email,
                SoDienThoai = khachHangDTO.SoDienThoai,
                MaTaiKhoan = khachHangDTO.MaTaiKhoan // Gán MaTaiKhoan từ DTO
            };

            // Thêm vào cơ sở dữ liệu
            _context.KhachHangs.Add(khachHang);
            await _context.SaveChangesAsync();

            // Trả về 201 Created và thông tin của khách hàng mới
            return CreatedAtAction(nameof(GetKhachHang), new { id = khachHang.MaKhachHang }, khachHang);
        }

        // PUT: api/KhachHang/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKhachHang(int id, KhachHangDTO khachHangDTO)
        {
            if (id != khachHangDTO.MaKhachHang)
            {
                return BadRequest();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            khachHang.TenKhachHang = khachHangDTO.TenKhachHang;
            khachHang.DiaChi = khachHangDTO.DiaChi;
            khachHang.Email = khachHangDTO.Email;
            khachHang.SoDienThoai = khachHangDTO.SoDienThoai;
            khachHang.MaTaiKhoan = khachHangDTO.MaTaiKhoan;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KhachHangExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/KhachHang/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKhachHang(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            _context.KhachHangs.Remove(khachHang);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KhachHangExists(int id)
        {
            return _context.KhachHangs.Any(e => e.MaKhachHang == id);
        }
    }
}

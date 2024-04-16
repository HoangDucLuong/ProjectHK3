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
    public class NhanVienController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public NhanVienController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/NhanVien
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVienDTO>>> GetNhanViens()
        {
            var nhanVienDTOs = await _context.NhanViens
                .Select(nv => new NhanVienDTO
                {
                    MaNhanVien = nv.MaNhanVien,
                    TenNhanVien = nv.TenNhanVien,
                    MaTaiKhoan = nv.MaTaiKhoan // Lấy MaTaiKhoan từ cơ sở dữ liệu
                }).ToListAsync();

            return nhanVienDTOs;
        }

        // GET: api/NhanVien/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVienDTO>> GetNhanVien(int id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            var nhanVienDTO = new NhanVienDTO
            {
                MaNhanVien = nhanVien.MaNhanVien,
                TenNhanVien = nhanVien.TenNhanVien,
                MaTaiKhoan = nhanVien.MaTaiKhoan // Lấy MaTaiKhoan từ cơ sở dữ liệu
            };

            return nhanVienDTO;
        }

        // POST: api/NhanVien
        [HttpPost]
        public async Task<IActionResult> PostNhanVien(NhanVienDTO nhanVienDTO)
        {
            // Tạo mới đối tượng NhanVien từ DTO
            var nhanVien = new NhanVien
            {
                TenNhanVien = nhanVienDTO.TenNhanVien,
                MaTaiKhoan = nhanVienDTO.MaTaiKhoan // Gán MaTaiKhoan từ DTO
            };

            // Thêm vào cơ sở dữ liệu
            _context.NhanViens.Add(nhanVien);
            await _context.SaveChangesAsync();

            // Trả về 201 Created và thông tin của nhân viên mới
            return CreatedAtAction(nameof(GetNhanVien), new { id = nhanVien.MaNhanVien }, nhanVien);
        }

        // PUT: api/NhanVien/5
        [HttpPut]
        public async Task<IActionResult> PutNhanVien(NhanVienDTO nhanVienDTO)
        {
            if (nhanVienDTO == null || nhanVienDTO.MaNhanVien == null)
            {
                return BadRequest();
            }

            var nhanVien = await _context.NhanViens.FindAsync(nhanVienDTO.MaNhanVien);
            if (nhanVien == null)
            {
                return NotFound();
            }

            nhanVien.TenNhanVien = nhanVienDTO.TenNhanVien;
            nhanVien.MaTaiKhoan = nhanVienDTO.MaTaiKhoan;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(nhanVienDTO.MaNhanVien))
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


        // DELETE: api/NhanVien/5
        [HttpDelete]
        public async Task<IActionResult> DeleteNhanVien(NhanVienDTO nhanVienDTO)
        {
            if (nhanVienDTO == null || nhanVienDTO.MaNhanVien == null)
            {
                return BadRequest();
            }

            var nhanVien = await _context.NhanViens.FindAsync(nhanVienDTO.MaNhanVien);
            if (nhanVien == null)
            {
                return NotFound();
            }

            _context.NhanViens.Remove(nhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool NhanVienExists(int id)
        {
            return _context.NhanViens.Any(e => e.MaNhanVien == id);
        }
    }
}

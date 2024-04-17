using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHK3.Models;
using ProjectHK3.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHK3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public LoaiSanPhamController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/LoaiSanPham
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiSanPhamDTO>>> GetLoaiSanPhams()
        {
            var loaiSanPhamDTOs = await _context.LoaiSanPhams
                .Select(lsp => new LoaiSanPhamDTO
                {
                    MaLoai = lsp.MaLoai,
                    TenLoai = lsp.TenLoai
                }).ToListAsync();

            return loaiSanPhamDTOs;
        }

        // GET: api/LoaiSanPham/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LoaiSanPhamDTO>> GetLoaiSanPham(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);

            if (loaiSanPham == null)
            {
                return NotFound();
            }

            var loaiSanPhamDTO = new LoaiSanPhamDTO
            {
                MaLoai = loaiSanPham.MaLoai,
                TenLoai = loaiSanPham.TenLoai
            };

            return loaiSanPhamDTO;
        }

        // POST: api/LoaiSanPham
        [HttpPost]
        public async Task<IActionResult> PostLoaiSanPham(LoaiSanPhamDTO loaiSanPhamDTO)
        {
            // Tạo mới đối tượng LoaiSanPham từ DTO
            var loaiSanPham = new LoaiSanPham
            {
                TenLoai = loaiSanPhamDTO.TenLoai
            };

            // Thêm vào cơ sở dữ liệu
            _context.LoaiSanPhams.Add(loaiSanPham);
            await _context.SaveChangesAsync();

            // Trả về 201 Created và thông tin của loại sản phẩm mới
            return CreatedAtAction(nameof(GetLoaiSanPham), new { id = loaiSanPham.MaLoai }, loaiSanPham);
        }

        // PUT: api/LoaiSanPham/5
        [HttpPut]
        public async Task<IActionResult> PutLoaiSanPham(LoaiSanPhamDTO loaiSanPhamDTO)
        {
            if (loaiSanPhamDTO == null || loaiSanPhamDTO.MaLoai == null)
            {
                return BadRequest();
            }

            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(loaiSanPhamDTO.MaLoai);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            loaiSanPham.TenLoai = loaiSanPhamDTO.TenLoai;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoaiSanPhamExists(loaiSanPhamDTO.MaLoai))
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


        // DELETE: api/LoaiSanPham/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoaiSanPham(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null)
            {
                return NotFound();
            }

            _context.LoaiSanPhams.Remove(loaiSanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoaiSanPhamExists(int id)
        {
            return _context.LoaiSanPhams.Any(e => e.MaLoai == id);
        }
    }
}

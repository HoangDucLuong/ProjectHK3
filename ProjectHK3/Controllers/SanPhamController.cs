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
    public class SanPhamController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public SanPhamController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/SanPham
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SanPhamDTO>>> GetSanPhams()
        {
            var sanPhamDTOs = await _context.SanPhams
                .Select(sp => new SanPhamDTO
                {
                    MaSanPham = sp.MaSanPham,
                    MaSoSanPham = sp.MaSoSanPham,
                    TenSanPham = sp.TenSanPham,
                    MoTaSanPham = sp.MoTaSanPham,
                    Gia = sp.Gia,
                    MaLoai = sp.MaLoai
                }).ToListAsync();

            return sanPhamDTOs;
        }

        // GET: api/SanPham/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SanPhamDTO>> GetSanPham(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);

            if (sanPham == null)
            {
                return NotFound();
            }

            var sanPhamDTO = new SanPhamDTO
            {
                MaSanPham = sanPham.MaSanPham,
                MaSoSanPham = sanPham.MaSoSanPham,
                TenSanPham = sanPham.TenSanPham,
                MoTaSanPham = sanPham.MoTaSanPham,
                Gia = sanPham.Gia,
                MaLoai = sanPham.MaLoai
            };

            return sanPhamDTO;
        }

        // POST: api/SanPham
        [HttpPost]
        public async Task<IActionResult> PostSanPham(SanPhamDTO sanPhamDTO)
        {
            // Tạo mới đối tượng SanPham từ DTO
            var sanPham = new SanPham
            {
                MaSoSanPham = sanPhamDTO.MaSoSanPham,
                TenSanPham = sanPhamDTO.TenSanPham,
                MoTaSanPham = sanPhamDTO.MoTaSanPham,
                Gia = sanPhamDTO.Gia,
                MaLoai = sanPhamDTO.MaLoai
            };

            // Thêm vào cơ sở dữ liệu
            _context.SanPhams.Add(sanPham);
            await _context.SaveChangesAsync();

            // Trả về 201 Created và thông tin của sản phẩm mới
            return CreatedAtAction(nameof(GetSanPham), new { id = sanPham.MaSanPham }, sanPham);
        }

        // PUT: api/SanPham/5
        [HttpPut]
        public async Task<IActionResult> PutSanPham(SanPhamDTO sanPhamDTO)
        {
            if (sanPhamDTO == null || sanPhamDTO.MaSanPham == null)
            {
                return BadRequest();
            }

            var sanPham = await _context.SanPhams.FindAsync(sanPhamDTO.MaSanPham);
            if (sanPham == null)
            {
                return NotFound();
            }

            sanPham.MaSoSanPham = sanPhamDTO.MaSoSanPham;
            sanPham.TenSanPham = sanPhamDTO.TenSanPham;
            sanPham.MoTaSanPham = sanPhamDTO.MoTaSanPham;
            sanPham.Gia = sanPhamDTO.Gia;
            sanPham.MaLoai = sanPhamDTO.MaLoai;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanPhamExists(sanPhamDTO.MaSanPham))
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


        // DELETE: api/SanPham/5
        [HttpDelete]
        public async Task<IActionResult> DeleteSanPham(SanPhamDTO sanPhamDTO)
        {
            if (sanPhamDTO == null || sanPhamDTO.MaSanPham == null)
            {
                return BadRequest();
            }

            var sanPham = await _context.SanPhams.FindAsync(sanPhamDTO.MaSanPham);
            if (sanPham == null)
            {
                return NotFound();
            }

            _context.SanPhams.Remove(sanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool SanPhamExists(int id)
        {
            return _context.SanPhams.Any(e => e.MaSanPham == id);
        }
    }
}

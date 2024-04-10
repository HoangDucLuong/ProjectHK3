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
    public class CodeSanPhamController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public CodeSanPhamController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/CodeSanPham
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeSanPhamDTO>>> GetCodeSanPhams()
        {
            var codeSanPhamDTOs = await _context.CodeSanPhams
                .Select(cs => new CodeSanPhamDTO
                {
                    MaCode = cs.MaCode,
                    MaSanPham = (int)cs.MaSanPham
                }).ToListAsync();

            return codeSanPhamDTOs;
        }

        // GET: api/CodeSanPham/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeSanPhamDTO>> GetCodeSanPham(int id)
        {
            var codeSanPham = await _context.CodeSanPhams.FindAsync(id);

            if (codeSanPham == null)
            {
                return NotFound();
            }

            var codeSanPhamDTO = new CodeSanPhamDTO
            {
                MaCode = codeSanPham.MaCode,
                MaSanPham = (int)codeSanPham.MaSanPham
            };

            return codeSanPhamDTO;
        }

        // POST: api/CodeSanPham
        [HttpPost]
        public async Task<IActionResult> PostCodeSanPham(CodeSanPhamDTO codeSanPhamDTO)
        {
            var codeSanPham = new CodeSanPham
            {
                MaSanPham = codeSanPhamDTO.MaSanPham
            };

            _context.CodeSanPhams.Add(codeSanPham);
            await _context.SaveChangesAsync();

            // Không cần trả về CodeSanPhamDTO vì sau khi tạo, trigger sẽ tự động thêm MaCode vào CodeSanPham

            return NoContent();
        }

        // PUT: api/CodeSanPham/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCodeSanPham(int id, CodeSanPhamDTO codeSanPhamDTO)
        {
            if (id != codeSanPhamDTO.MaCode)
            {
                return BadRequest();
            }

            var codeSanPham = await _context.CodeSanPhams.FindAsync(id);
            if (codeSanPham == null)
            {
                return NotFound();
            }

            codeSanPham.MaSanPham = codeSanPhamDTO.MaSanPham;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeSanPhamExists(id))
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

        // DELETE: api/CodeSanPham/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCodeSanPham(int id)
        {
            var codeSanPham = await _context.CodeSanPhams.FindAsync(id);
            if (codeSanPham == null)
            {
                return NotFound();
            }

            _context.CodeSanPhams.Remove(codeSanPham);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CodeSanPhamExists(int id)
        {
            return _context.CodeSanPhams.Any(e => e.MaCode == id);
        }
    }
}

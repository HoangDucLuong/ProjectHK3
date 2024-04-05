using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHK3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHK3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToansController : ControllerBase
    {
        private readonly ProjectHk3Context _thanhToan;

        public ThanhToansController(ProjectHk3Context context)
        {
            _thanhToan = context;
        }

        // GET: api/ThanhToans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThanhToan>>> GetThanhToans()
        {
            return await _thanhToan.ThanhToans.ToListAsync();
        }

        // GET: api/ThanhToans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThanhToan>> GetThanhToan(int id)
        {
            var thanhToan = await _thanhToan.ThanhToans.FindAsync(id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            return thanhToan;
        }

        // PUT: api/ThanhToans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThanhToan(int id, ThanhToan thanhToan)
        {
            if (id != thanhToan.MaThanhToan)
            {
                return BadRequest();
            }

            _thanhToan.Entry(thanhToan).State = EntityState.Modified;

            try
            {
                await _thanhToan.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThanhToanExists(id))
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

        // POST: api/ThanhToans
        [HttpPost]
        public async Task<ActionResult<ThanhToan>> PostThanhToan(ThanhToan thanhToan)
        {
            _thanhToan.ThanhToans.Add(thanhToan);
            await _thanhToan.SaveChangesAsync();

            return CreatedAtAction("GetThanhToan", new { id = thanhToan.MaThanhToan }, thanhToan);
        }

        // DELETE: api/ThanhToans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThanhToan(int id)
        {
            var thanhToan = await _thanhToan.ThanhToans.FindAsync(id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            _thanhToan.ThanhToans.Remove(thanhToan);
            await _thanhToan.SaveChangesAsync();

            return NoContent();
        }

        private bool ThanhToanExists(int id)
        {
            return _thanhToan.ThanhToans.Any(e => e.MaThanhToan == id);
        }
    }
}

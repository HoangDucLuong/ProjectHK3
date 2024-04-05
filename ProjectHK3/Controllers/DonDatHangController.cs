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
    public class DonDatHangsController : ControllerBase
    {
        private readonly ProjectHk3Context _donDatHang;

        public DonDatHangsController(ProjectHk3Context context)
        {
            _donDatHang = context;
        }

        // GET: api/DonDatHangs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonDatHang>>> GetDonDatHangs()
        {
            return await _donDatHang.DonDatHangs.ToListAsync();
        }

        // GET: api/DonDatHangs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DonDatHang>> GetDonDatHang(string id)
        {
            var donDatHang = await _donDatHang.DonDatHangs.FindAsync(id);

            if (donDatHang == null)
            {
                return NotFound();
            }

            return donDatHang;
        }

        // PUT: api/DonDatHangs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonDatHang(string id, DonDatHang donDatHang)
        {
            if (id != donDatHang.MaDonHang)
            {
                return BadRequest();
            }

            _donDatHang.Entry(donDatHang).State = EntityState.Modified;

            try
            {
                await _donDatHang.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonDatHangExists(id))
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

        // POST: api/DonDatHangs
        [HttpPost]
        public async Task<ActionResult<DonDatHang>> PostDonDatHang(DonDatHang donDatHang)
        {
            _donDatHang.DonDatHangs.Add(donDatHang);
            await _donDatHang.SaveChangesAsync();

            return CreatedAtAction("GetDonDatHang", new { id = donDatHang.MaDonHang }, donDatHang);
        }

        // DELETE: api/DonDatHangs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonDatHang(string id)
        {
            var donDatHang = await _donDatHang.DonDatHangs.FindAsync(id);
            if (donDatHang == null)
            {
                return NotFound();
            }

            _donDatHang.DonDatHangs.Remove(donDatHang);
            await _donDatHang.SaveChangesAsync();

            return NoContent();
        }

        private bool DonDatHangExists(string id)
        {
            return _donDatHang.DonDatHangs.Any(e => e.MaDonHang == id);
        }
    }
}

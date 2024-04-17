using Microsoft.AspNetCore.Mvc;
using ProjectHK3.DTOs;
using ProjectHK3.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHK3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoaiGiaoHangController : ControllerBase
    {
        private readonly ProjectHk3Context _context;

        public LoaiGiaoHangController(ProjectHk3Context context)
        {
            _context = context;
        }

        // GET: api/LoaiGiaoHang
        [HttpGet]
        public IEnumerable<LoaiGiaoHangDTO> GetLoaiGiaoHangs()
        {
            return _context.LoaiGiaoHangs
                .Select(lg => new LoaiGiaoHangDTO
                {
                    MaLoaiGiaoHang = lg.MaLoaiGiaoHang,
                    TenLoaiGiaoHang = lg.TenLoaiGiaoHang
                })
                .ToList();
        }

        // GET: api/LoaiGiaoHang/5
        [HttpGet("{id}")]
        public ActionResult<LoaiGiaoHangDTO> GetLoaiGiaoHang(int id)
        {
            var loaiGiaoHang = _context.LoaiGiaoHangs.Find(id);

            if (loaiGiaoHang == null)
            {
                return NotFound();
            }

            return new LoaiGiaoHangDTO
            {
                MaLoaiGiaoHang = loaiGiaoHang.MaLoaiGiaoHang,
                TenLoaiGiaoHang = loaiGiaoHang.TenLoaiGiaoHang
            };
        }

        // POST: api/LoaiGiaoHang
        [HttpPost]
        public ActionResult<LoaiGiaoHangDTO> PostLoaiGiaoHang(LoaiGiaoHangDTO loaiGiaoHangDTO)
        {
            var loaiGiaoHang = new LoaiGiaoHang
            {
                TenLoaiGiaoHang = loaiGiaoHangDTO.TenLoaiGiaoHang
            };

            _context.LoaiGiaoHangs.Add(loaiGiaoHang);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetLoaiGiaoHang), new { id = loaiGiaoHang.MaLoaiGiaoHang }, loaiGiaoHangDTO);
        }

        // PUT: api/LoaiGiaoHang
        [HttpPut]
        public IActionResult PutLoaiGiaoHang(LoaiGiaoHangDTO loaiGiaoHangDTO)
        {
            if (loaiGiaoHangDTO == null)
            {
                return BadRequest();
            }

            var loaiGiaoHang = _context.LoaiGiaoHangs.Find(loaiGiaoHangDTO.MaLoaiGiaoHang);
            if (loaiGiaoHang == null)
            {
                return NotFound();
            }

            loaiGiaoHang.TenLoaiGiaoHang = loaiGiaoHangDTO.TenLoaiGiaoHang;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                if (!LoaiGiaoHangExists(loaiGiaoHangDTO.MaLoaiGiaoHang))
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

        // DELETE: api/LoaiGiaoHang
        [HttpDelete("{id}")]
        public IActionResult DeleteLoaiGiaoHang(int id)
        {
            var loaiGiaoHang = _context.LoaiGiaoHangs.Find(id);
            if (loaiGiaoHang == null)
            {
                return NotFound();
            }

            _context.LoaiGiaoHangs.Remove(loaiGiaoHang);
            _context.SaveChanges();

            return NoContent();
        }

        private bool LoaiGiaoHangExists(int id)
        {
            return _context.LoaiGiaoHangs.Any(e => e.MaLoaiGiaoHang == id);
        }
    }
}

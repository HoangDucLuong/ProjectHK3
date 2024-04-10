﻿    using Microsoft.AspNetCore.Mvc;
    using ProjectHK3.Models;
    using ProjectHK3.DTOs;
    using System.Collections.Generic;
    using System.Linq;

    namespace ProjectHK3.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class TaiKhoanMatKhauController : ControllerBase
        {
            private readonly ProjectHk3Context _context;

            public TaiKhoanMatKhauController(ProjectHk3Context context)
            {
                _context = context;
            }

            // GET: api/TaiKhoanMatKhau
            [HttpGet]
            public ActionResult<IEnumerable<TaiKhoanMatKhauDTO>> GetTaiKhoanMatKhau()
            {
                var taiKhoanMatKhauDTOs = _context.TaiKhoanMatKhaus.Select(tk => new TaiKhoanMatKhauDTO
                {
                    MaTaiKhoan = tk.MaTaiKhoan,
                    TaiKhoan = tk.TaiKhoan,
                    MatKhau = tk.MatKhau,
                    Role = tk.Role.ToString() // Chuyển đổi enum Role thành string
                }).ToList();

                return taiKhoanMatKhauDTOs;
            }

            // GET: api/TaiKhoanMatKhau/5
            [HttpGet("{id}")]
            public ActionResult<TaiKhoanMatKhauDTO> GetTaiKhoanMatKhau(int id)
            {
                var taiKhoanMatKhau = _context.TaiKhoanMatKhaus.Find(id);

                if (taiKhoanMatKhau == null)
                {
                    return NotFound();
                }

                var taiKhoanMatKhauDTO = new TaiKhoanMatKhauDTO
                {
                    MaTaiKhoan = taiKhoanMatKhau.MaTaiKhoan,
                    TaiKhoan = taiKhoanMatKhau.TaiKhoan,
                    MatKhau = taiKhoanMatKhau.MatKhau,
                    Role = taiKhoanMatKhau.Role.ToString()
                };

                return taiKhoanMatKhauDTO;
            }

        // POST: api/TaiKhoanMatKhau
        [HttpPost]
        public ActionResult<TaiKhoanMatKhauDTO> PostTaiKhoanMatKhau(TaiKhoanMatKhauDTO taiKhoanMatKhauDTO)
        {
            var taiKhoanMatKhau = new TaiKhoanMatKhau
            {
                TaiKhoan = taiKhoanMatKhauDTO.TaiKhoan,
                MatKhau = taiKhoanMatKhauDTO.MatKhau,
                Role = string.IsNullOrEmpty(taiKhoanMatKhauDTO.Role) ? 3 : int.Parse(taiKhoanMatKhauDTO.Role) // Gán giá trị mặc định là 3 nếu Role không được cung cấp
            };

            _context.TaiKhoanMatKhaus.Add(taiKhoanMatKhau);
            _context.SaveChanges();

            taiKhoanMatKhauDTO.MaTaiKhoan = taiKhoanMatKhau.MaTaiKhoan; // Lưu lại ID mới
            return CreatedAtAction(nameof(GetTaiKhoanMatKhau), new { id = taiKhoanMatKhauDTO.MaTaiKhoan }, taiKhoanMatKhauDTO);
        }

        // PUT: api/TaiKhoanMatKhau/5
        [HttpPut("{id}")]
            public IActionResult PutTaiKhoanMatKhau(int id, TaiKhoanMatKhauDTO taiKhoanMatKhauDTO)
            {
                if (id != taiKhoanMatKhauDTO.MaTaiKhoan)
                {
                    return BadRequest();
                }

                var taiKhoanMatKhau = _context.TaiKhoanMatKhaus.Find(id);
                if (taiKhoanMatKhau == null)
                {
                    return NotFound();
                }

                taiKhoanMatKhau.TaiKhoan = taiKhoanMatKhauDTO.TaiKhoan;
                taiKhoanMatKhau.MatKhau = taiKhoanMatKhauDTO.MatKhau;
                taiKhoanMatKhau.Role = int.Parse(taiKhoanMatKhauDTO.Role);

                _context.SaveChanges();

                return NoContent();
            }

            // DELETE: api/TaiKhoanMatKhau/5
            [HttpDelete("{id}")]
            public IActionResult DeleteTaiKhoanMatKhau(int id)
            {
                var taiKhoanMatKhau = _context.TaiKhoanMatKhaus.Find(id);
                if (taiKhoanMatKhau == null)
                {
                    return NotFound();
                }

                _context.TaiKhoanMatKhaus.Remove(taiKhoanMatKhau);
                _context.SaveChanges();

                return NoContent();
            }
        }
    }
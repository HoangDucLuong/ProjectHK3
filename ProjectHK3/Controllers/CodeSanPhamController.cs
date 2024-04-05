using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectHK3.Models; // Đảm bảo thêm namespace của model
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
[Route("api/[controller]")]
[ApiController]
public class CodeSanPhamsController : ControllerBase
{
    private readonly ProjectHk3Context _codeSanPham;

    public CodeSanPhamsController(ProjectHk3Context context)
    {
        _codeSanPham = context;
    }

    // GET: api/CodeSanPhams
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CodeSanPham>>> GetCodeSanPhams()
    {
        return await _codeSanPham.CodeSanPhams.ToListAsync();
    }

    // GET: api/CodeSanPhams/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CodeSanPham>> GetCodeSanPham(int id)
    {
        var codeSanPham = await _codeSanPham.CodeSanPhams.FindAsync(id);

        if (codeSanPham == null)
        {
            return NotFound();
        }

        return codeSanPham;
    }

    // PUT: api/CodeSanPhams/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCodeSanPham(int id, CodeSanPham codeSanPham)
    {
        if (id != codeSanPham.MaCode)
        {
            return BadRequest();
        }

        _codeSanPham.Entry(codeSanPham).State = EntityState.Modified;

        try
        {
            await _codeSanPham.SaveChangesAsync();
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

    // POST: api/CodeSanPhams
    [HttpPost]
    public async Task<ActionResult<CodeSanPham>> PostCodeSanPham(CodeSanPham codeSanPham)
    {
        _codeSanPham.CodeSanPhams.Add(codeSanPham);
        await _codeSanPham.SaveChangesAsync();

        return CreatedAtAction("GetCodeSanPham", new { id = codeSanPham.MaCode }, codeSanPham);
    }

    // DELETE: api/CodeSanPhams/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCodeSanPham(int id)
    {
        var codeSanPham = await _codeSanPham.CodeSanPhams.FindAsync(id);
        if (codeSanPham == null)
        {
            return NotFound();
        }

        _codeSanPham.CodeSanPhams.Remove(codeSanPham);
        await _codeSanPham.SaveChangesAsync();

        return NoContent();
    }

    private bool CodeSanPhamExists(int id)
    {
        return _codeSanPham.CodeSanPhams.Any(e => e.MaCode == id);
    }
}

using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DentistasController(DentCareDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dentista>>> Listar() =>
        await db.Dentistas.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Dentista>> Find(int id)
    {
        var e = await db.Dentistas.FindAsync(id);
        
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Dentista>> Create(Dentista dto)
    {
        db.Dentistas.Add(dto);
        await db.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Find), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Dentista dto)
    {
        if (id != dto.Id) return BadRequest();
        
        db.Entry(dto).State = EntityState.Modified;
        
        await db.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Dentistas.FindAsync(id);
        
        if (e is null) return NotFound();
        
        db.Dentistas.Remove(e);
        await db.SaveChangesAsync();
        
        return NoContent();
    }
}

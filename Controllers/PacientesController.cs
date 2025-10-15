using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController(DentCareDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paciente>>> Index() =>
        await db.Pacientes.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Paciente>> Find(int id)
    {
        var e = await db.Pacientes.FindAsync(id);
        
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Paciente>> Create(Paciente dto)
    {
        db.Pacientes.Add(dto);
        await db.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Find), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Paciente dto)
    {
        if (id != dto.Id) return BadRequest();
        
        db.Entry(dto).State = EntityState.Modified;
        
        await db.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var e = await db.Pacientes.FindAsync(id);
        
        if (e is null) return NotFound();
        
        db.Pacientes.Remove(e);
        await db.SaveChangesAsync();
        
        return NoContent();
    }
}

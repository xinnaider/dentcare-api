using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TratamentosController(DentCareDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tratamento>>> Index() =>
        await db.Tratamentos.AsNoTracking().ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Tratamento>> Find(int id)
    {
        var e = await db.Tratamentos.FindAsync(id);
        
        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Tratamento>> Create(Tratamento dto)
    {
        db.Tratamentos.Add(dto);
        await db.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Find), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Tratamento dto)
    {
        if (id != dto.Id) return BadRequest();
        
        db.Entry(dto).State = EntityState.Modified;
        
        await db.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var e = await db.Tratamentos.FindAsync(id);
        
        if (e is null) return NotFound();
        
        db.Tratamentos.Remove(e);
        await db.SaveChangesAsync();
        
        return NoContent();
    }
}

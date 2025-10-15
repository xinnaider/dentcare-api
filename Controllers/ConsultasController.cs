using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController(DentCareDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Consulta>>> Index() =>
        await db.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Dentista)
            .Include(c => c.Itens).ThenInclude(i => i.Tratamento)
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Consulta>> Find(int id)
    {
        var e = await db.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Dentista)
            .Include(c => c.Itens).ThenInclude(i => i.Tratamento)
            .FirstOrDefaultAsync(c => c.Id == id);

        return e is null ? NotFound() : Ok(e);
    }

    [HttpPost]
    public async Task<ActionResult<Consulta>> Create(Consulta dto)
    {
        db.Consultas.Add(dto);
        await db.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Find), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Consulta dto)
    {
        if (id != dto.Id) return BadRequest();
        
        db.Entry(dto).State = EntityState.Modified;
        
        await db.SaveChangesAsync();
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var e = await db.Consultas.FindAsync(id);
        
        if (e is null) return NotFound();
        
        db.Consultas.Remove(e);
        await db.SaveChangesAsync();
        
        return NoContent();
    }

    public record AdicionarItemRequest(int TratamentoId, int Quantidade, decimal PrecoUnitario);

    [HttpPost("{id:int}/itens")]
    public async Task<ActionResult<ConsultaTratamento>> AdicionarItem(int id, AdicionarItemRequest req)
    {
        var consulta = await db.Consultas.FindAsync(id);
        
        if (consulta is null) return NotFound();

        var item = new ConsultaTratamento
        {
            ConsultaId = id,
            TratamentoId = req.TratamentoId,
            Quantidade = req.Quantidade,
            PrecoUnitario = req.PrecoUnitario
        };
        
        db.ConsultasTratamentos.Add(item);
        await db.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Find), new { id }, item);
    }

    [HttpDelete("{id:int}/itens/{itemId:int}")]
    public async Task<IActionResult> RemoverItem(int id, int itemId)
    {
        var item = await db.ConsultasTratamentos.FindAsync(itemId);
        
        if (item is null || item.ConsultaId != id) return NotFound();
        
        db.ConsultasTratamentos.Remove(item);
        await db.SaveChangesAsync();
        
        return NoContent();
    }
}

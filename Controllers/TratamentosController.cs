using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using DentCare.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TratamentosController : ControllerBase
{
    private readonly DentCareDbContext _db;

    public TratamentosController(DentCareDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TratamentoDTO>>> Get()
    {
        var tratamentos = await _db.Tratamentos
            .AsNoTracking()
            .Select(t => new TratamentoDTO
            {
                Id = t.Id,
                Nome = t.Nome,
                Preco = t.Preco
            })
            .ToListAsync();

        return Ok(tratamentos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TratamentoDTO>> GetById(int id)
    {
        var t = await _db.Tratamentos.FindAsync(id);
        if (t is null) return NotFound();

        var dto = new TratamentoDTO
        {
            Id = t.Id,
            Nome = t.Nome,
            Preco = t.Preco
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TratamentoDTO>> Create(TratamentoCreateDTO dto)
    {
        var t = new Tratamento
        {
            Nome = dto.Nome,
            Preco = dto.Preco
        };

        _db.Tratamentos.Add(t);
        await _db.SaveChangesAsync();

        var result = new TratamentoDTO
        {
            Id = t.Id,
            Nome = t.Nome,
            Preco = t.Preco
        };

        return CreatedAtAction(nameof(GetById), new { id = t.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TratamentoCreateDTO dto)
    {
        var t = await _db.Tratamentos.FindAsync(id);
        if (t is null) return NotFound();

        t.Nome = dto.Nome;
        t.Preco = dto.Preco;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Tratamentos.FindAsync(id);
        if (t is null) return NotFound();

        _db.Tratamentos.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
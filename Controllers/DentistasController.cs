using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using DentCare.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DentistasController : ControllerBase
{
    private readonly DentCareDbContext _db;

    public DentistasController(DentCareDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DentistaDTO>>> Get()
    {
        var dentistas = await _db.Dentistas
            .AsNoTracking()
            .Select(d => new DentistaDTO
            {
                Id = d.Id,
                NomeCompleto = d.NomeCompleto,
                CRO = d.CRO
            })
            .ToListAsync();

        return Ok(dentistas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DentistaDTO>> GetById(int id)
    {
        var d = await _db.Dentistas.FindAsync(id);
        if (d is null) return NotFound();

        var dto = new DentistaDTO
        {
            Id = d.Id,
            NomeCompleto = d.NomeCompleto,
            CRO = d.CRO
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<DentistaDTO>> Create(DentistaCreateDTO dto)
    {
        var d = new Dentista
        {
            NomeCompleto = dto.NomeCompleto,
            CRO = dto.CRO
        };

        _db.Dentistas.Add(d);
        await _db.SaveChangesAsync();

        var result = new DentistaDTO
        {
            Id = d.Id,
            NomeCompleto = d.NomeCompleto,
            CRO = d.CRO
        };

        return CreatedAtAction(nameof(GetById), new { id = d.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DentistaCreateDTO dto)
    {
        var d = await _db.Dentistas.FindAsync(id);
        if (d is null) return NotFound();

        d.NomeCompleto = dto.NomeCompleto;
        d.CRO = dto.CRO;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var d = await _db.Dentistas.FindAsync(id);
        if (d is null) return NotFound();

        _db.Dentistas.Remove(d);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
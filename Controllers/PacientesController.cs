using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using DentCare.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly DentCareDbContext _db;

    public PacientesController(DentCareDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteDTO>>> Get()
    {
        var pacientes = await _db.Pacientes
            .AsNoTracking()
            .Select(p => new PacienteDTO
            {
                Id = p.Id,
                NomeCompleto = p.NomeCompleto,
                DataNascimento = p.DataNascimento,
                Cpf = p.Cpf,
                Telefone = p.Telefone,
                Email = p.Email
            })
            .ToListAsync();

        return Ok(pacientes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PacienteDTO>> GetById(int id)
    {
        var p = await _db.Pacientes.FindAsync(id);
        if (p is null) return NotFound();

        var dto = new PacienteDTO
        {
            Id = p.Id,
            NomeCompleto = p.NomeCompleto,
            DataNascimento = p.DataNascimento,
            Cpf = p.Cpf,
            Telefone = p.Telefone,
            Email = p.Email
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<PacienteDTO>> Create(PacienteCreateDTO dto)
    {
        var p = new Paciente
        {
            NomeCompleto = dto.NomeCompleto,
            DataNascimento = dto.DataNascimento,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            Email = dto.Email
        };

        _db.Pacientes.Add(p);
        await _db.SaveChangesAsync();

        var result = new PacienteDTO
        {
            Id = p.Id,
            NomeCompleto = p.NomeCompleto,
            DataNascimento = p.DataNascimento,
            Cpf = p.Cpf,
            Telefone = p.Telefone,
            Email = p.Email
        };

        return CreatedAtAction(nameof(GetById), new { id = p.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PacienteCreateDTO dto)
    {
        var p = await _db.Pacientes.FindAsync(id);
        if (p is null) return NotFound();

        p.NomeCompleto = dto.NomeCompleto;
        p.DataNascimento = dto.DataNascimento;
        p.Cpf = dto.Cpf;
        p.Telefone = dto.Telefone;
        p.Email = dto.Email;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Pacientes.FindAsync(id);
        if (p is null) return NotFound();

        _db.Pacientes.Remove(p);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
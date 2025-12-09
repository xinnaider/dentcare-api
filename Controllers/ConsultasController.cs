using DentCare.Api.DataContexts;
using DentCare.Api.Models;
using DentCare.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController : ControllerBase
{
    private readonly DentCareDbContext _db;

    public ConsultasController(DentCareDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsultaDTO>>> Get()
    {
        var consultas = await _db.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Dentista)
            .Include(c => c.Itens).ThenInclude(i => i.Tratamento)
            .Select(c => new ConsultaDTO
            {
                Id = c.Id,
                DataHora = c.DataHora,
                Status = c.Status,
                Paciente = new PacienteDTO
                {
                    Id = c.Paciente.Id,
                    NomeCompleto = c.Paciente.NomeCompleto,
                    DataNascimento = c.Paciente.DataNascimento,
                    Cpf = c.Paciente.Cpf,
                    Telefone = c.Paciente.Telefone,
                    Email = c.Paciente.Email
                },
                Dentista = new DentistaDTO
                {
                    Id = c.Dentista.Id,
                    NomeCompleto = c.Dentista.NomeCompleto,
                    CRO = c.Dentista.CRO
                },
                Itens = c.Itens.Select(i => new ConsultaItemDTO
                {
                    Id = i.Id,
                    TratamentoId = i.TratamentoId,
                    TratamentoNome = i.Tratamento.Nome,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario
                }).ToList()
            })
            .ToListAsync();

        return Ok(consultas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConsultaDTO>> GetById(int id)
    {
        var c = await _db.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Dentista)
            .Include(c => c.Itens).ThenInclude(i => i.Tratamento)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (c is null) return NotFound();

        var dto = new ConsultaDTO
        {
            Id = c.Id,
            DataHora = c.DataHora,
            Status = c.Status,
            Paciente = new PacienteDTO
            {
                Id = c.Paciente.Id,
                NomeCompleto = c.Paciente.NomeCompleto,
                DataNascimento = c.Paciente.DataNascimento,
                Cpf = c.Paciente.Cpf,
                Telefone = c.Paciente.Telefone,
                Email = c.Paciente.Email
            },
            Dentista = new DentistaDTO
            {
                Id = c.Dentista.Id,
                NomeCompleto = c.Dentista.NomeCompleto,
                CRO = c.Dentista.CRO
            },
            Itens = c.Itens.Select(i => new ConsultaItemDTO
            {
                Id = i.Id,
                TratamentoId = i.TratamentoId,
                TratamentoNome = i.Tratamento.Nome,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<ConsultaDTO>> Create(ConsultaCreateDTO dto)
    {
        var consulta = new Consulta
        {
            PacienteId = dto.PacienteId,
            DentistaId = dto.DentistaId,
            DataHora = dto.DataHora
        };

        _db.Consultas.Add(consulta);
        await _db.SaveChangesAsync();

        return await GetById(consulta.Id);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ConsultaCreateDTO dto)
    {
        var c = await _db.Consultas.FindAsync(id);
        if (c is null) return NotFound();

        c.PacienteId = dto.PacienteId;
        c.DentistaId = dto.DentistaId;
        c.DataHora = dto.DataHora;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.Consultas.FindAsync(id);
        if (c is null) return NotFound();

        _db.Consultas.Remove(c);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id:int}/itens")]
    public async Task<ActionResult<ConsultaItemDTO>> AddItem(int id, ConsultaItemCreateDTO dto)
    {
        var consulta = await _db.Consultas.FindAsync(id);
        if (consulta is null) return NotFound();

        var item = new ConsultaTratamento
        {
            ConsultaId = id,
            TratamentoId = dto.TratamentoId,
            Quantidade = dto.Quantidade,
            PrecoUnitario = dto.PrecoUnitario
        };

        _db.ConsultasTratamentos.Add(item);
        await _db.SaveChangesAsync();

        var tratamento = await _db.Tratamentos.FindAsync(dto.TratamentoId);

        var result = new ConsultaItemDTO
        {
            Id = item.Id,
            TratamentoId = item.TratamentoId,
            TratamentoNome = tratamento?.Nome ?? string.Empty,
            Quantidade = item.Quantidade,
            PrecoUnitario = item.PrecoUnitario
        };

        return Ok(result);
    }

    [HttpDelete("{id:int}/itens/{itemId:int}")]
    public async Task<IActionResult> RemoveItem(int id, int itemId)
    {
        var item = await _db.ConsultasTratamentos.FindAsync(itemId);
        if (item is null || item.ConsultaId != id) return NotFound();

        _db.ConsultasTratamentos.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id:int}/finalizar")]
    public async Task<ActionResult<ConsultaDTO>> Finalizar(int id)
    {
        var c = await _db.Consultas.FindAsync(id);
        if (c is null) return NotFound();

        c.Status = "Concluida";
        await _db.SaveChangesAsync();

        return await GetById(id);
    }

    [HttpPut("{id:int}/cancelar")]
    public async Task<ActionResult<ConsultaDTO>> Cancelar(int id)
    {
        var c = await _db.Consultas.FindAsync(id);
        if (c is null) return NotFound();

        c.Status = "Cancelada";
        await _db.SaveChangesAsync();

        return await GetById(id);
    }
}
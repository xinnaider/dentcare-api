using System.ComponentModel.DataAnnotations;

namespace DentCare.Api.Models.DTOs;

public class ConsultaDTO
{
    public int Id { get; set; }

    [Required]
    public DateTime DataHora { get; set; }

    public required string Status { get; set; }

    public required PacienteDTO Paciente { get; set; }

    public required DentistaDTO Dentista { get; set; }

    public required List<ConsultaItemDTO> Itens { get; set; }
}

public class ConsultaCreateDTO
{
    [Required]
    public int PacienteId { get; set; }

    [Required]
    public int DentistaId { get; set; }

    [Required]
    public DateTime DataHora { get; set; }
}
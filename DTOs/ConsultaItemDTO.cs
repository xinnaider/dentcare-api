using System.ComponentModel.DataAnnotations;

namespace DentCare.Api.Models.DTOs;

public class ConsultaItemDTO
{
    public int Id { get; set; }

    [Required]
    public int TratamentoId { get; set; }

    public required string TratamentoNome { get; set; }

    [Required]
    public int Quantidade { get; set; }

    [Required]
    public decimal PrecoUnitario { get; set; }
}

public class ConsultaItemCreateDTO
{
    [Required]
    public int TratamentoId { get; set; }

    [Required]
    public int Quantidade { get; set; }

    [Required]
    public decimal PrecoUnitario { get; set; }
}
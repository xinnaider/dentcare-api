using System.ComponentModel.DataAnnotations;

namespace DentCare.Api.Models.DTOs;

public class TratamentoDTO
{
    public int Id { get; set; }

    [Required]
    public required string Nome { get; set; }

    [Required]
    public decimal Preco { get; set; }
}

public class TratamentoCreateDTO
{
    [Required]
    public required string Nome { get; set; }

    [Required]
    public decimal Preco { get; set; }
}
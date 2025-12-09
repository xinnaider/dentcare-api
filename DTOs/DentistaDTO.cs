using System.ComponentModel.DataAnnotations;

namespace DentCare.Api.Models.DTOs;

public class DentistaDTO
{
    public int Id { get; set; }

    [Required]
    public required string NomeCompleto { get; set; }

    [Required]
    public required string CRO { get; set; }
}

public class DentistaCreateDTO
{
    [Required]
    public required string NomeCompleto { get; set; }

    [Required]
    public required string CRO { get; set; }
}
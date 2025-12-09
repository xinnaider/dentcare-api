using System.ComponentModel.DataAnnotations;

namespace DentCare.Api.Models.DTOs;

public class PacienteDTO
{
    public int Id { get; set; }

    [Required]
    public required string NomeCompleto { get; set; }

    [Required]
    public DateTime DataNascimento { get; set; }

    [Required]
    public required string Cpf { get; set; }

    [Required]
    public required string Telefone { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}

public class PacienteCreateDTO
{
    [Required]
    public required string NomeCompleto { get; set; }

    [Required]
    public DateTime DataNascimento { get; set; }

    [Required]
    public required string Cpf { get; set; }

    [Required]
    public required string Telefone { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
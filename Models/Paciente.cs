namespace DentCare.Api.Models;

public class Paciente
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = default!;
    public DateTime DataNascimento { get; set; }
    public string Cpf { get; set; } = default!;
    public string Telefone { get; set; } = default!;
    public string Email { get; set; } = default!;

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}

namespace DentCare.Api.Models;

public class Dentista
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; } = default!;
    public string CRO { get; set; } = default!;

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}

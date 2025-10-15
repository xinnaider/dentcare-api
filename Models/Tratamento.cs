namespace DentCare.Api.Models;

public class Tratamento
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public decimal Preco { get; set; }

    public ICollection<ConsultaTratamento> Itens { get; set; } = new List<ConsultaTratamento>();
}

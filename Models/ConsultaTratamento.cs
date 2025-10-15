namespace DentCare.Api.Models;

public class ConsultaTratamento
{
    public int Id { get; set; }

    public int ConsultaId { get; set; }
    public Consulta Consulta { get; set; } = default!;

    public int TratamentoId { get; set; }
    public Tratamento Tratamento { get; set; } = default!;

    public int Quantidade { get; set; } = 1;
    public decimal PrecoUnitario { get; set; }
}

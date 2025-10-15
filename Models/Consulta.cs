namespace DentCare.Api.Models;

public class Consulta
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = "Agendada"; // Agendada, Concluida, Cancelada

    public int PacienteId { get; set; }
    public Paciente Paciente { get; set; } = default!;

    public int DentistaId { get; set; }
    public Dentista Dentista { get; set; } = default!;

    public ICollection<ConsultaTratamento> Itens { get; set; } = new List<ConsultaTratamento>();
}

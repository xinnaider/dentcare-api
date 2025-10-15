using DentCare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DentCare.Api.DataContexts;

public class DentCareDbContext(DbContextOptions<DentCareDbContext> options) : DbContext(options)
{
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Dentista> Dentistas => Set<Dentista>();
    public DbSet<Tratamento> Tratamentos => Set<Tratamento>();
    public DbSet<Consulta> Consultas => Set<Consulta>();
    public DbSet<ConsultaTratamento> ConsultasTratamentos => Set<ConsultaTratamento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ConsultaTratamento>()
            .HasOne(it => it.Consulta)
            .WithMany(c => c.Itens)
            .HasForeignKey(it => it.ConsultaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ConsultaTratamento>()
            .HasOne(it => it.Tratamento)
            .WithMany(t => t.Itens)
            .HasForeignKey(it => it.TratamentoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

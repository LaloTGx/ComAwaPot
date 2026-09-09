using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Data;

public class AppDbContext : DbContext
{
    public DbSet<Persona> Personas => Set<Persona>();

    public DbSet<HistorialSituacion> HistorialSituaciones => Set<HistorialSituacion>();

    public DbSet<Tarifa> Tarifas => Set<Tarifa>();

    public DbSet<PagoTarifa> PagosTarifa => Set<PagoTarifa>();

    public DbSet<DetallePagoTarifa> DetallesPagoTarifa => Set<DetallePagoTarifa>();

    public DbSet<AportacionExtraordinaria> AportacionesExtraordinarias => Set<AportacionExtraordinaria>();

    public DbSet<PagoAportacion> PagosAportacion => Set<PagoAportacion>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=ComAwaPot.db");
    }
}

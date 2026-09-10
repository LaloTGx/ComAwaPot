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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	    // Persona
	    modelBuilder.Entity<Persona>()
	        .HasMany(p => p.HistorialSituaciones)
	        .WithOne(h => h.Persona)
	        .HasForeignKey(h => h.IdPersona);
            .OnDelete(DeleteBehavior.Restrict);

	    modelBuilder.Entity<Persona>()
	        .HasMany(p => p.PagosTarifa)
	        .WithOne(p => p.Persona)
	        .HasForeignKey(p => p.IdPersona);
            .OnDelete(DeleteBehavior.Restrict);

	    modelBuilder.Entity<Persona>()
	        .HasMany(p => p.PagosAportacion)
	        .WithOne(p => p.Persona)
	        .HasForeignKey(p => p.IdPersona);
            .OnDelete(DeleteBehavior.Restrict);

	    // PagoTarifa
	    modelBuilder.Entity<PagoTarifa>()
	        .HasMany(p => p.Detalles)
	        .WithOne(d => d.PagoTarifa)
	        .HasForeignKey(d => d.IdPagoTarifa);
            .OnDelete(DeleteBehavior.Cascade);

	    // DetallePagoTarifa
	    modelBuilder.Entity<DetallePagoTarifa>()
	        .HasOne(d => d.Tarifa)
	        .WithMany()
	        .HasForeignKey(d => d.IdTarifa);
            .OnDelete(DeleteBehavior.Restrict);

	    // AportacionExtraordinaria
	    modelBuilder.Entity<AportacionExtraordinaria>()
	        .HasMany(a => a.Pagos)
	        .WithOne(p => p.AportacionExtraordinaria)
	        .HasForeignKey(p => p.IdAportacionExtraordinaria);
            .OnDelete(DeleteBehavior.Restrict);

	    // Índices de Persona
	    modelBuilder.Entity<Persona>()
	        .HasIndex(p => p.Nombre);

	    modelBuilder.Entity<Persona>()
	        .HasIndex(p => p.Calle);

	    // Tarifa: no puede haber dos tarifas para el mismo mes
	    modelBuilder.Entity<Tarifa>()
	        .HasIndex(t => t.Periodo)
	        .IsUnique();

	    // Precisión de montos
	    modelBuilder.Entity<Tarifa>()
	        .Property(t => t.MontoMensual)
	        .HasPrecision(18, 2);

	    modelBuilder.Entity<PagoTarifa>()
	        .Property(p => p.MontoTotal)
	        .HasPrecision(18, 2);

	    modelBuilder.Entity<DetallePagoTarifa>()
	        .Property(d => d.MontoAplicado)
	        .HasPrecision(18, 2);

	    modelBuilder.Entity<AportacionExtraordinaria>()
	        .Property(a => a.MontoTotal)
	        .HasPrecision(18, 2);

	    modelBuilder.Entity<AportacionExtraordinaria>()
	        .Property(a => a.MontoPorPersona)
	        .HasPrecision(18, 2);

	    modelBuilder.Entity<PagoAportacion>()
	        .Property(p => p.MontoPagado)
	        .HasPrecision(18, 2);
	}
}

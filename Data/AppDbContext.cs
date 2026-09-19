using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Persona> Personas => Set<Persona>();

    public DbSet<Toma> Tomas => Set<Toma>();

    public DbSet<HistorialSituacion> HistorialSituaciones => Set<HistorialSituacion>();

    public DbSet<Tarifa> Tarifas => Set<Tarifa>();

    public DbSet<TarifaToma> TarifaTomas => Set<TarifaToma>();

    public DbSet<PagoTarifa> PagosTarifa => Set<PagoTarifa>();

    public DbSet<DetallePagoTarifa> DetallesPagoTarifa => Set<DetallePagoTarifa>();

    public DbSet<AportacionExtraordinaria> AportacionesExtraordinarias => Set<AportacionExtraordinaria>();

    public DbSet<AportacionPersona> AportacionesPersonas => Set<AportacionPersona>();

    public DbSet<PagoAportacion> PagosAportacion => Set<PagoAportacion>();


    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlite("Data Source=ComAwaPot.db");
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Persona → Toma

        modelBuilder.Entity<Persona>()
            .HasMany(p => p.Tomas)
            .WithOne(t => t.Persona)
            .HasForeignKey(t => t.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Toma → HistorialSituacion

        modelBuilder.Entity<Toma>()
            .HasMany(t => t.HistorialSituaciones)
            .WithOne(h => h.Toma)
            .HasForeignKey(h => h.TomaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Toma → PagoTarifa

        modelBuilder.Entity<Toma>()
            .HasMany(t => t.PagosTarifa)
            .WithOne(p => p.Toma)
            .HasForeignKey(p => p.TomaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Toma → TarifaToma

        modelBuilder.Entity<Toma>()
            .HasMany(t => t.Tarifas)
            .WithOne(tt => tt.Toma)
            .HasForeignKey(tt => tt.TomaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Tarifa → TarifaToma

        modelBuilder.Entity<Tarifa>()
            .HasMany(t => t.Tomas)
            .WithOne(tt => tt.Tarifa)
            .HasForeignKey(tt => tt.TarifaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Persona → PagoAportacion

        modelBuilder.Entity<Persona>()
            .HasMany(p => p.PagosAportacion)
            .WithOne(p => p.Persona)
            .HasForeignKey(p => p.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Persona → AportacionPersona

        modelBuilder.Entity<Persona>()
            .HasMany<AportacionPersona>()
            .WithOne(ap => ap.Persona)
            .HasForeignKey(ap => ap.PersonaId)
            .OnDelete(DeleteBehavior.Restrict);

        // AportacionExtraordinaria → AportacionPersona

        modelBuilder.Entity<AportacionExtraordinaria>()
            .HasMany(a => a.Personas)
            .WithOne(ap => ap.AportacionExtraordinaria)
            .HasForeignKey(ap => ap.AportacionExtraordinariaId)
            .OnDelete(DeleteBehavior.Restrict);

        // PagoTarifa → DetallePagoTarifa

        modelBuilder.Entity<PagoTarifa>()
            .HasMany(p => p.Detalles)
            .WithOne(d => d.PagoTarifa)
            .HasForeignKey(d => d.PagoTarifaId)
            .OnDelete(DeleteBehavior.Cascade);

        // DetallePagoTarifa → Tarifa

        modelBuilder.Entity<DetallePagoTarifa>()
            .HasOne(d => d.Tarifa)
            .WithMany()
            .HasForeignKey(d => d.TarifaId)
            .OnDelete(DeleteBehavior.Restrict);

        // AportacionExtraordinaria → PagoAportacion

        modelBuilder.Entity<AportacionExtraordinaria>()
            .HasMany(a => a.Pagos)
            .WithOne(p => p.AportacionExtraordinaria)
            .HasForeignKey(p => p.AportacionExtraordinariaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índices de Persona

        modelBuilder.Entity<Persona>()
            .HasIndex(p => p.Nombre);

        // Toma: Índices

        modelBuilder.Entity<Toma>()
            .HasIndex(t => t.Calle);

        modelBuilder.Entity<Toma>()
            .HasIndex(t => t.NumeroContrato)
            .IsUnique();

        // Tarifa: una tarifa por tipo y periodo

        modelBuilder.Entity<Tarifa>()
            .HasIndex(t => new { t.Tipo, t.Periodo })
            .IsUnique();

        // TarifaToma: evitar asignaciones duplicadas

        modelBuilder.Entity<TarifaToma>()
            .HasIndex(tt => new { tt.TomaId, tt.TarifaId })
            .IsUnique();

        // AportacionPersona: evitar personas duplicadas

        modelBuilder.Entity<AportacionPersona>()
            .HasIndex(ap => new
            {
                ap.AportacionExtraordinariaId,
                ap.PersonaId
            })
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
            .Property(a => a.MontoPorPersona)
            .HasPrecision(18, 2);

        modelBuilder.Entity<AportacionPersona>()
            .Property(ap => ap.MontoEsperado)
            .HasPrecision(18, 2);

        modelBuilder.Entity<PagoAportacion>()
            .Property(p => p.MontoPagado)
            .HasPrecision(18, 2);
    }
}

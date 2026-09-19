using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class HistorialSituacionService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public HistorialSituacionService(
        DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    // Registrar situación inicial
    public async Task<HistorialSituacion?> CrearAsync(
        int tomaId,
        Situacion estado,
        DateTime fechaInicio)
    {
        using var db = new AppDbContext(_dbOptions);

        var toma = await db.Tomas
            .FirstOrDefaultAsync(t => t.TomaId == tomaId);

        if (toma is null)
        {
            return null;
        }

        // Verificar que no exista una situación actual
        var historialExiste = await db.HistorialSituaciones
            .AnyAsync(h =>
                h.TomaId == tomaId &&
                h.FechaFin == null
            );

        if (historialExiste)
        {
            return null;
        }

        var historial = new HistorialSituacion
        {
            TomaId = tomaId,
            Estado = estado,
            FechaInicio = fechaInicio,
            FechaFin = null
        };

        toma.Estado = estado;

        db.HistorialSituaciones.Add(historial);

        await db.SaveChangesAsync();

        return historial;
    }

    // Cambiar situación
    public async Task<HistorialSituacion?> CambiarSituacionAsync(
        int tomaId,
        Situacion nuevoEstado,
        DateTime fechaInicio)
    {
        using var db = new AppDbContext(_dbOptions);

        var toma = await db.Tomas
            .FirstOrDefaultAsync(t => t.TomaId == tomaId);

        if (toma is null)
        {
            return null;
        }

        var historialActual = await db.HistorialSituaciones
            .FirstOrDefaultAsync(h =>
                h.TomaId == tomaId &&
                h.FechaFin == null
            );

        if (historialActual is null)
        {
            return null;
        }

        if (historialActual.Estado == nuevoEstado)
        {
            return null;
        }

        historialActual.FechaFin = fechaInicio;

        var nuevoHistorial = new HistorialSituacion
        {
            TomaId = tomaId,
            Estado = nuevoEstado,
            FechaInicio = fechaInicio,
            FechaFin = null
        };

        toma.Estado = nuevoEstado;

        db.HistorialSituaciones.Add(nuevoHistorial);

        await db.SaveChangesAsync();

        return nuevoHistorial;
    }

    // Obtener historial de una toma
    public async Task<List<HistorialSituacion>> ObtenerPorTomaAsync(
        int tomaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.HistorialSituaciones
            .Where(h => h.TomaId == tomaId)
            .OrderBy(h => h.FechaInicio)
            .ToListAsync();
    }

    // Obtener situación actual
    public async Task<HistorialSituacion?> ObtenerActualAsync(
        int tomaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.HistorialSituaciones
            .FirstOrDefaultAsync(h =>
                h.TomaId == tomaId &&
                h.FechaFin == null
            );
    }
}

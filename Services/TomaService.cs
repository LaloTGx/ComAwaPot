using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class TomaService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public TomaService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    // Crear toma
    public async Task<Toma?> CrearAsync(
        Toma toma,
        DateTime? fechaInicioSituacion = null)
    {
        using var db = new AppDbContext(_dbOptions);

        // Verificar que la persona exista
        var personaExiste = await db.Personas
            .AnyAsync(p => p.PersonaId == toma.PersonaId);

        if (!personaExiste)
        {
            return null;
        }

        // Verificar que el contrato no exista
        var contratoExiste = await db.Tomas
            .AnyAsync(t => t.NumeroContrato == toma.NumeroContrato);

        if (contratoExiste)
        {
            return null;
        }

        db.Tomas.Add(toma);

        // Crear historial inicial
        db.HistorialSituaciones.Add(new HistorialSituacion
        {
            Toma = toma,
            Estado = toma.Estado,
            FechaInicio = fechaInicioSituacion ?? DateTime.Today,
            FechaFin = null
        });

        await db.SaveChangesAsync();

        return toma;
    }

    // Obtener toma por ID
    public async Task<Toma?> ObtenerPorIdAsync(int tomaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Tomas
            .Include(t => t.Persona)
            .FirstOrDefaultAsync(t => t.TomaId == tomaId);
    }

    // Obtener toma por contrato
    public async Task<Toma?> ObtenerPorContratoAsync(int numeroContrato)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Tomas
            .Include(t => t.Persona)
            .FirstOrDefaultAsync(
                t => t.NumeroContrato == numeroContrato
            );
    }

    // Obtener todas las tomas
    public async Task<List<Toma>> ObtenerTodasAsync()
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Tomas
            .Include(t => t.Persona)
            .OrderBy(t => t.NumeroContrato)
            .ToListAsync();
    }

    // Obtener tomas por persona
    public async Task<List<Toma>> ObtenerPorPersonaAsync(int personaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Tomas
            .Where(t => t.PersonaId == personaId)
            .OrderBy(t => t.NumeroContrato)
            .ToListAsync();
    }

    // Actualizar toma
    public async Task<bool> ActualizarAsync(Toma toma)
    {
        using var db = new AppDbContext(_dbOptions);

        var tomaExistente = await db.Tomas
            .FirstOrDefaultAsync(t => t.TomaId == toma.TomaId);

        if (tomaExistente is null)
        {
            return false;
        }

        // Verificar que otro contrato no use el mismo número
        var contratoExiste = await db.Tomas
            .AnyAsync(t =>
                t.NumeroContrato == toma.NumeroContrato &&
                t.TomaId != toma.TomaId
            );

        if (contratoExiste)
        {
            return false;
        }

        tomaExistente.NumeroContrato = toma.NumeroContrato;
        tomaExistente.Calle = toma.Calle;
        tomaExistente.NumExt = toma.NumExt;

        await db.SaveChangesAsync();

        return true;
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

        // No crear historial si la situación no cambia
        if (toma.Estado == nuevoEstado)
        {
            return null;
        }

        // Obtener el historial actual
        var historialActual = await db.HistorialSituaciones
            .FirstOrDefaultAsync(h =>
                h.TomaId == tomaId &&
                h.FechaFin == null
            );

        if (historialActual is null)
        {
            return null;
        }

        // Cerrar situación anterior
        historialActual.FechaFin = fechaInicio;

        // Crear nueva situación
        var nuevoHistorial = new HistorialSituacion
        {
            TomaId = tomaId,
            Estado = nuevoEstado,
            FechaInicio = fechaInicio,
            FechaFin = null
        };

        // Actualizar situación actual de la toma
        toma.Estado = nuevoEstado;

        db.HistorialSituaciones.Add(nuevoHistorial);

        await db.SaveChangesAsync();

        return nuevoHistorial;
    }
}

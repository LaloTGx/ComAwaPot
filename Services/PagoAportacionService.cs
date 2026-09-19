using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class PagoAportacionService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public PagoAportacionService(
        DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    // Registrar pago
    public async Task<PagoAportacion?> CrearAsync(
        int aportacionId,
        int personaId,
        decimal montoPagado,
        DateTime fechaPago,
        string? observaciones = null)
    {
        using var db = new AppDbContext(_dbOptions);

        // Verificar que la aportación exista
        var aportacionExiste = await db.AportacionesExtraordinarias
            .AnyAsync(a =>
                a.AportacionExtraordinariaId == aportacionId
            );

        if (!aportacionExiste)
        {
            return null;
        }

        // Verificar que la persona exista
        var personaExiste = await db.Personas
            .AnyAsync(p => p.PersonaId == personaId);

        if (!personaExiste)
        {
            return null;
        }

        // Verificar que la persona participe en la aportación
        var aportacionPersona = await db.AportacionesPersonas
            .FirstOrDefaultAsync(ap =>
                ap.AportacionExtraordinariaId == aportacionId &&
                ap.PersonaId == personaId
            );

        if (aportacionPersona is null)
        {
            return null;
        }

        // Verificar que no haya pagado anteriormente
        var pagoExiste = await db.PagosAportacion
            .AnyAsync(p =>
                p.AportacionExtraordinariaId == aportacionId &&
                p.PersonaId == personaId
            );

        if (pagoExiste)
        {
            return null;
        }

        // Verificar que el monto sea válido
        if (montoPagado <= 0)
        {
            return null;
        }

        var pago = new PagoAportacion
        {
            AportacionExtraordinariaId = aportacionId,
            PersonaId = personaId,
            MontoPagado = montoPagado,
            FechaPago = fechaPago,
            Observaciones = observaciones
        };

        db.PagosAportacion.Add(pago);

        await db.SaveChangesAsync();

        return pago;
    }

    // Obtener pago por ID
    public async Task<PagoAportacion?> ObtenerPorIdAsync(
        int pagoId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.PagosAportacion
            .Include(p => p.Persona)
            .Include(p => p.AportacionExtraordinaria)
            .FirstOrDefaultAsync(
                p => p.PagoAportacionId == pagoId
            );
    }

    // Obtener pagos de una aportación
    public async Task<List<PagoAportacion>> ObtenerPorAportacionAsync(
        int aportacionId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.PagosAportacion
            .Where(p =>
                p.AportacionExtraordinariaId == aportacionId
            )
            .Include(p => p.Persona)
            .OrderBy(p => p.FechaPago)
            .ToListAsync();
    }

    // Obtener pagos de una persona
    public async Task<List<PagoAportacion>> ObtenerPorPersonaAsync(
        int personaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.PagosAportacion
            .Where(p => p.PersonaId == personaId)
            .Include(p => p.AportacionExtraordinaria)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
    }

    // Verificar si una persona ya pagó
    public async Task<bool> EstaPagadoAsync(
        int aportacionId,
        int personaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.PagosAportacion
            .AnyAsync(p =>
                p.AportacionExtraordinariaId == aportacionId &&
                p.PersonaId == personaId
            );
    }
}

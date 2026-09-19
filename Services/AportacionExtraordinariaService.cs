using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class AportacionExtraordinariaService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public AportacionExtraordinariaService(
        DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    // Crear aportación
    public async Task<AportacionExtraordinaria?> CrearAsync(
        AportacionExtraordinaria aportacion)
    {
        using var db = new AppDbContext(_dbOptions);

        db.AportacionesExtraordinarias.Add(aportacion);

        await db.SaveChangesAsync();

        return aportacion;
    }

    // Obtener aportación por ID
    public async Task<AportacionExtraordinaria?> ObtenerPorIdAsync(
        int aportacionId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.AportacionesExtraordinarias
            .FirstOrDefaultAsync(
                a => a.AportacionExtraordinariaId == aportacionId
            );
    }

    // Obtener todas las aportaciones
    public async Task<List<AportacionExtraordinaria>> ObtenerTodasAsync()
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.AportacionesExtraordinarias
            .OrderByDescending(a => a.FechaCreacion)
            .ToListAsync();
    }

    // Agregar persona a una aportación
    public async Task<bool> AgregarPersonaAsync(
        int aportacionId,
        int personaId)
    {
        using var db = new AppDbContext(_dbOptions);

        // Verificar que la aportación exista
        var aportacionExiste = await db.AportacionesExtraordinarias
            .AnyAsync(a =>
                a.AportacionExtraordinariaId == aportacionId
            );

        if (!aportacionExiste)
        {
            return false;
        }

        // Verificar que la persona exista
        var personaExiste = await db.Personas
            .AnyAsync(p => p.PersonaId == personaId);

        if (!personaExiste)
        {
            return false;
        }

        // Verificar que no esté agregada
        var asignacionExiste = await db.AportacionesPersonas
            .AnyAsync(ap =>
                ap.AportacionExtraordinariaId == aportacionId &&
                ap.PersonaId == personaId
            );

        if (asignacionExiste)
        {
            return false;
        }

        var aportacion = await db.AportacionesExtraordinarias
            .FirstAsync(a =>
                a.AportacionExtraordinariaId == aportacionId
            );

        db.AportacionesPersonas.Add(new AportacionPersona
        {
            AportacionExtraordinariaId = aportacionId,
            PersonaId = personaId,
            MontoEsperado = aportacion.MontoPorPersona
        });

        await db.SaveChangesAsync();

        return true;
    }

    // Quitar persona de una aportación
    public async Task<bool> QuitarPersonaAsync(
        int aportacionId,
        int personaId)
    {
        using var db = new AppDbContext(_dbOptions);

        var asignacion = await db.AportacionesPersonas
            .FirstOrDefaultAsync(ap =>
                ap.AportacionExtraordinariaId == aportacionId &&
                ap.PersonaId == personaId
            );

        if (asignacion is null)
        {
            return false;
        }

        // No permitir quitar una persona que ya realizó un pago
        var pagoExiste = await db.PagosAportacion
            .AnyAsync(p =>
                p.AportacionExtraordinariaId == aportacionId &&
                p.PersonaId == personaId
            );

        if (pagoExiste)
        {
            return false;
        }

        db.AportacionesPersonas.Remove(asignacion);

        await db.SaveChangesAsync();

        return true;
    }

    // Obtener personas de una aportación
    public async Task<List<AportacionPersona>> ObtenerPersonasAsync(
        int aportacionId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.AportacionesPersonas
            .Where(ap =>
                ap.AportacionExtraordinariaId == aportacionId
            )
            .Include(ap => ap.Persona)
            .OrderBy(ap => ap.Persona.Nombre)
            .ToListAsync();
    }

    // Obtener monto esperado
    public async Task<decimal> ObtenerMontoEsperadoAsync(
        int aportacionId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.AportacionesPersonas
            .Where(ap =>
                ap.AportacionExtraordinariaId == aportacionId
            )
            .SumAsync(ap => ap.MontoEsperado);
    }

    // Obtener monto recaudado
    public async Task<decimal> ObtenerMontoRecaudadoAsync(
        int aportacionId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.PagosAportacion
            .Where(p =>
                p.AportacionExtraordinariaId == aportacionId
            )
            .SumAsync(p => p.MontoPagado);
    }
}

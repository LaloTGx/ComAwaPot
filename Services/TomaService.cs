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
    public async Task<Toma?> CrearAsync(Toma toma)
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
        tomaExistente.Estado = toma.Estado;

        await db.SaveChangesAsync();

        return true;
    }
}

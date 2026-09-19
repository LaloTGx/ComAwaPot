using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class TarifaTomaService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public TarifaTomaService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    // Asignar tarifa a una toma
    public async Task<bool> AsignarAsync(
        int tomaId,
        int tarifaId)
    {
        using var db = new AppDbContext(_dbOptions);

        var tomaExiste = await db.Tomas
            .AnyAsync(t => t.TomaId == tomaId);

        if (!tomaExiste)
        {
            return false;
        }

        var tarifaExiste = await db.Tarifas
            .AnyAsync(t => t.TarifaId == tarifaId);

        if (!tarifaExiste)
        {
            return false;
        }

        var asignacionExiste = await db.TarifaTomas
            .AnyAsync(tt =>
                tt.TomaId == tomaId &&
                tt.TarifaId == tarifaId
            );

        if (asignacionExiste)
        {
            return false;
        }

        db.TarifaTomas.Add(new TarifaToma
        {
            TomaId = tomaId,
            TarifaId = tarifaId
        });

        await db.SaveChangesAsync();

        return true;
    }

    // Obtener tarifas de una toma
    public async Task<List<Tarifa>> ObtenerPorTomaAsync(
        int tomaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.TarifaTomas
            .Where(tt => tt.TomaId == tomaId)
            .Select(tt => tt.Tarifa)
            .OrderBy(t => t.Periodo)
            .ToListAsync();
    }

    // Obtener tomas de una tarifa
    public async Task<List<Toma>> ObtenerPorTarifaAsync(
        int tarifaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Tomas
            .Where(t =>
                t.Tarifas.Any(tt => tt.TarifaId == tarifaId)
            )
            .Include(t => t.Persona)
            .OrderBy(t => t.NumeroContrato)
            .ToListAsync();
    }

    // Verificar asignación
    public async Task<bool> EstaAsignadaAsync(
        int tomaId,
        int tarifaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.TarifaTomas
            .AnyAsync(tt =>
                tt.TomaId == tomaId &&
                tt.TarifaId == tarifaId
            );
    }

    // Quitar tarifa de una toma
    public async Task<bool> QuitarAsync(
        int tomaId,
        int tarifaId)
    {
        using var db = new AppDbContext(_dbOptions);

        var asignacion = await db.TarifaTomas
            .FirstOrDefaultAsync(tt =>
                tt.TomaId == tomaId &&
                tt.TarifaId == tarifaId
            );

        if (asignacion is null)
        {
            return false;
        }

        db.TarifaTomas.Remove(asignacion);

        await db.SaveChangesAsync();

        return true;
    }
}

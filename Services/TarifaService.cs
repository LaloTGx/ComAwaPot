using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class TarifaService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public TarifaService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task<Tarifa?> CrearAsync(Tarifa tarifa)
    {
        using var db = new AppDbContext(_dbOptions);

        var existe = await db.Tarifas
            .AnyAsync(t =>
                t.Tipo == tarifa.Tipo &&
                t.Periodo == tarifa.Periodo
            );

        if (existe)
        {
            return null;
        }

        db.Tarifas.Add(tarifa);
        await db.SaveChangesAsync();

        return tarifa;
    }

    public async Task<Tarifa?> ObtenerPorPeriodoAsync(
        TipoTarifa tipo,
        DateTime periodo)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Tarifas
            .FirstOrDefaultAsync(t =>
                t.Tipo == tipo &&
                t.Periodo == periodo
            );
    }

    public async Task<List<Tarifa>> ObtenerTodasAsync()
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.Tarifas
            .OrderBy(t => t.Periodo)
            .ThenBy(t => t.Tipo)
            .ToListAsync();
    }
}

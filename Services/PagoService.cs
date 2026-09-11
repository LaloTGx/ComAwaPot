using ComAwaPot.Data;
using ComAwaPot.Models;
using Microsoft.EntityFrameworkCore;

namespace ComAwaPot.Services;

public class PagoService
{
    private readonly DbContextOptions<AppDbContext> _dbOptions;

    public PagoService(DbContextOptions<AppDbContext> dbOptions)
    {
        _dbOptions = dbOptions;
    }

    public async Task<PagoTarifa?> CrearPagoTarifaAsync(
	    int personaId,
	    DateTime fechaPago,
	    List<int> tarifasIds)
	{
	    using var db = new AppDbContext(_dbOptions);

	    // Verificar que las tarifas existan

	    var tarifas = await db.Tarifas
	        .Where(t => tarifasIds.Contains(t.TarifaId))
	        .OrderBy(t => t.Periodo)
	        .ToListAsync();

	    if (tarifas.Count != tarifasIds.Count)
	    {
	        return null;
	    }

	    // Verificar periodos ya pagados

	    var periodosPagados = await db.DetallesPagoTarifa
	        .Where(d =>
	            d.PagoTarifa.PersonaId == personaId &&
	            tarifasIds.Contains(d.TarifaId))
	        .Select(d => d.Periodo)
	        .ToListAsync();

	    if (periodosPagados.Count > 0)
	    {
	        return null;
	    }

	    // Crear pago

	    var pago = new PagoTarifa
	    {
	        PersonaId = personaId,
	        FechaPago = fechaPago,
	        MontoTotal = tarifas.Sum(t => t.MontoMensual)
	    };

	    // Crear detalles

	    foreach (var tarifa in tarifas)
	    {
	        pago.Detalles.Add(new DetallePagoTarifa
	        {
	            TarifaId = tarifa.TarifaId,
	            Periodo = tarifa.Periodo,
	            MontoAplicado = tarifa.MontoMensual
	        });
	    }

	    // Guardar

	    db.PagosTarifa.Add(pago);

	    await db.SaveChangesAsync();

	    return pago;
	}

    public async Task<PagoTarifa?> ObtenerPagoTarifaPorIdAsync(
        int pagoTarifaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.PagosTarifa
            .Include(p => p.Persona)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Tarifa)
            .FirstOrDefaultAsync(p => p.PagoTarifaId == pagoTarifaId);
    }
}

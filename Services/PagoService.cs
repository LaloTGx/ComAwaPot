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

    // Crear pago de tarifa

    public async Task<ResultadoPagoTarifa> CrearPagoTarifaAsync(
        int tomaId,
        DateTime fechaPago,
        List<int> tarifasIds)
    {
        using var db = new AppDbContext(_dbOptions);

        // Verificar que se hayan enviado tarifas

        if (tarifasIds.Count == 0)
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.SinTarifas
            };
        }

        // Verificar IDs de tarifas duplicados

        if (tarifasIds.Count != tarifasIds.Distinct().Count())
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.TarifaDuplicada
            };
        }

        // Verificar que la toma exista

        var toma = await db.Tomas
            .FirstOrDefaultAsync(t => t.TomaId == tomaId);

        if (toma is null)
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.TomaNoExiste
            };
        }

        // Verificar que la toma esté activa

        if (toma.Estado != Situacion.Activa)
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.TomaInactiva
            };
        }

        // Obtener tarifas

        var tarifas = await db.Tarifas
            .Where(t => tarifasIds.Contains(t.TarifaId))
            .OrderBy(t => t.Periodo)
            .ToListAsync();

        if (tarifas.Count != tarifasIds.Count)
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.TarifaNoExiste
            };
        }

        // Verificar que las tarifas estén
        // asignadas a la toma

        var tarifasAsignadasIds = await db.TarifaTomas
            .Where(tt =>
                tt.TomaId == tomaId &&
                tarifasIds.Contains(tt.TarifaId))
            .Select(tt => tt.TarifaId)
            .ToListAsync();

        if (tarifasAsignadasIds.Count != tarifasIds.Count)
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.TarifaNoAsignada
            };
        }

        // Verificar que no haya dos tarifas
        // para el mismo periodo

        var periodosDuplicados = tarifas
            .GroupBy(t => t.Periodo)
            .Any(g => g.Count() > 1);

        if (periodosDuplicados)
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.PeriodoDuplicado
            };
        }

        // Verificar periodos ya pagados

        var periodosSeleccionados = tarifas
            .Select(t => t.Periodo)
            .ToList();

        var periodosPagados = await db.DetallesPagoTarifa
            .Where(d =>
                d.PagoTarifa.TomaId == tomaId &&
                periodosSeleccionados.Contains(d.Periodo))
            .Select(d => d.Periodo)
            .ToListAsync();

        if (periodosPagados.Count > 0)
        {
            return new ResultadoPagoTarifa
            {
                Estado = ResultadoPago.PeriodoYaPagado
            };
        }

        // Crear pago

        var pago = new PagoTarifa
        {
            TomaId = tomaId,
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

        // Pago creado correctamente

        return new ResultadoPagoTarifa
        {
            Estado = ResultadoPago.Exitoso,
            Pago = pago
        };
    }

    // Obtener pago por ID

    public async Task<PagoTarifa?> ObtenerPagoTarifaPorIdAsync(
        int pagoTarifaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.PagosTarifa
            .Include(p => p.Toma)
                .ThenInclude(t => t.Persona)
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Tarifa)
            .FirstOrDefaultAsync(
                p => p.PagoTarifaId == pagoTarifaId
            );
    }

    // Obtener periodos pagados por toma

    public async Task<List<DetallePagoTarifa>> ObtenerPeriodosPagadosAsync(
        int tomaId)
    {
        using var db = new AppDbContext(_dbOptions);

        return await db.DetallesPagoTarifa
            .Where(d => d.PagoTarifa.TomaId == tomaId)
            .Include(d => d.Tarifa)
            .OrderBy(d => d.Periodo)
            .ToListAsync();
    }

    // Obtener estado de los periodos
    // para una toma

    public async Task<List<EstadoPeriodoInfo>> ObtenerEstadoPeriodosAsync(
        int tomaId)
    {
        using var db = new AppDbContext(_dbOptions);

        // Obtener la toma

        var toma = await db.Tomas
            .FirstOrDefaultAsync(t => t.TomaId == tomaId);

        if (toma is null)
        {
            return new List<EstadoPeriodoInfo>();
        }

        // Obtener tarifas asignadas a la toma

        var tarifas = await db.TarifaTomas
            .Where(tt => tt.TomaId == tomaId)
            .Select(tt => tt.Tarifa)
            .OrderBy(t => t.Periodo)
            .ToListAsync();

        // Obtener periodos pagados

        var periodosPagados = await db.DetallesPagoTarifa
            .Where(d => d.PagoTarifa.TomaId == tomaId)
            .Select(d => d.Periodo)
            .ToListAsync();

        // Determinar estado

        var primerDiaMesActual = new DateTime(
            DateTime.Today.Year,
            DateTime.Today.Month,
            1
        );

        var resultado = new List<EstadoPeriodoInfo>();

        foreach (var tarifa in tarifas)
        {
            EstadoPeriodo estado;

            if (periodosPagados.Contains(tarifa.Periodo))
            {
                estado = EstadoPeriodo.Pagado;
            }
            else if (tarifa.Periodo < primerDiaMesActual)
            {
                estado = EstadoPeriodo.Adeudo;
            }
            else
            {
                estado = EstadoPeriodo.Pendiente;
            }

            resultado.Add(new EstadoPeriodoInfo
            {
                Tarifa = tarifa,
                Estado = estado
            });
        }

        return resultado;
    }
}

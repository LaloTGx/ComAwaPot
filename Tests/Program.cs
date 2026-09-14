using ComAwaPot.Data;
using ComAwaPot.Models;
using ComAwaPot.Services;
using Microsoft.EntityFrameworkCore;


// ========================================
// Configuración
// ========================================

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite("Data Source=ComAwaPot.Tests.db")
    .Options;

var personaService = new PersonaService(options);
var tarifaService = new TarifaService(options);
var pagoService = new PagoService(options);


// ========================================
// Preparar base de datos de pruebas
// ========================================

using (var db = new AppDbContext(options))
{
    await db.Database.EnsureDeletedAsync();
    await db.Database.MigrateAsync();
}

Console.WriteLine("Base de datos de pruebas preparada.");


// ========================================
// Crear personas
// ========================================

var juan = await personaService.CrearAsync(new Persona
{
    Nombre = "Juan Pérez"
});

var ana = await personaService.CrearAsync(new Persona
{
    Nombre = "Ana López"
});

var carlos = await personaService.CrearAsync(new Persona
{
    Nombre = "Carlos Hernández"
});

Console.WriteLine();
Console.WriteLine("Personas creadas:");

Console.WriteLine(
    $"- {juan.PersonaId} - {juan.Nombre}"
);

Console.WriteLine(
    $"- {ana.PersonaId} - {ana.Nombre}"
);

Console.WriteLine(
    $"- {carlos.PersonaId} - {carlos.Nombre}"
);


// ========================================
// Obtener persona por ID
// ========================================

var personaEncontrada =
    await personaService.ObtenerPorIdAsync(juan.PersonaId);

Console.WriteLine();
Console.WriteLine("Persona recuperada desde SQLite:");

if (personaEncontrada is not null)
{
    Console.WriteLine($"ID: {personaEncontrada.PersonaId}");
    Console.WriteLine($"Nombre: {personaEncontrada.Nombre}");
}
else
{
    Console.WriteLine("No se encontró la persona.");
}


// ========================================
// Actualizar persona
// ========================================

juan.Nombre = "Juan Pérez Actualizado";

await personaService.ActualizarAsync(juan);

var juanActualizado =
    await personaService.ObtenerPorIdAsync(juan.PersonaId);

Console.WriteLine();
Console.WriteLine("Persona después de actualizar:");

if (juanActualizado is not null)
{
    Console.WriteLine(
        $"ID: {juanActualizado.PersonaId}"
    );

    Console.WriteLine(
        $"Nombre: {juanActualizado.Nombre}"
    );
}


// ========================================
// Crear tomas
// ========================================

// Juan tiene dos tomas:
// 1. Una doméstica
// 2. Una comercial

var tomaJuanDomestica = new Toma
{
    NumeroContrato = 1001,
    PersonaId = juan.PersonaId,
    Calle = "Hidalgo",
    NumExt = 15,
    Estado = Situacion.Activa,
    Tipo = TipoToma.Domestica
};

var tomaJuanComercial = new Toma
{
    NumeroContrato = 1002,
    PersonaId = juan.PersonaId,
    Calle = "Morelos",
    NumExt = 25,
    Estado = Situacion.Activa,
    Tipo = TipoToma.Comercial
};

// Ana tiene una toma doméstica.

var tomaAnaDomestica = new Toma
{
    NumeroContrato = 1003,
    PersonaId = ana.PersonaId,
    Calle = "Juárez",
    NumExt = 20,
    Estado = Situacion.Activa,
    Tipo = TipoToma.Domestica
};


// Guardar las tomas directamente.
// Todavía no tenemos TomaService.

using (var db = new AppDbContext(options))
{
    db.Tomas.AddRange(
        tomaJuanDomestica,
        tomaJuanComercial,
        tomaAnaDomestica
    );

    await db.SaveChangesAsync();
}

Console.WriteLine();
Console.WriteLine("Tomas creadas:");

Console.WriteLine(
    $"- Contrato {tomaJuanDomestica.NumeroContrato} | " +
    $"Juan | {tomaJuanDomestica.Tipo}"
);

Console.WriteLine(
    $"- Contrato {tomaJuanComercial.NumeroContrato} | " +
    $"Juan | {tomaJuanComercial.Tipo}"
);

Console.WriteLine(
    $"- Contrato {tomaAnaDomestica.NumeroContrato} | " +
    $"Ana | {tomaAnaDomestica.Tipo}"
);


// ========================================
// Crear tarifas domésticas
// ========================================

var tarifaDomesticaEnero = await tarifaService.CrearAsync(
    new Tarifa
    {
        Tipo = TipoToma.Domestica,
        Periodo = new DateTime(2026, 1, 1),
        MontoMensual = 100.00m
    }
);

var tarifaDomesticaFebrero = await tarifaService.CrearAsync(
    new Tarifa
    {
        Tipo = TipoToma.Domestica,
        Periodo = new DateTime(2026, 2, 1),
        MontoMensual = 100.00m
    }
);

var tarifaDomesticaMarzo = await tarifaService.CrearAsync(
    new Tarifa
    {
        Tipo = TipoToma.Domestica,
        Periodo = new DateTime(2026, 3, 1),
        MontoMensual = 120.00m
    }
);

var tarifaDomesticaSeptiembre = await tarifaService.CrearAsync(
    new Tarifa
    {
        Tipo = TipoToma.Domestica,
        Periodo = new DateTime(2026, 9, 1),
        MontoMensual = 120.00m
    }
);


// ========================================
// Crear tarifas comerciales
// ========================================

var tarifaComercialEnero = await tarifaService.CrearAsync(
    new Tarifa
    {
        Tipo = TipoToma.Comercial,
        Periodo = new DateTime(2026, 1, 1),
        MontoMensual = 180.00m
    }
);

var tarifaComercialFebrero = await tarifaService.CrearAsync(
    new Tarifa
    {
        Tipo = TipoToma.Comercial,
        Periodo = new DateTime(2026, 2, 1),
        MontoMensual = 180.00m
    }
);

Console.WriteLine();
Console.WriteLine("Tarifas creadas:");

foreach (var tarifa in new[]
{
    tarifaDomesticaEnero,
    tarifaDomesticaFebrero,
    tarifaDomesticaMarzo,
    tarifaDomesticaSeptiembre,
    tarifaComercialEnero,
    tarifaComercialFebrero
})
{
    if (tarifa is not null)
    {
        Console.WriteLine(
            $"{tarifa.Tipo} | " +
            $"{tarifa.Periodo:MM/yyyy} | " +
            $"${tarifa.MontoMensual:F2}"
        );
    }
}


// ========================================
// Comprobar que doméstica y comercial
// pueden coexistir en el mismo periodo
// ========================================

var tarifaComercialEneroEncontrada =
    await tarifaService.ObtenerPorPeriodoAsync(
        TipoToma.Comercial,
        new DateTime(2026, 1, 1)
    );

Console.WriteLine();
Console.WriteLine("Tarifa comercial de enero:");

if (tarifaComercialEneroEncontrada is not null)
{
    Console.WriteLine(
        $"{tarifaComercialEneroEncontrada.Tipo} | " +
        $"{tarifaComercialEneroEncontrada.Periodo:MM/yyyy} | " +
        $"${tarifaComercialEneroEncontrada.MontoMensual:F2}"
    );
}
else
{
    Console.WriteLine("No se encontró la tarifa.");
}


// ========================================
// Intentar crear tarifa duplicada
// ========================================

var tarifaDuplicada = await tarifaService.CrearAsync(
    new Tarifa
    {
        Tipo = TipoToma.Domestica,
        Periodo = new DateTime(2026, 3, 1),
        MontoMensual = 150.00m
    }
);

Console.WriteLine();
Console.WriteLine("Intentar crear tarifa doméstica duplicada:");

if (tarifaDuplicada is null)
{
    Console.WriteLine(
        "Correcto: no se creó porque ya existe " +
        "una tarifa doméstica para marzo de 2026."
    );
}
else
{
    Console.WriteLine(
        "ERROR: se creó una tarifa duplicada."
    );
}


// ========================================
// Obtener todas las tarifas
// ========================================

var tarifas =
    await tarifaService.ObtenerTodasAsync();

Console.WriteLine();
Console.WriteLine("Todas las tarifas:");

foreach (var tarifa in tarifas)
{
    Console.WriteLine(
        $"{tarifa.Tipo} | " +
        $"{tarifa.Periodo:MM/yyyy} | " +
        $"${tarifa.MontoMensual:F2}"
    );
}


// ========================================
// Pago de Juan - toma doméstica
// Enero + febrero
// ========================================

var resultadoPago =
    await pagoService.CrearPagoTarifaAsync(
        tomaJuanDomestica.TomaId,
        new DateTime(2026, 3, 10),
        new List<int>
        {
            tarifaDomesticaEnero!.TarifaId,
            tarifaDomesticaFebrero!.TarifaId
        }
    );

Console.WriteLine();
Console.WriteLine(
    "Pago de Juan - toma doméstica:"
);

if (resultadoPago.Exitoso)
{
    var pago = resultadoPago.Pago!;

    Console.WriteLine(
        "Pago creado correctamente."
    );

    Console.WriteLine(
        $"ID: {pago.PagoTarifaId}"
    );

    Console.WriteLine(
        $"Toma ID: {pago.TomaId}"
    );

    Console.WriteLine(
        $"Fecha: {pago.FechaPago:dd/MM/yyyy}"
    );

    Console.WriteLine(
        $"Monto total: ${pago.MontoTotal:F2}"
    );
}
else
{
    Console.WriteLine(
        $"ERROR: {resultadoPago.Estado}"
    );
}


// ========================================
// Obtener pago por ID
// ========================================

if (resultadoPago.Pago is not null)
{
    var pagoEncontrado =
        await pagoService.ObtenerPagoTarifaPorIdAsync(
            resultadoPago.Pago.PagoTarifaId
        );

    Console.WriteLine();
    Console.WriteLine(
        "Pago recuperado desde SQLite:"
    );

    if (pagoEncontrado is not null)
    {
        Console.WriteLine(
            $"ID: {pagoEncontrado.PagoTarifaId}"
        );

        Console.WriteLine(
            $"Persona: " +
            $"{pagoEncontrado.Toma.Persona.Nombre}"
        );

        Console.WriteLine(
            $"Contrato: " +
            $"{pagoEncontrado.Toma.NumeroContrato}"
        );

        Console.WriteLine(
            $"Tipo de toma: " +
            $"{pagoEncontrado.Toma.Tipo}"
        );

        Console.WriteLine(
            $"Fecha: " +
            $"{pagoEncontrado.FechaPago:dd/MM/yyyy}"
        );

        Console.WriteLine(
            $"Monto total: " +
            $"${pagoEncontrado.MontoTotal:F2}"
        );

        Console.WriteLine();
        Console.WriteLine("Detalles del pago:");

        foreach (var detalle in pagoEncontrado.Detalles)
        {
            Console.WriteLine(
                $"{detalle.Periodo:MM/yyyy} - " +
                $"${detalle.MontoAplicado:F2}"
            );
        }
    }
    else
    {
        Console.WriteLine(
            "No se encontró el pago."
        );
    }
}


// ========================================
// Intentar pagar enero nuevamente
// ========================================

var resultadoDuplicado =
    await pagoService.CrearPagoTarifaAsync(
        tomaJuanDomestica.TomaId,
        new DateTime(2026, 3, 11),
        new List<int>
        {
            tarifaDomesticaEnero!.TarifaId
        }
    );

Console.WriteLine();
Console.WriteLine(
    "Intentar pagar enero nuevamente:"
);

if (resultadoDuplicado.Exitoso)
{
    Console.WriteLine(
        "ERROR: el pago duplicado fue creado."
    );
}
else
{
    Console.WriteLine(
        $"Correcto: {resultadoDuplicado.Estado}"
    );
}


// ========================================
// Intentar pagar tarifa comercial
// con una toma doméstica
// ========================================

var resultadoTipoIncorrecto =
    await pagoService.CrearPagoTarifaAsync(
        tomaJuanDomestica.TomaId,
        new DateTime(2026, 3, 12),
        new List<int>
        {
            tarifaComercialEnero!.TarifaId
        }
    );

Console.WriteLine();
Console.WriteLine(
    "Intentar pagar tarifa comercial " +
    "con toma doméstica:"
);

if (resultadoTipoIncorrecto.Exitoso)
{
    Console.WriteLine(
        "ERROR: se permitió una tarifa incorrecta."
    );
}
else
{
    Console.WriteLine(
        $"Correcto: {resultadoTipoIncorrecto.Estado}"
    );
}


// ========================================
// Pago de Juan - toma comercial
// ========================================

var resultadoPagoComercial =
    await pagoService.CrearPagoTarifaAsync(
        tomaJuanComercial.TomaId,
        new DateTime(2026, 3, 13),
        new List<int>
        {
            tarifaComercialEnero!.TarifaId,
            tarifaComercialFebrero!.TarifaId
        }
    );

Console.WriteLine();
Console.WriteLine(
    "Pago de Juan - toma comercial:"
);

if (resultadoPagoComercial.Exitoso)
{
    Console.WriteLine(
        "Pago comercial creado correctamente."
    );

    Console.WriteLine(
        $"Monto total: " +
        $"${resultadoPagoComercial.Pago!.MontoTotal:F2}"
    );
}
else
{
    Console.WriteLine(
        $"ERROR: {resultadoPagoComercial.Estado}"
    );
}


// ========================================
// Intentar pagar con toma inexistente
// ========================================

var resultadoTomaInexistente =
    await pagoService.CrearPagoTarifaAsync(
        999,
        new DateTime(2026, 3, 14),
        new List<int>
        {
            tarifaDomesticaMarzo!.TarifaId
        }
    );

Console.WriteLine();
Console.WriteLine(
    "Intentar pagar con toma inexistente:"
);

if (resultadoTomaInexistente.Exitoso)
{
    Console.WriteLine(
        "ERROR: se creó un pago."
    );
}
else
{
    Console.WriteLine(
        $"Correcto: {resultadoTomaInexistente.Estado}"
    );
}


// ========================================
// Dar de baja temporal la toma de Ana
// ========================================

tomaAnaDomestica.Estado =
    Situacion.BajaTemporal;

using (var db = new AppDbContext(options))
{
    db.Tomas.Update(tomaAnaDomestica);
    await db.SaveChangesAsync();
}

Console.WriteLine();
Console.WriteLine(
    "Toma de Ana cambiada a BajaTemporal."
);


// ========================================
// Intentar pagar con toma inactiva
// ========================================

var resultadoTomaInactiva =
    await pagoService.CrearPagoTarifaAsync(
        tomaAnaDomestica.TomaId,
        new DateTime(2026, 3, 15),
        new List<int>
        {
            tarifaDomesticaMarzo!.TarifaId
        }
    );

Console.WriteLine();
Console.WriteLine(
    "Intentar pagar con toma en BajaTemporal:"
);

if (resultadoTomaInactiva.Exitoso)
{
    Console.WriteLine(
        "ERROR: se permitió el pago."
    );
}
else
{
    Console.WriteLine(
        $"Correcto: {resultadoTomaInactiva.Estado}"
    );
}


// ========================================
// Obtener periodos pagados
// de la toma doméstica de Juan
// ========================================

var periodosPagados =
    await pagoService.ObtenerPeriodosPagadosAsync(
        tomaJuanDomestica.TomaId
    );

Console.WriteLine();
Console.WriteLine(
    "Periodos pagados por la toma doméstica de Juan:"
);

foreach (var detalle in periodosPagados)
{
    Console.WriteLine(
        $"{detalle.Periodo:MM/yyyy} - " +
        $"${detalle.MontoAplicado:F2}"
    );
}


// ========================================
// Obtener estado de periodos
// de la toma doméstica de Juan
// ========================================

var estados =
    await pagoService.ObtenerEstadoPeriodosAsync(
        tomaJuanDomestica.TomaId
    );

Console.WriteLine();
Console.WriteLine(
    "Estado de los periodos de la toma doméstica de Juan:"
);

foreach (var estado in estados)
{
    Console.WriteLine(
        $"{estado.Tarifa.Periodo:MM/yyyy} - " +
        $"${estado.Tarifa.MontoMensual:F2} - " +
        $"{estado.Estado}"
    );
}

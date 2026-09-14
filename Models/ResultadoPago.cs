namespace ComAwaPot.Models;

public enum ResultadoPago
{
    Exitoso,
    TomaNoExiste,
    TomaInactiva,
    TarifaNoExiste,
    TarifaNoAsignada,
    TarifaDuplicada,
    PeriodoDuplicado,
    PeriodoYaPagado,
    SinTarifas
}

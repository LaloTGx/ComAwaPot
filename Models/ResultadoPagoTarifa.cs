namespace ComAwaPot.Models;

public class ResultadoPagoTarifa
{
    public ResultadoPago Estado { get; set; }

    public PagoTarifa? Pago { get; set; }

    public bool Exitoso =>
        Estado == ResultadoPago.Exitoso;
}

namespace ManejoPresupuestos.Models
{
    public class IndiceCuentasViewModel
    {
        public string TipoCuenta { get; set; }
        public IEnumerable<Cuenta> Cuentas { get; set; }
        public decimal balance => Cuentas.Sum(x => x.Balance);
    
    }
}

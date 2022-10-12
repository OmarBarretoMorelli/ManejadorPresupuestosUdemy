using ManejoPresupuestos.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuestos.Models
{
    public class Cuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength: 50)]
        public string Nombre { get; set; }

        [Display(Name = "Tipo de cuenta")]
        public int TipoCuentaId { get; set; }
        public decimal Balance { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Descripcion { get; set; }
        public string TipoCuenta { get; set; }
    }
}

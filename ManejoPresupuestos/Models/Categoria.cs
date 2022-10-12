using ManejoPresupuestos.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuestos.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [PrimeraLetraMayuscula(ErrorMessage = "Debe iniciar con una letra mayuscula.")]
        [StringLength(maximumLength:50, ErrorMessage = "El largo maximo es de {1} caracteres.")]
        public string Nombre { get; set; }
        [Display(Name = "Tipo operacion")]
        public TipoOperacion TipoOperacionId { get; set; }
        public int UsuarioId { get; set; }
    }
}

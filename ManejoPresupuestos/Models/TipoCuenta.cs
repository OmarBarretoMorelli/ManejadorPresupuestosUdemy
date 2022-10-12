using ManejoPresupuestos.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuestos.Models
{
    public class TipoCuenta
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre no puede ser vacio.")]
        [PrimeraLetraMayuscula]
        [Remote(action: "VerificarExistenciaTipoCuenta", controller: "TipoCuentas")]
        public string Nombre { get; set; }

        public int UsuarioId { get; set; }

        public int Orden { get; set; }
    }
}

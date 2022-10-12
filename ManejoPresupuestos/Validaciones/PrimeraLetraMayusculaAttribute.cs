using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuestos.Validaciones
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString();
            if (primeraLetra.Equals(primeraLetra.ToUpper()))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("La primera letra debe ser mayúscula.");



        }
    }
}

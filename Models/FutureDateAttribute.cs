using System.ComponentModel.DataAnnotations;

namespace GestaoVoluntariado.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date <= DateTime.Now)
                {
                    return new ValidationResult(ErrorMessage ?? "A data deve ser futura");
                }
            }
            return ValidationResult.Success;
        }
    }
}
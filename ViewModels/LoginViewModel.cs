using System.ComponentModel.DataAnnotations; // Necessário para [Required]

namespace GestaoVoluntariado.ViewModels // Verifique se o namespace é o nome do seu projeto
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;
    }
}
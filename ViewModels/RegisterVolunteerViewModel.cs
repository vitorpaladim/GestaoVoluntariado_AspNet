using System.ComponentModel.DataAnnotations;

namespace GestaoVoluntariado.ViewModels
{
    public class RegisterVolunteerViewModel
    {
        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [Display(Name = "Nome Completo")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public int OpportunityId { get; set; }
    }
}
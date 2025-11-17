using System.ComponentModel.DataAnnotations;

namespace GestaoVoluntariado.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email fornecido não é válido.")]
        public string Email { get; set; } = string.Empty;

        
        public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; } = new List<VolunteerOpportunity>();
    }
}

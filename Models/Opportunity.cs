using System.ComponentModel.DataAnnotations;

namespace GestaoVoluntariado.Models
{
    public class Opportunity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Título é obrigatório")]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Data é obrigatória")]
        [FutureDate(ErrorMessage = "A data da oportunidade deve ser futura")] 
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Organização é obrigatória")]
        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; } = new List<VolunteerOpportunity>();
    }
}
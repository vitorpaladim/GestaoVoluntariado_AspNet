namespace GestaoVoluntariado.Models
{
    public class Opportunity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int OrganizationId { get; set; }

        // Navigation properties
        public Organization? Organization { get; set; }
        public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; } = new List<VolunteerOpportunity>();
    }
}

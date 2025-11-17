namespace GestaoVoluntariado.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation property
        public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; } = new List<VolunteerOpportunity>();
    }
}

namespace GestaoVoluntariado.Models
{
    public class VolunteerOpportunity
    {
        public int VolunteerId { get; set; }
        public int OpportunityId { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.Now;

  
        public Volunteer? Volunteer { get; set; }
        public Opportunity? Opportunity { get; set; }
    }
}

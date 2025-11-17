namespace GestaoVoluntariado.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    }
}

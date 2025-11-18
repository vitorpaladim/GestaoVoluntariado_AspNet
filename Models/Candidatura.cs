namespace GestaoVoluntariado.Models
{
    public class Candidatura
    {
        public int Id { get; set; }
        public int ProjetoId { get; set; }
        public string VoluntarioId { get; set; } = string.Empty;
        public string Status { get; set; } = "Pendente"; // "Pendente", "Aceita", "Recusada"
        public DateTime DataCandidatura { get; set; } = DateTime.Now;

        // Relacionamentos
        public Projeto? Projeto { get; set; }
        public Voluntario? Voluntario { get; set; }
    }
}

namespace GestaoVoluntariado.Models
{
    public class Voluntario : ApplicationUser
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? FotoPath { get; set; }
        public string? CurriculoPath { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Relacionamento com candidaturas
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    }
}

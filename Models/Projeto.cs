namespace GestaoVoluntariado.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string OngId { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public int TotalVagas { get; set; }
        public int Views { get; set; } = 0;
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Relacionamentos
        public ONG? ONG { get; set; }
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    }
}

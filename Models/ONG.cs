namespace GestaoVoluntariado.Models
{
    public class ONG : ApplicationUser
    {
        public string NomeONG { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? DocumentoPath { get; set; }
        public string Status { get; set; } = "Pendente"; // "Pendente" ou "Validada"
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Relacionamento com projetos
        public ICollection<Projeto> Projetos { get; set; } = new List<Projeto>();
    }
}

namespace DeskFlow.API.Models.Entities
{
    // NOTA: Essa classe vai virar a tabela central do sistema.
    public class Chamado
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        
        public string Prioridade { get; set; } 
        
        public string Status { get; set; } 
        
        public string SolicitanteNome { get; set; }
        public DateTime DataAbertura { get; set; }
        
        // NOTA: DataFechamento e Solucao podem ser nulos (?) porque o chamado nasce aberto.
        public DateTime? DataFechamento { get; set; }
        public string? Solucao { get; set; }

        // NOTA: Foreign Key (Chave Estrangeira). O Entity Framework entende que esse campo aponta para a tabela Categoria.
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // NOTA: Relacionamento 1:N com Interacoes (um chamado tem vários comentários).
        public List<Interacao> Interacoes { get; set; } = new List<Interacao>();
    }
}
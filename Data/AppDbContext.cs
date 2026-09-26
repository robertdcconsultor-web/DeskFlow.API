using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

// NOTA: Essa classe herda de DbContext. O DbContext é o "motor" do Entity Framework que faz a ponte entre o C# e o SQL Server.
namespace DeskFlow.API.Data
{
    public class AppDbContext : DbContext
    {
        // NOTA: O DbSet representa a tabela real no banco de dados. Aqui criamos a tabela de Categorias.
        public DbSet<Categoria> Categorias { get; set; }

        // NOTA: O construtor recebe as configurações do banco e passa para a classe base (DbContext).
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // NOTA: O método OnConfiguring é executado para configurar o banco. Coloquei a string de conexão direto aqui no código porque foi mais fácil para fazer os testes locais rodarem rápido.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories
{
    // NOTA: Repositório exclusivo para as interações. Mentém as responsabilidades separadas.
    public class InteracaoRepository
    {
        private readonly AppDbContext _context;

        public InteracaoRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Adicionar(Interacao interacao)
        {
            _context.Interacoes.Add(interacao);
            _context.SaveChanges();
        }
    }
}
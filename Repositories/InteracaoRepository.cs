using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using System.Threading.Tasks; //NOTA: Necessário para usar o Task

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

        //CORREÇÃO: Método agora retorna TASK e usa o sufixo Async
        public async Task AdicionarAsync(Interacao interacao)
        {
            //NOTA: AddAsync  SaveChangesAsync liberam a thread do servidor
            await _context.Interacoes.AddAsync(interacao);
            await _context.SaveChangesAsync();
        }
    }
}
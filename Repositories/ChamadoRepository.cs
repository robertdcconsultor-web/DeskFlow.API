using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore; //Nota: Necessário para usar o Include() no EF Core.
using System.Linq; //Nota: Necessário para o IQueryable.
using System.Threading.Tasks;

namespace DeskFlow.API.Repositories
{
    // NOTA: Este repositório centraliza os comandos SQL para a entidade principal do projeto.
    public class ChamadoRepository
    {
        private readonly AppDbContext _context;

        public ChamadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Chamado chamado)
        {
            await _context.Chamados.AddAsync(chamado);
            await _context.SaveChangesAsync();
        }

        public async Task<Chamado> ObterPorIdAsync(int id)
        {
            // O RF11 exige que o chamado venha com sua Categoria e Histórico de Interações.
            return await _context.Chamados
                .Include(c => c.Categoria)
                .Include(c => c.Interacoes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AtualizarAsync(Chamado chamado)
        {
            // NOTA: O Update marca o registo como modificado para o EF Core atualizar todos os campos no banco. Não tem versão Async
            _context.Chamados.Update(chamado);
            // O SaveChanges é quem efetivamente dispara o comando SQL, por isso ele é "awaitable".
            await _context.SaveChangesAsync();
        }

        // NOTA: O IQueryable não vai no banco na hora. Ele é apenas a "planta baixa" da query SQL.
        public IQueryable<Chamado> ObterQueryable()
        {
            return _context.Chamados.AsQueryable();
        }
    }
}
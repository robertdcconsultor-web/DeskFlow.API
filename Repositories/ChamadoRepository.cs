using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

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

        public void Adicionar(Chamado chamado)
        {
            _context.Chamados.Add(chamado);
            _context.SaveChanges();
        }

        public Chamado ObterPorId(int id)
        {
            return _context.Chamados.FirstOrDefault(c => c.Id == id);
        }

        public void Atualizar(Chamado chamado)
        {
            // NOTA: O Update marca o registo como modificado para o EF Core atualizar todos os campos no banco.
            _context.Chamados.Update(chamado);
            _context.SaveChanges();
        }

        public List<Chamado> ObterTodos()
        {
            return _context.Chamados.ToList();
        }
    }
}
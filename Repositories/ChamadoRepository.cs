using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore; //Nota: Necessário para usar o Include() no EF Core.
using System.Linq; //Nota: Necessário para o IQueryable.

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
            // O RF11 exige que o chamado venha com sua Categoria e Histórico de Interações.
            return _context.Chamados
                .Include(c => c.Categoria)
                .Include(c => c.Interacoes)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Atualizar(Chamado chamado)
        {
            // NOTA: O Update marca o registo como modificado para o EF Core atualizar todos os campos no banco.
            _context.Chamados.Update(chamado);
            _context.SaveChanges();
        }

        // NOTA SÊNIOR: O IQueryable não vai no banco na hora. Ele é apenas a "planta baixa" da query SQL.
        public IQueryable<Chamado> ObterQueryable()
        {
            return _context.Chamados.AsQueryable();
        }
    }
}
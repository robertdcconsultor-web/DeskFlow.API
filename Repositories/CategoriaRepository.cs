using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;
using Microsoft.EntityFrameworkCore; //Nota: Necessário para usar o Include() no EF Core e ToListAsync.
using System.Linq; //Nota: Necessário para o IQueryable.
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeskFlow.API.Repositories
{
    // NOTA: O repositório serve para centralizar todos os comandos de banco de dados num lugar só. Assim não espalhamos SQL pelo projeto todo.
    public class CategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            // NOTA: Aqui estamos recebendo o contexto do banco para poder usar nos métodos.
            _context = context;
        }

        //CORREÇÃO: Transição para o modelo Assincrono
        public async Task<List<Categoria>> ObterTodasAsync()
        {
            // NOTA: O ToList() vai lá no banco, faz um SELECT * e transforma numa lista do C#.
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria?> ObterPorIdAsync(int id)
        {
            // NOTA: O Find procura pelo Id. Se não achar, retorna null. Precisamos do Include (c => c.Chamados)para que a validação de exclusão no Service consiga contar se existem chamados vinculados.
            return await _context.Categorias
                .Include(c => c.Chamados)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AdicionarAsync(Categoria categoria)
        {
            // NOTA: O Add só coloca na memória do Entity Framework.
            await _context.Categorias.AddAsync(categoria);
            // NOTA: O SaveChanges é o que realmente faz o INSERT no banco de dados. Sem ele, nada é salvo!
            await _context.SaveChangesAsync();
        }

        public async Task DeletarAsync(Categoria categoria)
        {
            // NOTA: O método Remove do EF Core é apenas de memória (síncrono), a ida ao banco acontece apenas no SaveChangesAsync.
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
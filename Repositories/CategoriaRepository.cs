using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

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

        public List<Categoria> ObterTodas()
        {
            // NOTA: O ToList() vai lá no banco, faz um SELECT * e transforma numa lista do C#.
            return _context.Categorias.ToList();
        }

        public Categoria ObterPorId(int id)
        {
            // NOTA: O Find procura pelo Id. Se não achar, retorna null.
            return _context.Categorias.Find(id);
        }

        public void Adicionar(Categoria categoria)
        {
            // NOTA: O Add só coloca na memória do Entity Framework.
            _context.Categorias.Add(categoria);
            // NOTA: O SaveChanges é o que realmente faz o INSERT no banco de dados. Sem ele, nada é salvo!
            _context.SaveChanges();
        }

        public void Deletar(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
        }
    }
}
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services
{
    // NOTA: O Service é o "cérebro" da operação. Ele recebe o pedido da Controller, aplica as regras de negócio rigorosas e só depois manda o Repository executar no banco.
    public class CategoriaService
    {
        private readonly CategoriaRepository _repository;

        public CategoriaService(CategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<Categoria> ObterTodas()
        {
            return _repository.ObterTodas();
        }

        public Categoria ObterPorId(int id)
        {
            return _repository.ObterPorId(id);
        }

        public void Adicionar(Categoria categoria)
        {
            _repository.Adicionar(categoria);
        }

        public void Deletar(int id)
        {
            var categoria = _repository.ObterPorId(id);
            if (categoria == null)
            {
                // NOTA: Lançamos uma exceção aqui. A Controller ou o Middleware vai capturar isso depois.
                throw new Exception("Categoria não encontrada.");
            }

            if (categoria.Chamados != null && categoria.Chamados.Count > 0)
            {
                throw new Exception("Não é possível deletar uma categoria que possui chamados.");
            }

            _repository.Deletar(categoria);
        }
    }
}
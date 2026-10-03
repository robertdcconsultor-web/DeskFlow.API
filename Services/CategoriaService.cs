using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Threading.Tasks;

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

        public async Task<List<Categoria>> ObterTodasAsync()
        {
            return await _repository.ObterTodasAsync();
        }

        public async Task<Categoria> ObterPorIdAsync(int id)
        {
            return await _repository.ObterPorIdAsync(id);
        }

        public async Task AdicionarAsync(Categoria categoria)
        {
            await _repository.AdicionarAsync(categoria);
        }

        public async Task DeletarAsync(int id)
        {
            var categoria = await _repository.ObterPorIdAsync(id);
            if (categoria == null)
            {
                // NOTA: Lançamos uma exceção aqui. A Controller ou o Middleware vai capturar isso depois.
                throw new Exception("Categoria não encontrada.");
            }

            // A regra de negócio continua protegida, mas agora roda de forma assíncrona
            if (categoria.Chamados != null && categoria.Chamados.Count > 0)
            {
                throw new Exception("Não é possível deletar uma categoria que possui chamados.");
            }

            await _repository.DeletarAsync(categoria);
        }
    }
}
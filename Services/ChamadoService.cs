using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeskFlow.API.Services
{
    // NOTA: O ChamadoService é a nossa "Controladora de Acesso". Nenhuma alteração vai para o BD sem passar por aqui.
    public class ChamadoService
    {
        private readonly ChamadoRepository _chamadoRepository;
        private readonly InteracaoRepository _interacaoRepository;

        public ChamadoService(ChamadoRepository chamadoRepository, InteracaoRepository interacaoRepository)
        {
            _chamadoRepository = chamadoRepository;
            _interacaoRepository = interacaoRepository;
        }

        public void AbrirChamado(Chamado chamado)
        {
            chamado.Status = "Aberto";
            chamado.DataAbertura = DateTime.Now;

            _chamadoRepository.Adicionar(chamado);
            
            // CORREÇÃO: A criação automática de interação também centralizada aqui.
            _interacaoRepository.Adicionar(new Interacao { 
                ChamadoId = chamado.Id, 
                DataRegistro = DateTime.Now, 
                Autor = "Sistema",
                Mensagem = "Chamado aberto." 
            });
        }

        public Chamado ObterDetalhes(int id)
        {
            return _chamadoRepository.ObterPorId(id);
        }

        public void IniciarAtendimento(int id)
        {
            var chamado = _chamadoRepository.ObterPorId(id);
            if (chamado == null) throw new Exception("Chamado não encontrado.");

            chamado.Status = "EmAndamento";
            _chamadoRepository.Atualizar(chamado);
        }

        public void EncerrarChamado(int id, string solucao)
        {
            var chamado = _chamadoRepository.ObterPorId(id);
            if (chamado == null) throw new Exception("Chamado não encontrado.");

            // CORREÇÃO: Validação de negócio no Service, não na Controller.
            if (string.IsNullOrEmpty(solucao))
            {
                throw new Exception("A solução é obrigatória para encerrar o chamado.");
            }

            chamado.Status = "Fechado";
            chamado.Solucao = solucao;
            chamado.DataFechamento = DateTime.Now;

            _chamadoRepository.Atualizar(chamado);
        }

        public List<Chamado> Listar(string? status, string? prioridade, int? categoriaId)
        {
            var todosChamados = _chamadoRepository.ObterTodos();

            if (!string.IsNullOrEmpty(status))
                todosChamados = todosChamados.Where(c => c.Status == status).ToList();

            if (!string.IsNullOrEmpty(prioridade))
                todosChamados = todosChamados.Where(c => c.Prioridade == prioridade).ToList();

            if (categoriaId.HasValue)
                todosChamados = todosChamados.Where(c => c.CategoriaId == categoriaId.Value).ToList();

            return todosChamados;
        }

        public void AdicionarInteracao(int chamadoId, Interacao interacao)
        {
            var chamado = _chamadoRepository.ObterPorId(chamadoId);
            if (chamado == null) throw new Exception("Chamado não encontrado.");

            // CORREÇÃO: Implementada a trava de segurança exigida pelo RF10! Não permitimos inserir comentários em chamados fechados.
            if (chamado.Status == "Fechado")
            {
                throw new Exception("Não é permitido adicionar interações em um chamado fechado.");
            }

            interacao.ChamadoId = chamadoId;
            interacao.DataRegistro = DateTime.Now;

            _interacaoRepository.Adicionar(interacao);
        }
    }
}
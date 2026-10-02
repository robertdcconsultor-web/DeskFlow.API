using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; //NOTA: Necessário para o ToListAsync
using System.Threading.Tasks;
using System.Runtime.Intrinsics.Arm;

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

        public async Task AbrirChamadoAsync(Chamado chamado)
        {
            chamado.Status = "Aberto";
            chamado.DataAbertura = DateTime.Now;

            //NOTA: Aguardamos a gravação do chamado no BD
            await _chamadoRepository.AdicionarAsync(chamado);
            
            // CORREÇÃO: A criação automática de interação também centralizada aqui.
            await _interacaoRepository.AdicionarAsync(new Interacao { 
                ChamadoId = chamado.Id, 
                DataRegistro = DateTime.Now, 
                Autor = "Sistema",
                Mensagem = "Chamado aberto." 
            });
        }

        public async Task<Chamado?> ObterDetalhesAsync(int id)
        {
            return await _chamadoRepository.ObterPorIdAsync(id);
        }

        public async Task IniciarAtendimentoAsync(int id)
        {
            var chamado = await _chamadoRepository.ObterPorIdAsync(id);
            if (chamado == null) throw new Exception("Chamado não encontrado.");

            chamado.Status = "EmAndamento";
            await _chamadoRepository.AtualizarAsync(chamado);
        }

        public async Task EncerrarChamadoAsync(int id, string solucao)
        {
            var chamado = await _chamadoRepository.ObterPorIdAsync(id);
            if (chamado == null) throw new Exception("Chamado não encontrado.");

            // CORREÇÃO: Validação de negócio no Service, não na Controller.
            if (string.IsNullOrEmpty(solucao))
            {
                throw new Exception("A solução é obrigatória para encerrar o chamado.");
            }

            chamado.Status = "Fechado";
            chamado.Solucao = solucao;
            chamado.DataFechamento = DateTime.Now;

            await _chamadoRepository.AtualizarAsync(chamado);
        }

        public async Task<List<Chamado>> ListarAsync(string? status, string? prioridade, int? categoriaId)
        {
            //Correção
            var query = _chamadoRepository.ObterQueryable();

            // NOTA: Vamos empilhando os filtros (WHERE) na query SQL, sem ir ao banco de dados ainda.
            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (!string.IsNullOrEmpty(prioridade))
                query = query.Where(c => c.Prioridade == prioridade);

            if (categoriaId.HasValue)
                query = query.Where(c => c.CategoriaId == categoriaId.Value);

            // Apenas aqui, na hora do .ToList(), o Entity Framework traduz tudo para um comando SQL, ex: SELECT * FROM Chamados WHERE Status = 'Aberto' e dispara para o SQL Server, economizando banda e memória RAM!
            return await query.ToListAsync();
        }

        public async Task AdicionarInteracaoAsync(int chamadoId, Interacao interacao)
        {
            var chamado = await _chamadoRepository.ObterPorIdAsync(chamadoId);
            if (chamado == null) throw new Exception("Chamado não encontrado.");

            // CORREÇÃO: Implementada a trava de segurança exigida pelo RF10! Não permitimos inserir comentários em chamados fechados.
            if (chamado.Status == "Fechado")
            {
                throw new Exception("Não é permitido adicionar interações em um chamado fechado.");
            }

            interacao.ChamadoId = chamadoId;
            interacao.DataRegistro = DateTime.Now;

            await _interacaoRepository.AdicionarAsync(interacao);
        }
    }
}
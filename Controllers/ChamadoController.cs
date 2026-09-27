using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Repositories;
using DeskFlow.API.Models.Entities;
using System;
using System.Linq; //Necessário para uso do .Where() do LINQ.

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : ControllerBase
    {
        private readonly ChamadoRepository _chamadoRepository;
        private readonly InteracaoRepository _interacaoRepository;

        // NOTA: Injetamos agora os dois repositórios necessários na Controller.
        public ChamadosController(ChamadoRepository chamadoRepository, InteracaoRepository interacaoRepository)
        {
            _chamadoRepository = chamadoRepository;
            _interacaoRepository = interacaoRepository;
        }

        [HttpPost]
        public IActionResult AbrirChamado(Chamado chamado)
        {
            // NOTA: Forçamos o status inicial e a data do sistema para impedir que o utilizador envie via Postman um chamado com status "Fechado" na data de abertura.
            chamado.Status = "Aberto";
            chamado.DataAbertura = DateTime.Now;

            _chamadoRepository.Adicionar(chamado);
            _interacaoRepository.Adicionar(new Interacao { ChamadoId = chamado.Id, Data = DateTime.Now, Descricao = "Chamado aberto." });
            return Created("", chamado);
        }

        [HttpGet("{id}")]
        public IActionResult ObterDetalhes(int id)
        {
            var chamado = _chamadoRepository.ObterPorId(id);
            if (chamado == null) return NotFound();
            
            return Ok(chamado);
        }

        // NOTA: O método HttpPatch é ideal para atualizações parciais num recurso (neste caso, apenas alterar o Status).
        [HttpPatch("{id}/iniciar")]
        public IActionResult IniciarAtendimento(int id)
        {
            var chamado = _chamadoRepository.ObterPorId(id);
            if (chamado == null) return NotFound();

            chamado.Status = "EmAndamento";
            _chamadoRepository.Atualizar(chamado);

            return NoContent();
        }

        [HttpPatch("{id}/encerrar")]
        public IActionResult EncerrarChamado(int id, [FromBody] string solucao)
        {
            var chamado = _chamadoRepository.ObterPorId(id);
            if (chamado == null) return NotFound();

            if (string.IsNullOrEmpty(solucao))
            {
                // NOTA: O RF08 exige texto de solução. Se não for enviado, devolvemos erro 400.
                return BadRequest("A solução é obrigatória para encerrar o chamado.");
            }

            chamado.Status = "Fechado";
            chamado.Solucao = solucao;
            chamado.DataFechamento = DateTime.Now;

            _chamadoRepository.Atualizar(chamado);
            return NoContent();
        }

        // RF12: Listagem com Filtros Dinâmicos
        [HttpGet]
        public IActionResult Listar([FromQuery] string? status, [FromQuery] string? prioridade, [FromQuery] int? categoriaId)
        {
            var todosChamados = _chamadoRepository.ObterTodos();

            if (!string.IsNullOrEmpty(status))
                todosChamados = todosChamados.Where(c => c.Status == status).ToList();

            if (!string.IsNullOrEmpty(prioridade))
                todosChamados = todosChamados.Where(c => c.Prioridade == prioridade).ToList();

            if (categoriaId.HasValue)
                todosChamados = todosChamados.Where(c => c.CategoriaId == categoriaId.Value).ToList();

            return Ok(todosChamados);
        }

        // RF10: Adicionar Interação
        [HttpPost("{id}/interacoes")]
        public IActionResult AdicionarInteracao(int id, Interacao interacao)
        {
            var chamado = _chamadoRepository.ObterPorId(id);
            if (chamado == null) return NotFound("Chamado não encontrado.");

            // NOTA: Associamos o ID do chamado à interação para o banco de dados fazer a ligação (Foreign Key).
            interacao.ChamadoId = id;
            interacao.DataRegistro = DateTime.Now;

            _interacaoRepository.Adicionar(interacao);

            return Created("", interacao);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services;
using System;
using System.Threading.Tasks;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : ControllerBase
    {
        private readonly ChamadoService _service;

        // NOTA SÊNIOR: A Controller agora só conhece o Service. Ignora a existência de Repositories.
        public ChamadosController(ChamadoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AbrirChamado(Chamado chamado)
        {
            await _service.AbrirChamadoAsync(chamado);
            return Created("", chamado);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterDetalhes(int id)
        {
            var chamado = await _service.ObterDetalhesAsync(id);
            if (chamado == null) return NotFound();
            return Ok(chamado);
        }

        [HttpPatch("{id}/iniciar")]
        public async Task<IActionResult> IniciarAtendimento(int id)
        {
            try
            {
                await _service.IniciarAtendimentoAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Chamado não encontrado.") return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/encerrar")]
        public async Task<IActionResult> EncerrarChamado(int id, [FromBody] string solucao)
        {
            try
            {
                await _service.EncerrarChamadoAsync(id, solucao);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Chamado não encontrado.") return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? status, [FromQuery] string? prioridade, [FromQuery] int? categoriaId)
        {
            var chamados = await _service.ListarAsync(status, prioridade, categoriaId);
            return Ok(chamados);
        }

        [HttpPost("{id}/interacoes")]
        public async Task<IActionResult> AdicionarInteracao(int id, Interacao interacao)
        {
            try
            {
                await _service.AdicionarInteracaoAsync(id, interacao);
                return Created("", interacao);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Chamado não encontrado.") return NotFound();
                return BadRequest(ex.Message);
            }
        }
    }
}
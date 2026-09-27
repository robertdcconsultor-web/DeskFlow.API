using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services;
using System;

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
        public IActionResult AbrirChamado(Chamado chamado)
        {
            _service.AbrirChamado(chamado);
            return Created("", chamado);
        }

        [HttpGet("{id}")]
        public IActionResult ObterDetalhes(int id)
        {
            var chamado = _service.ObterDetalhes(id);
            if (chamado == null) return NotFound();
            return Ok(chamado);
        }

        [HttpPatch("{id}/iniciar")]
        public IActionResult IniciarAtendimento(int id)
        {
            try
            {
                _service.IniciarAtendimento(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Chamado não encontrado.") return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/encerrar")]
        public IActionResult EncerrarChamado(int id, [FromBody] string solucao)
        {
            try
            {
                _service.EncerrarChamado(id, solucao);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Chamado não encontrado.") return NotFound();
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] string? status, [FromQuery] string? prioridade, [FromQuery] int? categoriaId)
        {
            var chamados = _service.Listar(status, prioridade, categoriaId);
            return Ok(chamados);
        }

        [HttpPost("{id}/interacoes")]
        public IActionResult AdicionarInteracao(int id, Interacao interacao)
        {
            try
            {
                _service.AdicionarInteracao(id, interacao);
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
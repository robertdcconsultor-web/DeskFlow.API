using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Models.Entities;
using DeskFlow.API.Services; // Adicionado para enxergar o Service

namespace DeskFlow.API.Controllers
{
    // NOTA: O ApiController ativa comportamentos automáticos do .NET. O Route define a URL base. O [controller] é substituído pelo nome da classe, então a rota vai ser: /api/categorias
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        // CORREÇÃO: Removemos o CategoriaRepository e injetamos o CategoriaService. A Controller agora conversa com o Service. O Service conversa com o Repository.
        private readonly CategoriaService _service;

        public CategoriasController(CategoriaService service)
        {
            _service = service;
        }

        // NOTA: HttpGet significa que esse endpoint atende requisições do tipo GET (para buscar dados).
        [HttpGet]
        public IActionResult Listar()
        {
            var categorias = _service.ObterTodas();
            // NOTA: Retornamos Ok() que representa o Status 200 HTTP.
            return Ok(categorias);
        }

        [HttpPost]
        public IActionResult Cadastrar(Categoria categoria)
        {
            // NOTA: HttpPost é usado para criar novos recursos.
            _service.Adicionar(categoria);
            // NOTA: Retorno 201 Created indica sucesso na criação.
            return Created("", categoria);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            // CORREÇÃO: A Controller ficou extremamente limpa (Lean Controller). Ela apenas chama a ação. Se a regra de negócio falhar, o erro estoura no bloco try/catch.
            try
            {
                _service.Deletar(id);
                return NoContent(); // Status 204
            }
            catch (System.Exception ex)
            {
                // NOTA: Devolvemos o erro da regra de negócio de forma limpa para quem chamou a API.
                if (ex.Message == "Categoria não encontrada.") return NotFound();
                
                return BadRequest(ex.Message);
            }
        }
    }
}
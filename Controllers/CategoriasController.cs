using Microsoft.AspNetCore.Mvc;
using DeskFlow.API.Repositories;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Controllers
{
    // NOTA: O ApiController ativa comportamentos automáticos do .NET. O Route define a URL base. O [controller] é substituído pelo nome da classe, então a rota vai ser: /api/categorias
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaRepository _repository;

        public CategoriasController(CategoriaRepository repository)
        {
            _repository = repository;
        }

        // NOTA: HttpGet significa que esse endpoint atende requisições do tipo GET (para buscar dados).
        [HttpGet]
        public IActionResult Listar()
        {
            var categorias = _repository.ObterTodas();
            // NOTA: Retornamos Ok() que representa o Status 200 HTTP.
            return Ok(categorias);
        }

        [HttpPost]
        public IActionResult Cadastrar(Categoria categoria)
        {
            // NOTA: HttpPost é usado para criar novos recursos.
            _repository.Adicionar(categoria);
            // NOTA: Retorno 201 Created indica sucesso na criação.
            return Created("", categoria);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var categoria = _repository.ObterPorId(id);
            if (categoria == null)
            {
                return NotFound(); // Status 404
            }

            if (categoria.Chamados != null && categoria.Chamados.Count > 0)
            {
                // NOTA: Não deixamos excluir se tiver chamados para não quebrar a integridade do banco.
                return BadRequest("Não é possível deletar uma categoria que possui chamados.");
            }

            _repository.Deletar(categoria);
            return NoContent(); // Status 204
        }
    }
}
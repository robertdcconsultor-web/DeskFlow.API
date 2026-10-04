using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DeskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // NOTA: Criamos um DTO (Data Transfer Object) simples apenas para receber os dados do corpo da requisição.
        public class LoginDTO
        {
            public string Usuario { get; set; }
            public string Senha { get; set; }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            // NOTA: Em um cenário real, aqui nós injetaríamos um UsuarioService, iríamos até o banco de dados e validaríamos o hash da senha.
            // Para validar o RNF05, usaremos um usuário de sistema chumbado no código (Mock).
            
            if (login.Usuario == "admin" && login.Senha == "admin!123")
            {
                var token = GerarTokenJwt();
                return Ok(new { token = token, mensagem = "Autenticado com sucesso!" });
            }

            return Unauthorized("Usuário ou senha inválidos.");
        }

        private string GerarTokenJwt()
        {
            // Buscamos a mesma chave secreta que configuramos no appsettings.json
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "ChaveSuperSecretaDeskFlowApi123456789!");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Aqui definimos o que vai "dentro" do crachá (ex: Nome, Permissões/Roles)
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "Administrador DeskFlow"),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                
                // O token expira em 2 horas
                Expires = DateTime.UtcNow.AddHours(2),
                
                // Assinatura digital do token para evitar falsificação
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature),
                
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
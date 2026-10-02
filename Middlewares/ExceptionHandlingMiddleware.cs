using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeskFlow.API.Middlewares
{
    // NOTA: Esta classe intercepta absolutamente tudo que entra e sai da API.
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // NOTA: O 'next' manda a requisição seguir o fluxo normal até chegar na Controller.
                await _next(context);
            }
            catch (Exception ex)
            {
                // CORREÇÃO (RNF03): Se qualquer código falhar e não tivermos tratado com try/catch lá na Controller, o erro cai direto aqui.
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // NOTA: Forçamos a resposta a ser um JSON.
            context.Response.ContentType = "application/json";
            
            // Definimos o Status HTTP 500 (Erro Interno do Servidor)
            context.Response.StatusCode = 500; 

            // Montamos um objeto limpo e seguro para devolver ao usuário
            var result = JsonSerializer.Serialize(new
            {
                mensagem = "Ocorreu um erro interno inesperado no sistema.",
                detalhe = exception.Message // Em um cenário de segurança máxima, nem a mensagem do sistema repassaríamos.
            });

            return context.Response.WriteAsync(result);
        }
    }
}
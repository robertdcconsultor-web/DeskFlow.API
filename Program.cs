using DeskFlow.API.Data;
using DeskFlow.API.Repositories;
using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Services;

var builder = WebApplication.CreateBuilder(args);

// NOTA: Precisamos avisar o sistema que vamos usar o Controllers.
builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NOTA: Configurando a Injeção de Dependência do DbContext.
builder.Services.AddDbContext<AppDbContext>();

// NOTA: Injeção de dependência do Repository. Isso significa: "Sempre que a Controller pedir um CategoriaRepository, crie um novo (Scoped)".
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<ChamadoRepository>();
builder.Services.AddScoped<InteracaoRepository>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ChamadoService>();

// NOTA: Configuração de CORS. Libera a API para ser acessada por qualquer Front-End. 
// Em produção, substituiríamos AllowAnyOrigin pelo domínio exato da empresa (ex: helpdesk.empresa.com.br).
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// NOTA: Adicionamos o Middleware Global de Erros no pipeline de execução.
app.UseMiddleware<DeskFlow.API.Middlewares.ExceptionHandlingMiddleware>();

// NOTA: Ativa a página do Swagger para podermos testar a API no navegador.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// CORREÇÃO: Ativa a política de CORS que criamos acima.
app.UseCors("PermitirTudo");
app.UseAuthorization();
app.MapControllers();

app.Run();
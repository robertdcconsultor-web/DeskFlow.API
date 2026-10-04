using DeskFlow.API.Data;
using DeskFlow.API.Repositories;
using Microsoft.EntityFrameworkCore;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// NOTA: Precisamos avisar o sistema que vamos usar o Controllers.
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        // CORREÇÃO: Evita o loop infinito ao serializar Entidades relacionadas (Chamado <-> Interacao)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();

// NOTA: Configuração do Swagger para aceitar o Token JWT na interface gráfica
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeskFlow API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta maneira: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

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

// NOTA: Configuração de Autenticação JWT
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? "ChaveSuperSecretaDeskFlowApi123456789!");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"]
    };
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

// CORREÇÃO (RNF05): O sistema de Autenticação DEVE vir antes da Autorização!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
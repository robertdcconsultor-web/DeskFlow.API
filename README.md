# 🎧 DeskFlow API — Gestão de Chamados e Helpdesk de TI

## 🎯 Sobre o Projeto
A **DeskFlow API** é uma Web API RESTful construída em .NET Core 10 utilizando Entity Framework Core e SQL Server. O sistema automatiza o gerenciamento de chamados de suporte técnico, histórico de interações e acompanhamento de status do atendimento.

Este projeto foi desenvolvido como avaliação final do Módulo 01 por **Robert Diório Campos**, aplicando conceitos sólidos de arquitetura em camadas, injeção de dependência e tratamento global de exceções.

## 🛠️ Tecnologias Utilizadas
- .NET Core 10 / Web API
- Entity Framework Core 10
- SQL Server (LocalDB / Express)
- Swagger / OpenAPI para documentação de endpoints
- Arquitetura em Camadas (Controllers, Services, Repositories)

## 🚀 Como Executar a Aplicação

### Pré-requisitos
- .NET SDK 10 (ou superior)
- SQL Server em execução na máquina local
- Ferramenta global do EF Core (`dotnet tool install --global dotnet-ef`)

### Passo a Passo
1. **Clone este repositório:**
   ```bash
   git clone [https://github.com/robertdcconsultor-web/DeskFlow.API.git](https://github.com/robertdcconsultor-web/DeskFlow.API.git)

2. **Acesse a pasta do projeto:**
    ```bash
    cd DeskFlow.API

3. **Configure a Connection String no arquivo appsettings.json (se necessário, ajuste a instância do servidor para localhost\SQLEXPRESS ou (localdb)\mssqllocaldb):**

    JSON
    "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;"
    }

4. **Execute as Migrations para criar a estrutura no banco de dados:**

   ```bash
    dotnet ef database update

5. **Execute a API:**

   ```bash
    dotnet run
    
6. **Acesse a documentação do Swagger para testar os endpoints:**
    Abra no navegador: http://localhost:5000/swagger ou https://localhost:7001/swagger (verifique a porta gerada no seu terminal).


## 🧠 Ciclo de Vida do Chamado

- Aberto: Chamado registrado pelo solicitante.

- EmAndamento: Suporte assumiu o atendimento ao chamado.

- Fechado: Chamado encerrado obrigatoriamente com um texto de solução e data de conclusão.

## 🧱 Arquitetura e Decisões Técnicas

- Controllers: Recebem as requisições HTTP e definem os Status Codes (Lean Controllers).

- Services: Contêm as regras de negócio rigorosas (ex: não permitir interações em chamados fechados).

- Repositories: Executam comandos e consultas de banco via EF Core utilizando IQueryable para otimização de memória.

- Middlewares: Implementação de ExceptionHandlingMiddleware para tratamento e padronização de erros globais sem vazar o Stack Trace.

- Integridade: Uso de Data Annotations e Enums para garantir que dados inconsistentes não cheguem ao banco de dados.   

## 🎥 Vídeo de Apresentação
👉 [INSERIR LINK DO SEU VÍDEO DO YOUTUBE/DRIVE AQUI] 👈
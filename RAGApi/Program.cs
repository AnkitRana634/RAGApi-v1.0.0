using Application.ServiceClass;
using Application.ServiceContract;
using Application.VectorStore;
using Microsoft.EntityFrameworkCore;
using RAGApi.Application.ServiceClass;
using RAGApi.Application.ServiceContract;
using RAGApi.Repositories;
using RAGApi.Domain.Repository;
using RAGApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RagDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("MSSQLDbConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                )));

builder.Services.AddHttpClient<ILlmService, OllamaLlmService>(
    client =>
    {
        client.BaseAddress =
            new Uri("http://localhost:11434");

        client.Timeout =
            TimeSpan.FromMinutes(5);
    });
builder.Services.AddHttpClient<IEmbeddingService,EmbeddingService>(
    client =>
    {
        client.BaseAddress =
            new Uri("http://localhost:11434");
    });
builder.Services.AddScoped<IDocumentExtract, DocumentExtract>();
builder.Services.AddScoped<ITextChunkingService,TextChunkingService>();
builder.Services.AddScoped<IDocumentIngestionService, DocumentIngestionService>();
builder.Services.AddSingleton<IVectorStore, InMemoryVectorStore>();
builder.Services.AddScoped<IRagService, RagService>();
builder.Services.AddScoped<IMcpRepo, McpRepo>();
builder.Services.AddScoped<IMCPService, MCPService>();
builder.Services.AddScoped<IAgent, AiAgent>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();

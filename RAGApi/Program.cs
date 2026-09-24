using Application.ServiceClass;
using Application.ServiceContract;
using Application.VectorStore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using RAGApi.Application.ServiceClass;
using RAGApi.Application.ServiceContract;
using RAGApi.Domain.Repository;
using RAGApi.Infrastructure.Services;
using RAGApi.Repositories;
using Microsoft.OpenApi;

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
        client.BaseAddress = builder.Configuration.GetValue<string>("Env") == "production" ?
            new Uri("https://ollama.internal.livelydune-92aae421.westus2.azurecontainerapps.io") : new Uri("http://localhost:11434");

        client.Timeout =
            TimeSpan.FromMinutes(5);
    });
builder.Services.AddHttpClient<IEmbeddingService, EmbeddingService>(
    client =>
    {
        client.BaseAddress = builder.Configuration.GetValue<string>("Env") == "production" ?
            new Uri("https://ollama.internal.livelydune-92aae421.westus2.azurecontainerapps.io") : new Uri("http://localhost:11434");
    });
// Configure Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AgenticRAGMCPAPI", Version = "v1" });
});
// Configure CORS - allow all origins (development convenience)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddScoped<IDocumentExtract, DocumentExtract>();
builder.Services.AddScoped<ITextChunkingService, TextChunkingService>();
builder.Services.AddScoped<IDocumentIngestionService, DocumentIngestionService>();
builder.Services.AddSingleton<IVectorStore, InMemoryVectorStore>();
builder.Services.AddScoped<IRagService, RagService>();
builder.Services.AddScoped<IMcpRepo, McpRepo>();
builder.Services.AddScoped<IMCPService, MCPService>();
builder.Services.AddScoped<IAgent, AiAgent>();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

// Enable Swagger UI and open it automatically on app start (development only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgenticRAGMCPAPI v1");
    });

    app.Lifetime.ApplicationStarted.Register(() =>
    {
        try
        {
            var url = app.Urls.FirstOrDefault() ?? builder.Configuration["ASPNETCORE_URLS"] ?? "http://localhost:5070";
            var swaggerUrl = url.TrimEnd('/') + "/swagger/index.html";
            Process.Start(new ProcessStartInfo { FileName = swaggerUrl, UseShellExecute = true });
        }
        catch
        {
            // ignore failures to open browser
        }
    });
}
else
{
    // In non-development environments you may still enable swagger if desired
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgenticRAGMCPAPI v1"));
}

// Enable CORS globally
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();

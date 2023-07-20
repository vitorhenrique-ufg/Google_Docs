using GoogleDocs.Aplicacao;
using GoogleDocs.Aplicacao.Comandos;
using GoogleDocs.Aplicacao.Comandos.Implementacoes;
using GoogleDocs.Aplicacao.Repositorios;
using GoogleDocs.Aplicacao.Repositorios.Implementacoes;
using GoogleDocs.WebSockets.Handler;
using GoogleDocs.WebSockets.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services
       .AddSingleton<IComandoInsiraDocumentoTexto, ComandoInsiraDocumentoTexto>()
       .AddSingleton<IRepositorioDocs, RepositorioDocs>()
       .AddSingleton<WebSocketHandler>();

builder.Services.AddTransient(sp =>
{
    IConfiguration? config = sp.GetRequiredService<IConfiguration>();
    return new ConexaoPostgres(config.GetSection("ConnectionStrings:Conexao")?.Value);
});

var app = builder.Build();


app.UseHttpsRedirection()
    .UseHsts()
   .UseWebSockets()
   .Map("/ws", (x) => x.UseMiddleware<WebSocketMiddleware>())
   .UseStaticFiles();


app.Run();


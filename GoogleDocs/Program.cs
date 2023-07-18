using GoogleDocs.Aplicacao;
using GoogleDocs.Aplicacao.Comandos;
using GoogleDocs.Aplicacao.Comandos.Implementacoes;
using GoogleDocs.Aplicacao.Consultas;
using GoogleDocs.Aplicacao.Consultas.Implementacoes;
using GoogleDocs.Aplicacao.Repositorios;
using GoogleDocs.Aplicacao.Repositorios.Implementacoes;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder
    .Services
    .AddResponseCompression()
    .AddCors()
    .Configure<KestrelServerOptions>(options =>
    {
        options.AllowSynchronousIO = true;
    })
    .Configure<IISServerOptions>(options =>
    {
        options.AllowSynchronousIO = true;
    })
    .ConfigureHttpJsonOptions((json) =>
    {
        json.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services
       .AddScoped<IComandoInsiraDocumentoTexto, ComandoInsiraDocumentoTexto>()
       .AddScoped<IConsultaDocumentoTexto, ConsultaDocumentoTexto>()
       .AddScoped<IRepositorioDocs, RepositorioDocs>();

builder.Services.AddTransient(sp =>
{
    IConfiguration? config = sp.GetRequiredService<IConfiguration>();
    return new ConexaoPostgres(config.GetSection("ConnectionStrings:Conexao")?.Value);
});

var app = builder.Build();

app.UseCors(x => x.AllowAnyOrigin())
   .UseStaticFiles()
   .UseRouting()
   .UseResponseCompression();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ManipuladorDocs}/{action=Index}/{id?}");

app.Run();

using GoogleDocs.Aplicacao.Repositorios;
using GoogleDocs.Aplicacao.Requisicoes;

namespace GoogleDocs.Aplicacao.Comandos.Implementacoes;

public sealed class ComandoInsiraDocumentoTexto : IComandoInsiraDocumentoTexto
{
    private readonly SemaphoreSlim _semaforo = new(1, 1);
    private readonly IRepositorioDocs _repositorioDocs;

    public ComandoInsiraDocumentoTexto(IRepositorioDocs repositorioDocs)
    {
        _repositorioDocs = repositorioDocs;
    }

    public async Task Execute(DocumentoRequisicao requisicao)
    {
        try
        {
            await _semaforo.WaitAsync();
            await _repositorioDocs.InsiraDocumento(requisicao);
        }
        finally
        {
            _semaforo.Release();
        }
    }
}


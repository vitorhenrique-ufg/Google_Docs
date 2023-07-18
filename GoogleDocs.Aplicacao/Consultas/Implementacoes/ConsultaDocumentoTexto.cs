using GoogleDocs.Aplicacao.Dominio;
using GoogleDocs.Aplicacao.Repositorios;
using GoogleDocs.Aplicacao.Requisicoes;

namespace GoogleDocs.Aplicacao.Consultas.Implementacoes;

public sealed class ConsultaDocumentoTexto : IConsultaDocumentoTexto
{
    private readonly IRepositorioDocs _repositorioDocs;

    public ConsultaDocumentoTexto(IRepositorioDocs repositorioDocs)
    {
        _repositorioDocs = repositorioDocs;
    }

    public async Task<Documento?> Execute() =>
        await _repositorioDocs.Consulte();
}


using GoogleDocs.Aplicacao.Dominio;
using GoogleDocs.Aplicacao.Requisicoes;

namespace GoogleDocs.Aplicacao.Repositorios;

public interface IRepositorioDocs
{
    Task InsiraDocumento(DocumentoRequisicao requisicao);

    Task<Documento?> Consulte();
}

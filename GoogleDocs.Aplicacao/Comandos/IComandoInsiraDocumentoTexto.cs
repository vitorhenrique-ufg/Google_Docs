using GoogleDocs.Aplicacao.Requisicoes;

namespace GoogleDocs.Aplicacao.Comandos;

public interface IComandoInsiraDocumentoTexto
{
    Task Execute(DocumentoRequisicao requisicao);
}


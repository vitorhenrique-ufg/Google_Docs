using GoogleDocs.Aplicacao.Dominio;

namespace GoogleDocs.Aplicacao.Consultas;

public interface IConsultaDocumentoTexto
{
    Task<Documento?> Execute();
}


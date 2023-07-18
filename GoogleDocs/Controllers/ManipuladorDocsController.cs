using GoogleDocs.Aplicacao.Comandos;
using GoogleDocs.Aplicacao.Consultas;
using GoogleDocs.Aplicacao.Dominio;
using GoogleDocs.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoogleDocs.Controllers
{
    public class ManipuladorDocsController : Controller
    {
        private readonly IConsultaDocumentoTexto _consultaDocumentoTexto;

        public ManipuladorDocsController(IConsultaDocumentoTexto consultaDocumentoTexto)
        {
            _consultaDocumentoTexto = consultaDocumentoTexto;
        }

        public async Task<IActionResult> Index()
        {
            Documento? documento = await _consultaDocumentoTexto.Execute();
            return documento is null ? View() : View(new DocumentoModel() { Texto = documento.TextoDocumento});
        }
    }
}
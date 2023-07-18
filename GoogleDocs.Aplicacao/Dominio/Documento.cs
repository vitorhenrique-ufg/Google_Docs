namespace GoogleDocs.Aplicacao.Dominio;

public sealed record class Documento
{
    public long  Id { get; private set; }

    public string? TextoDocumento { get; private set; }

    public Documento(long id, string textoDocumento)
    {
        Id = id;
        TextoDocumento = textoDocumento;
    }
}

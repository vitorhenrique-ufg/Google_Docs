using GoogleDocs.Aplicacao.Dominio;
using GoogleDocs.Aplicacao.Extensions;
using GoogleDocs.Aplicacao.Requisicoes;
using System.Data.Common;

namespace GoogleDocs.Aplicacao.Repositorios.Implementacoes;

public class RepositorioDocs : IRepositorioDocs
{
    private readonly ConexaoPostgres _conexao;

    public RepositorioDocs(ConexaoPostgres conexao)
    {
        _conexao = conexao;
    }

    public async Task<Documento?> Consulte()
    {
        await using DbConnection? cn = await _conexao.OpenAsync();
        await using DbCommand? cmd = cn.CreateCommand();

        cmd.CommandText = @"SELECT id, documento FROM tbdocs";
        await using DbDataReader? dr = await cmd.ExecuteReaderAsync();

        return await dr.ReadAsync() ? new(dr.GetInteger("id"), dr.GetString("documento")) : null;
    }

    public async Task InsiraDocumento(DocumentoRequisicao requisicao)
    {
        try 
        {
            await using DbConnection? cn = await _conexao.OpenAsync();
            await using DbCommand? cmd = cn.CreateCommand();

            cmd.CommandText = @"UPDATE TBDOCS SET documento = @DOCUMENTO";
            cmd.Parameters.CrieParametros("@DOCUMENTO", requisicao.TextoDocumento);

            if (await cmd.ExecuteNonQueryAsync() is 0)
            {
                cmd.CommandText = @"INSERT INTO TBDOCS(documento) VALUES(@DOCUMENTO)";
                await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {

        }
    }
}


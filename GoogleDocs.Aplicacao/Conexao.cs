using Npgsql;
using System.Data.Common;

namespace GoogleDocs.Aplicacao;

public sealed class ConexaoPostgres
{
    public string ConnectionString { get; private set; }
    public ConexaoPostgres(string? connectionString)
    {
        if(string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
        ConnectionString = connectionString;
    }

    public async Task<DbConnection> OpenAsync(CancellationToken cancellationToken = default)
    {
        NpgsqlConnection cn = new(ConnectionString);
        await ((DbConnection)(object)cn).OpenAsync(cancellationToken);
        return (DbConnection)(object)cn;
    }
}


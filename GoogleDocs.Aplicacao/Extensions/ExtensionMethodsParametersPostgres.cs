using Npgsql;
using System.Data;
using System.Data.Common;

namespace GoogleDocs.Aplicacao.Extensions;

public static class ExtensionMethodsParametersPostgres
{
    public static DbParameter CrieParametros(this DbParameterCollection parameterCollection, string parameterName, object? value = null)
    {
        NpgsqlParameter val = new()
        {
            Direction = ParameterDirection.Input,
            ParameterName = parameterName,
            Value = value ?? DBNull.Value
        };
        NpgsqlParameter val2 = val;
        parameterCollection.Add(val2);
        return (DbParameter)(object)val2;
    }

    public static int GetInteger(this DbDataReader dr, string name)
    {
        int ordinal = dr.GetOrdinal(name);
        return dr.GetInt32(ordinal);
    }

    public static string GetString(this DbDataReader dr, string name)
    {
        int ordinal = dr.GetOrdinal(name);
        return dr.GetString(ordinal);
    }
}

using Application.Abstractions.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Persistence.Options.Database;

namespace Persistence.Data;

internal sealed class SqlConnectionFactory(IOptions<DatabaseOptions> options) : ISqlConnectionFactory
{
    private readonly string _connectionString = options.Value.ConnectionString;

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
using Microsoft.Data.SqlClient;

namespace Application.Abstractions.Data;

public interface ISqlConnectionFactory
{
    SqlConnection CreateConnection();
}
using MySql.Data.MySqlClient;

namespace TeamTaskTracker.Api.Data;

public interface IDbConnectionFactory
{
    MySqlConnection CreateConnection();
}

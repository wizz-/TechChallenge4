using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TechChallenge4.Infra.Data.Dapper.Contexto
{
    public class DbConnectionFactory(IConfiguration configuration)
    {
        private SqlConnection _connection;

        public IDbConnection GetConnection()
        {
            if (_connection == null) _connection = new(configuration.GetConnectionString("DefaultConnection"));
            if (_connection.State != ConnectionState.Open) _connection.Open();
            return _connection;
        }
    }
}

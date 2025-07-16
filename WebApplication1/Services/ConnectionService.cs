using Microsoft.Extensions.Configuration;
using Npgsql;

namespace WebApplication1.Services
{
    public class ConnectionService
    {
        private readonly string _connectionString;

        public ConnectionService(IConfiguration configuration, string connectionName = "DefaultConnection")
        {
            _connectionString = configuration.GetConnectionString(connectionName) 
                ?? throw new InvalidOperationException($"Connection string '{connectionName}' not found.");
        }

        public NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}

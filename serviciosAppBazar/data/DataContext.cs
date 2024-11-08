using System.Data;
using Microsoft.Data.SqlClient;

namespace serviciosAppBazar.data
{
    public class DataContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DataContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }    
        public IDbConnection createConnectionMainDatabase() => new SqlConnection(_connectionString);
    }
}

using NIST.CVP.Pools.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace NIST.CVP.Pools.Services
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Get(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
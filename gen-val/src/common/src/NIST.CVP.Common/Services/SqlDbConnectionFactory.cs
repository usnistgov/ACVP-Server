using System.Data.Common;
using System.Data.SqlClient;
using NIST.CVP.Common.Interfaces;

namespace NIST.CVP.Common.Services
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public DbConnection Get(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
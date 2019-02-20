using System.Data;
using System.Data.SqlClient;
using NIST.CVP.Common.Interfaces;

namespace NIST.CVP.Common.Services
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection Get(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}
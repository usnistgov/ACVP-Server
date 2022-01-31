using System.Data.Common;
using System.Data.SqlClient;
using NIST.CVP.ACVTS.Libraries.Common.Interfaces;

namespace NIST.CVP.ACVTS.Libraries.Common.Services
{
    public class SqlDbConnectionFactory : IDbConnectionFactory
    {
        public DbConnection Get(string connectionString)
        {
            return new SqlConnection(connectionString);
        }
    }
}

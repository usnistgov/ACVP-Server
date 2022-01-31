using Microsoft.Extensions.Configuration;
using NIST.CVP.ACVTS.Libraries.Common.Interfaces;

namespace NIST.CVP.ACVTS.Libraries.Common.Services
{
    public class DbConnectionStringFactory : IDbConnectionStringFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionStringFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string connectionStringName)
        {
            return _configuration.GetConnectionString(connectionStringName);
        }

        public string GetMightyConnectionString(string connectionStringName)
        {
            return $"ProviderName=System.Data.SqlClient;{GetConnectionString(connectionStringName)}";
        }
    }

}

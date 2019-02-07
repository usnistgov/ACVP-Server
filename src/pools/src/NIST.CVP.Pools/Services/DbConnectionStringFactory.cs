using Microsoft.Extensions.Configuration;
using NIST.CVP.Pools.Interfaces;

namespace NIST.CVP.Pools.Services
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
    }

}
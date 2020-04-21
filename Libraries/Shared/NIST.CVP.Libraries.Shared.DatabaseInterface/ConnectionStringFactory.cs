using Microsoft.Extensions.Configuration;

namespace NIST.CVP.Libraries.Shared.DatabaseInterface
{
	public class ConnectionStringFactory : IConnectionStringFactory
	{
		private readonly IConfiguration _configuration;

		public ConnectionStringFactory(IConfiguration configuration)
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

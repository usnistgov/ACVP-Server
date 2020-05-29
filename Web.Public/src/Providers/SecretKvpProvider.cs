using System;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;

namespace Web.Public.Providers
{
	public class SecretKvpProvider : ISecretKvpProvider
	{
		#region Key Constants
		public const string JwtSigningKey = "jwtSigningKey";
		#endregion Key Constants
		
		private readonly ILogger<SecretKvpProvider> _logger;
		private readonly string _connectionString;
        
		public SecretKvpProvider(ILogger<SecretKvpProvider> logger, IConnectionStringFactory connectionStringFactory)
		{
			_logger = logger;
			_connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}
		
		public string GetValueFromKey(string key)
		{
			var db = new MightyOrm(_connectionString);

			try
			{
				var requestData = db.ExecuteProcedure("external.SecretKeyValuePairGet", 
				new
				{
					ConfigKey = key
				}, 
				new
				{
					ConfigValue = string.Empty
				});

				return requestData.ConfigValue;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error getting value from key {key}");
				return null;
			}
		}
	}
}
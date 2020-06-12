using System;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class ValidationProvider : IValidationProvider
	{
		private readonly ILogger<ValidationProvider> _logger;
		private readonly string _connectionString;
		
		public ValidationProvider(ILogger<ValidationProvider> logger, IConnectionStringFactory connectionStringFactory)
		{
			_logger = logger;
			_connectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}
		
		public Validation GetValidation(long id)
		{
			var db = new MightyOrm<Validation>(_connectionString);
            
			try
			{
				var data = db.SingleFromProcedure("val.ValidationGet", new
				{
					Id = id
				});

				return data;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Failed getting validation for {id}");
				return null;
			}
		}
	}
}
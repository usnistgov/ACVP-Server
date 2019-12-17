using System;
using System.Collections.Generic;
using ACVPWorkflow;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace MessageQueueProcessor
{
	public class AutoApproveProvider : IAutoApproveProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<AutoApproveProvider> _logger;

		public AutoApproveProvider(IConnectionStringFactory connectionStringFactory, ILogger<AutoApproveProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Dictionary<APIAction, bool> GetAutoApproveConfiguration()
		{
			var db = new MightyOrm(_acvpConnectionString);


			Dictionary<APIAction, bool> autoApproveConfiguration = new Dictionary<APIAction, bool>();
			try
			{
				var data = db.QueryFromProcedure("acvp.AutoApproveConfigurationGet");

				foreach (var action in data)
				{
					autoApproveConfiguration.Add((APIAction)action.APIActionID, action.AutoApprove);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return autoApproveConfiguration;
		}
	}
}

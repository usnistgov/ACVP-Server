using System;
using ACVPWorkflow.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPWorkflow.Providers
{
	public class RequestProvider : IRequestProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<RequestProvider> _logger;

		public RequestProvider(IConnectionStringFactory connectionStringFactory, ILogger<RequestProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result Create(long requestID, RequestAction action, long workflowID, long userID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("acvp.RequestInsert", inParams: new
				{
					RequestID = requestID,
					ActionID = action,
					WorkflowID = workflowID,
					UserID = userID
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}
	}
}

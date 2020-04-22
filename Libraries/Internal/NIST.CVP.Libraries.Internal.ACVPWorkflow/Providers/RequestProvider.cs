using System;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
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

		public KillThisResult Create(long requestID, RequestAction action, long workflowID, long userID)
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

				return new KillThisResult();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new KillThisResult(ex.Message);
			}
		}
	}
}

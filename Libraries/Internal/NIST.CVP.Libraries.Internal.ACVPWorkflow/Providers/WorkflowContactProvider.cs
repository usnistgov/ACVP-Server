using System;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
{
	public class WorkflowContactProvider : IWorkflowContactProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<WorkflowContactProvider> _logger;

		public WorkflowContactProvider(IConnectionStringFactory connectionStringFactory, ILogger<WorkflowContactProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public WorkflowContact GetContactForACVPUser(long acvpUserID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("acvp.WorkflowContactDataForUserGet", inParams: new
				{
					ACVPUserID = acvpUserID
				});

				if (data != null)
				{
					return new WorkflowContact
					{
						Name = data.Name,
						Lab = data.Lab,
						Email = data.EmailAddress
					};
				}
				else return null;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}
	}
}

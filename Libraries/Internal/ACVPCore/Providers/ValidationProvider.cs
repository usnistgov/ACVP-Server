using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.Results;

namespace ACVPCore.Providers
{
	public class ValidationProvider : IValidationProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TestSessionProvider> _logger;

		public ValidationProvider(IConnectionStringFactory connectionStringFactory, ILogger<TestSessionProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public InsertResult Insert(ValidationSource validationSource, long validationNumber, long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ValidationInsert", inParams: new
				{
					ImplementationId = implementationID,
					SourceId = validationSource,             //TODO - Once LCAVP goes away hardcode this to ACVP maybe
					ValidationNumber = validationNumber
				});

				return new InsertResult(data.ValidationId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public List<(long ValidationID, int ValidationSource)> GetValidationsForImplementation(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<(long ValidationID, int ValidationSource)> validations = new List<(long ValidationID, int ValidationSource)>();
			try
			{
				var data = db.QueryFromProcedure("val.ValidationsForImplementationGet", inParams: new
				{
					ImplementationId = implementationID
				});

				foreach (var validation in data)
				{
					validations.Add((validation.ValidationId, validation.SourceId));
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return validations;
		}

		public long GetNextACVPValidationNumber()
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.NextACVPValidationNumberGet");

				return data.ValidationNumber;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return -1;
			}
		}

		public long GetNextLCAVPValidationNumber()
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.NextLCAVPValidationNumberGet");

				return data.ValidationNumber;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return -1;
			}
		}

		public Result ValidationTestSessionInsert(long validationID, long testSessionID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.ValidationTestSessionsInsert", inParams: new
				{
					ValidationId = validationID,
					TestSessionId = testSessionID
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public PagedEnumerable<ValidationLite> GetValidations(ValidationListParameters param)
		{
			List<ValidationLite> result = new List<ValidationLite>();
			long totalRecords = 0;
			var db = new MightyOrm<ValidationLite>(_acvpConnectionString);

			try
			{
				var dbData = db.QueryWithExpando("val.ValidationsGet",
					inParams: new
					{
						PageSize = param.PageSize,
						PageNumber = param.Page,
						ValidationId = param.ValidationId,
						ValidationLabel = param.ValidationLabel,
						ProductName = param.ProductName
					},
					new
					{
						totalRecords = (long)0
					});

				result = dbData.Data;
				totalRecords = dbData.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
			}
			
			return result.WrapPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}

		public Validation GetValidation(long validationId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ValidationGetById", new
				{
					validationId
				});

				if (data == null)
					return null;
				
				return new Validation()
				{
					Created = data.created,
					ProductName = data.productName,
					ValidationId = data.validationId,
					ValidationLabel = data.validationLabel,
					Updated = data.updated,
					VendorId = data.vendorId
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}
	}
}

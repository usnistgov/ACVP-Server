using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class OEProvider : IOEProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<OEProvider> _logger;

		public OEProvider(IConnectionStringFactory connectionStringFactory, ILogger<OEProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public List<long> GetDependencyLinks(long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<long> dependencyIDs = new List<long>();
			try
			{
				var data = db.QueryFromProcedure("dbo.OEDependencyLinksGet", inParams: new
				{
					OEID = oeID
				});

				foreach (var dependency in data)
				{
					dependencyIDs.Add(dependency.DependencyID);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return dependencyIDs;
		}

		public Result Delete(long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.OEDelete", inParams: new { OEId = oeID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllDependencyLinks(long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.OEDependencyForOEDeleteAll", inParams: new { OEId = oeID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteDependencyLink(long oeID, long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.OEDependencyDelete", inParams: new { DependencyId = dependencyID, OEId = oeID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(string name, bool isITAR)
		{
			if (string.IsNullOrWhiteSpace(name)) return new InsertResult("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.OEInsert", inParams: new
				{
					Name = name,
					IsITAR = isITAR
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert OE");
				}
				else
				{
					return new InsertResult((long)data.OEId);
				}

			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result InsertDependencyLink(long oeID, long dependencyID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//Since the link record is just these 2 IDs, no other ID on the record, no need to return an ID of the newly created record
				db.ExecuteProcedure("dbo.OEDependencyInsert", inParams: new
				{
					OEId = oeID,
					DependencyId = dependencyID,
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result Update(long oeID, string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return new Result("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.OEUpdate", inParams: new
				{
					OEId = oeID,
					Name = name
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public bool OEIsUsed(long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.OEIsUsed", inParams: new
				{
					OEID = oeID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return true;    //Default to true so we don't try do delete when we shouldn't
			}
		}

		public bool OEExists(long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.OEExists", inParams: new
				{
					OEId = oeID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;    //Default to false so we don't try do use it when we don't know if it exists
			}
		}

		public OperatingEnvironment Get(long oeID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			var OEResult = new OperatingEnvironment();
			try
			{
				var OEData = db.SingleFromProcedure("dbo.OEGet", inParams: new
				{
					OEId = oeID
				});

				OEResult.ID = OEData.OEId;
				OEResult.Name = OEData.Name;

				var data = db.QueryFromProcedure("dbo.OEDependenciesDetailsGet", inParams: new
				{
					OEID = oeID
				});

				foreach (var dependency in data)
				{
					OEResult.Dependencies.Add(new DependencyLite
					{
						ID = dependency.DependencyId,
						Name = dependency.Name,
						Type = dependency.DependencyType,
						Description = dependency.Description
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return OEResult;
		}

		public PagedEnumerable<OperatingEnvironmentLite> Get(OeListParameters param)
		{
			List<OperatingEnvironmentLite> result = new List<OperatingEnvironmentLite>();
			long totalRecords = 0;
			var db = new MightyOrm<OperatingEnvironmentLite>(_acvpConnectionString);

			try
			{
				var dbResult = db.QueryWithExpando("dbo.OEsGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					OEId = param.Id,
					Name = param.Name
				}, new
				{
					totalRecords = (long)0
				});

				result = dbResult.Data;
				totalRecords = dbResult.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return result.ToPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}

		public List<OperatingEnvironmentLite> GetOEsOnValidation(long validationID)
		{
			var db = new MightyOrm<OperatingEnvironmentLite>(_acvpConnectionString);

			try
			{
				return db.QueryFromProcedure("dbo.OEsForValidationGet", inParams: new { ValidationId = validationID }).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}
	}
}

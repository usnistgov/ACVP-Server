using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;
using System.Linq;

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
				var data = db.QueryFromProcedure("val.OEDependencyLinksGet", inParams: new
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
				db.ExecuteProcedure("val.OEDelete", inParams: new { OEID = oeID });
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
				db.ExecuteProcedure("val.OEDependencyLinksDeleteAll", inParams: new { OEID = oeID });
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
				db.ExecuteProcedure("val.DependencyOELinkDelete", inParams: new { DependencyID = dependencyID, OEID = oeID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) return new InsertResult("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.OEInsert", inParams: new
				{
					Name = name
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert OE");
				}
				else
				{
					return new InsertResult((long)data.OEID);
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
				db.ExecuteProcedure("val.OEDependencyLinkInsert", inParams: new
				{
					OEID = oeID,
					DependencyID = dependencyID,
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
				db.ExecuteProcedure("val.OEUpdate", inParams: new
				{
					OEID = oeID,
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
				var data = db.SingleFromProcedure("val.OEIsUsed", inParams: new
				{
					OEID = oeID
				});

				return data.IsUsed;
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
				return (bool)db.ScalarFromProcedure("val.OEExists", inParams: new
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
				var OEData = db.SingleFromProcedure("val.OEGet", inParams: new
				{
					OEID = oeID
				});

				OEResult.ID = OEData.Id;
				OEResult.Name = OEData.Name;

				var data = db.QueryFromProcedure("val.OEDependenciesGet", inParams: new
				{
					OEID = oeID
				});

				foreach (var dependency in data)
				{
					OEResult.Dependencies.Add(new DependencyLite
					{
						ID = dependency.id,
						Name = dependency.name,
						Type = dependency.dependency_type,
						Description = dependency.description
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
				var dbResult = db.QueryWithExpando("val.OEsGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					Id = param.Id,
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
				return db.QueryFromProcedure("val.OEsForValidationGet", inParams: new { ValidationId = validationID }).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}
	}
}

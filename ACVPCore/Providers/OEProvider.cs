using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class OEProvider : IOEProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<DependencyProvider> _logger;

		public OEProvider(IConnectionStringFactory connectionStringFactory, ILogger<DependencyProvider> logger)
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
				db.Execute("val.OEDelete @0", oeID);
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
				db.Execute("val.OEDependencyLinksDeleteAll @0", oeID);
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
				db.Execute("val.DependencyOELinkDelete @0, @1", dependencyID, oeID);
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
 
				OEResult.ID = OEData.id;
				OEResult.Name = OEData.name;

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

		public List<OperatingEnvironmentLite> Get(long pageSize, long pageNumber)
		{
			var db = new MightyOrm(_acvpConnectionString);

			List<OperatingEnvironmentLite> result = new List<OperatingEnvironmentLite>();

			try
			{
				var data = db.QueryFromProcedure("val.OEsGet", inParams: new
				{
					PageSize = pageSize,
					PageNumber = pageNumber
				});

				foreach (var oe in data)
				{
					Console.WriteLine(oe.id);
					result.Add(new OperatingEnvironmentLite
					{
						ID = oe.id,
						Name = oe.name
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}
			return result;
		}
	}
}

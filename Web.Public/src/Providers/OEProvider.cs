using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using Serilog;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class OEProvider : IOEProvider
	{
		private readonly ILogger<OEProvider> _logger;
		private readonly string _acvpPublicConnectionString;

		public OEProvider(IConnectionStringFactory connectionStringFactory, ILogger<OEProvider> logger)
		{
			_logger = logger;
			_acvpPublicConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}

		public OperatingEnvironment Get(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.OEGet", new
				{
					OEID = id
				});

				if (data == null)
				{
					throw new Exception($"Unable to find OE with id: {id}");
				}
				
				var result = new OperatingEnvironment
				{
					Name = data.Name
				};

				var dependencyData = db.QueryFromProcedure("val.DependenciesForOEGet", new
				{
					OEID = id
				});

				if (dependencyData != null)
				{
					result.DependencyIDs = new List<long>();
					foreach (var dependency in dependencyData)
					{
						result.DependencyIDs.Add(dependency.DependencyID);
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Unable to find OE");
				throw;
			}
		}

		public bool Exists(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.OEExists",
					new
					{
						oeId = id
					},
					new
					{
						exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError("Unable to validate existence of OE.", e);
				return false;
			}
		}

		public bool IsUsed(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.OEIsUsed",
					new
					{
						oeId = id
					},
					new
					{
						exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError("Unable to determine if OE in use.", e);
				return false;
			}
		}

		public (long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.QueryMultipleFromProcedure("val.OEFilteredListGet", inParams: new
				{
					Filter = filter,
					Limit = limit,
					Offset = offset,
					ORdelimiter = orDelimiter,
					ANDdelimiter = andDelimiter
				});

				//Create the objects to hold the final data
				long totalRecords;
				var oes = new List<OperatingEnvironment>();

				//Get the enumerator to manually iterate over the results
				using var enumerator = data.GetEnumerator();

				//Move to the first result set, the total records
				enumerator.MoveNext();
				var resultSet = enumerator.Current;

				totalRecords = resultSet.First().TotalRecords;

				//Move to the second result set, the OEs
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawOEs = resultSet.Select(x => (x.Id, x.Name)).ToList();

				//Move to the third result set, the dependencies
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawDependencies = resultSet.Select(x => (x.OEId, x.DependencyId)).ToList();

				//Build the list of OE objects
				foreach (var rawOE in rawOEs.OrderBy(x => x.Id))
				{
					var dependencies = rawDependencies.Where(x => x.OEId == rawOE.Id)?.Select(x => (long)x.DependencyId)?.ToList();

					oes.Add(new OperatingEnvironment
					{
						ID = rawOE.Id,
						Name = rawOE.Name,
						DependencyIDs = dependencies.Count > 0 ? dependencies : null
					});
				}

				return (totalRecords, oes);
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to get OE list", ex);
				throw;
			}
		}
	}
}
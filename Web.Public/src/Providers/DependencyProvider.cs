using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Serilog;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class DependencyProvider : IDependencyProvider
	{
		private readonly ILogger<DependencyProvider> _logger;
		private readonly string _acvpPublicConnectionString;

		public DependencyProvider(IConnectionStringFactory connectionStringFactory, ILogger<DependencyProvider> logger)
		{
			_logger = logger;
			_acvpPublicConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}

		public Dependency GetDependency(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.DependencyGet", new
				{
					DependencyID = id
				});

				if (data == null)
				{
					throw new Exception($"Unable to find dependency with id: {id}");
				}
				
				var result = new Dependency
				{
					ID = id,
					Description = data.Description,
					Name = data.Name,
					Type = data.Type
				};

				var dataAttributes = db.QueryFromProcedure("val.DependencyAttributeGet", new
				{
					DependencyID = id
				});

				if (dataAttributes != null)
				{
					result.Attributes = new Dictionary<string, object>();
					foreach (var attribute in dataAttributes)
					{
						result.Attributes.Add(attribute.Name, attribute.Value);
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Unable to find dependency");
				throw;
			}
		}

		public bool Exists(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.DependencyExists",
					new
					{
						dependencyId = id
					},
					new
					{
						exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError("Unable to validate existence of dependency.", e);
				return false;
			}
		}

		public (long TotalCount, List<Dependency> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.QueryMultipleFromProcedure("val.DependencyFilteredListGet", inParams: new
				{
					Filter = filter,
					Limit = limit,
					Offset = offset,
					ORdelimiter = orDelimiter,
					ANDdelimiter = andDelimiter
				});

				//Create the objects to hold the final data
				long totalRecords;
				var dependencies = new List<Dependency>();

				//Get the enumerator to manually iterate over the results
				using var enumerator = data.GetEnumerator();

				//Move to the first result set, the total records
				enumerator.MoveNext();
				var resultSet = enumerator.Current;

				totalRecords = resultSet.First().TotalRecords;

				//Move to the second result set, the dependencies
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawDependencies = resultSet.Select(x => (x.Id, x.DependencyType, x.Name, x.Description)).ToList();

				//Move to the third result set, the attributes
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawAttributes = resultSet.Select(x => (x.DependencyId, x.Name, x.Value)).ToList();

				//Build the list of Dependency objects
				foreach (var rawDependency in rawDependencies.OrderBy(x => x.Id))
				{
					var attributes = rawAttributes.Where(x => x.DependencyId == rawDependency.Id)?.ToDictionary(x => (string)x.Name, x => (object)x.Value);

					dependencies.Add(new Dependency
					{
						ID = rawDependency.Id,
						Type = rawDependency.DependencyType,
						Name = rawDependency.Name,
						Description = rawDependency.Description,
						Attributes = attributes.Count > 0 ? attributes : null
					});
				}

				return (totalRecords, dependencies);
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to get dependency list", ex);
				throw;
			}
		}
	}
}
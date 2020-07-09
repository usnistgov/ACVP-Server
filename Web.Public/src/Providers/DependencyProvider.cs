using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Web.Public.Helpers;
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
				var data = db.SingleFromProcedure("dbo.DependencyGet", inParams: new
				{
					DependencyId = id
				});

				if (data == null)
				{
					return null;
				}

				var result = new Dependency
				{
					ID = id,
					Description = data.Description,
					Name = data.Name,
					DependencyType = data.DependencyType
				};

				var dataAttributes = db.QueryFromProcedure("dbo.DependencyAttributeGet", inParams: new
				{
					DependencyId = id
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
				_logger.LogError(ex, "Unable to find dependency");
				throw;
			}
		}

		public bool Exists(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("dbo.DependencyExists", inParams:
					new
					{
						DependencyId = id
					}, outParams:
					new
					{
						Exists = false
					});

				return data.Exists;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to validate existence of dependency.");
				return false;
			}
		}

		public (long TotalCount, List<Dependency> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit)
		{
			//Build the query to get all the matching org IDs
			string query = "SELECT DependencyId FROM dbo.Dependencies D";

			if (orClauses.Count > 0)
			{
				//First OR clause gets special treatment - it starts with the WHERE, not an OR
				List<string> orStrings = new List<string>();

				foreach (OrClause orClause in orClauses)
				{
					List<string> andStrings = new List<string>();
					foreach (AndClause andClause in orClause.AndClauses)
					{
						switch (andClause.Property)
						{
							case "name":
								andStrings.Add($"Name {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "type":
								andStrings.Add($"DependencyType {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "description":
								andStrings.Add($"Description {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							default: break;
						}
					}

					//Combine all of the AND clauses with ANDs, and add as an OrClause
					orStrings.Add(string.Join(" AND ", andStrings));
				}

				//Combine all of the OR clauses with ORs and parens, add it to the query. Each OR clause needs to be wrapped in parens, so the join puts the closing and opening on ones in the middle, but need to add the opening for the first and closing for the last
				query += $" WHERE ({string.Join(") OR (", orStrings)})";
			}

			query += " ORDER BY D.DependencyId";

			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var dependencies = new List<Dependency>();

				//Get all the IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.DependencyId).ToArray();

				//Set the total records value since we have them all
				totalRecords = allIDs.Length;

				//If we didn't find any, can stop here
				if (totalRecords == 0)
				{
					return (0, dependencies);
				}

				//Get the page of IDs we care about for the rest
				Index startIndex = (Index)offset;
				Index endIndex = (Index)Math.Min(offset + limit, totalRecords); //end of a range is exclusive, which is weird given the start is inclusive

				long[] pagedIDs = allIDs[startIndex..endIndex];

				var data = db.QueryMultipleFromProcedure("dbo.DependencyFilteredListDataGet", inParams: new
				{
					IDs = string.Join(",", pagedIDs)
				});

				//Get the enumerator to manually iterate over the results
				using var enumerator = data.GetEnumerator();

				//Move to the first result set, the dependencies
				enumerator.MoveNext();
				var resultSet = enumerator.Current;

				var rawDependencies = resultSet.Select(x => (x.DependencyId, x.DependencyType, x.Name, x.Description)).ToList();

				//Move to the second result set, the attributes
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawAttributes = resultSet.Select(x => (x.DependencyId, x.Name, x.Value)).ToList();

				//Build the list of Dependency objects
				foreach (var rawDependency in rawDependencies.OrderBy(x => x.DependencyId))
				{
					var attributes = rawAttributes.Where(x => x.DependencyId == rawDependency.DependencyId)?.ToDictionary(x => (string)x.Name, x => (object)x.Value);

					dependencies.Add(new Dependency
					{
						ID = rawDependency.DependencyId,
						DependencyType = rawDependency.DependencyType,
						Name = rawDependency.Name,
						Description = rawDependency.Description,
						Attributes = attributes.Count > 0 ? attributes : null
					});
				}

				return (totalRecords, dependencies);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to get dependency list");
				throw;
			}
		}
	}
}
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
	public class OEProvider : IOEProvider
	{
		private readonly ILogger<OEProvider> _logger;
		private readonly string _acvpPublicConnectionString;
		private readonly IDependencyProvider _dependencyProvider;

		public OEProvider(IConnectionStringFactory connectionStringFactory, ILogger<OEProvider> logger, IDependencyProvider dependencyProvider)
		{
			_logger = logger;
			_acvpPublicConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
			_dependencyProvider = dependencyProvider;
		}

		public OperatingEnvironmentWithDependencies Get(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.OEGet", inParams: new
				{
					OEId = id
				});

				if (data == null)
				{
					return null;
				}
				
				var result = new OperatingEnvironmentWithDependencies
				{
					ID = id,
					Name = data.Name
				};

				var dependencyData = db.QueryFromProcedure("dbo.DependenciesForOEGet", inParams: new
				{
					OEId = id
				});

				if (dependencyData != null)
				{
					result.Dependencies = new List<Dependency>();
					foreach (var dependency in dependencyData)
					{
						result.Dependencies.Add(_dependencyProvider.GetDependency(dependency.DependencyId));
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Unable to find OE w/ ID {id}");
				return null;
			}
		}

		public bool Exists(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("dbo.OEExists", inParams:
					new
					{
						OEId = id
					}, outParams:
					new
					{
						Exists = false
					});

				return data.Exists;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to validate existence of OE.");
				return false;
			}
		}

		public bool IsUsed(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("dbo.OEIsUsed", inParams:
					new
					{
						OEId = id
					}, outParams:
					new
					{
						IsUsed = false
					});

				return data.IsUsed;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to determine if OE in use.");
				return false;
			}
		}

		public (long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(List<OrClause> orClauses, long offset, long limit)
		{
			//Build the query to get all the matching org IDs
			string query = "SELECT OEId FROM dbo.OEs OE";

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
							default: break;
						}
					}

					//Combine all of the AND clauses with ANDs, and add as an OrClause
					orStrings.Add(string.Join(" AND ", andStrings));
				}

				//Combine all of the OR clauses with ORs and parens, add it to the query. Each OR clause needs to be wrapped in parens, so the join puts the closing and opening on ones in the middle, but need to add the opening for the first and closing for the last
				query += $" WHERE ({string.Join(") OR (", orStrings)})";
			}

			query += " ORDER BY OE.OEId";

			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var oes = new List<OperatingEnvironment>();

				//Get all the org IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.OEId).ToArray();

				//Set the total records value since we have them all
				totalRecords = allIDs.Length;

				//If we didn't find any, can stop here
				if (totalRecords > 0)
				{
					//Get the page of IDs we care about for the rest
					Index startIndex = (Index)offset;
					Index endIndex = (Index)Math.Min(offset + limit, totalRecords); //end of a range is exclusive, which is weird given the start is inclusive

					long[] pagedIDs = allIDs[startIndex..endIndex];

					var data = db.QueryMultipleFromProcedure("dbo.OEFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the OEs
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawOEs = resultSet.Select(x => (x.OEId, x.Name)).ToList();

					//Move to the second result set, the dependencies
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawDependencies = resultSet.Select(x => (x.OEId, x.DependencyId)).ToList();

					//Build the list of OE objects
					foreach (var rawOE in rawOEs.OrderBy(x => x.OEId))
					{
						var dependencies = rawDependencies.Where(x => x.OEId == rawOE.OEId)?.Select(x => (long)x.DependencyId)?.ToList();

						oes.Add(new OperatingEnvironment
						{
							ID = rawOE.OEId,
							Name = rawOE.Name,
							DependencyIDs = dependencies.Count > 0 ? dependencies : null
						});
					}
				}

				return (totalRecords, oes);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to get OE list");
				throw;
			}
		}
	}
}
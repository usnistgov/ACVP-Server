using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using Serilog;
using Web.Public.Models;
using Web.Public.Helpers;

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
				var data = db.SingleFromProcedure("val.OEGet", new
				{
					OEID = id
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

				var dependencyData = db.QueryFromProcedure("val.DependenciesForOEGet", new
				{
					OEID = id
				});

				if (dependencyData != null)
				{
					result.Dependencies = new List<Dependency>();
					foreach (var dependency in dependencyData)
					{
						result.Dependencies.Add(_dependencyProvider.GetDependency(dependency.DependencyID));
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
				_logger.LogError(e, "Unable to validate existence of OE.");
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
				_logger.LogError(e, "Unable to determine if OE in use.");
				return false;
			}
		}

		public (long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(List<OrClause> orClauses, long offset, long limit)
		{
			//Build the query to get all the matching org IDs
			string query = "SELECT id FROM val.VALIDATION_OE OE";

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
								andStrings.Add($"name {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
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

			query += " ORDER BY O.id";

			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var oes = new List<OperatingEnvironment>();

				//Get all the org IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.id).ToArray();

				//Set the total records value since we have them all
				totalRecords = allIDs.Length;

				//If we didn't find any, can stop here
				if (totalRecords > 0)
				{
					//Get the page of IDs we care about for the rest
					Index startIndex = (Index)offset;
					Index endIndex = (Index)Math.Min(offset + limit, totalRecords); //end of a range is exclusive, which is weird given the start is inclusive

					long[] pagedIDs = allIDs[startIndex..endIndex];

					var data = db.QueryMultipleFromProcedure("val.OEFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the OEs
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawOEs = resultSet.Select(x => (x.Id, x.Name)).ToList();

					//Move to the second result set, the dependencies
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
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Web.Public.Helpers;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public class ImplementationProvider : IImplementationProvider
	{
		private readonly ILogger<ImplementationProvider> _logger;
		private readonly string _acvpPublicConnectionString;

		public ImplementationProvider(IConnectionStringFactory connectionStringFactory, ILogger<ImplementationProvider> logger)
		{
			_logger = logger;
			_acvpPublicConnectionString = connectionStringFactory.GetMightyConnectionString("ACVPPublic");
		}

		public Implementation Get(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ImplementationGet", new
				{
					ID = id
				});

				if (data == null)
				{
					return null;
				}
				
				var result = new Implementation
				{
					ID = id,
					Name = data.ImplementationName,
					Type = (ImplementationType)data.ImplementationType,
					Version = data.ImplementationVersion,
					Description = data.ImplementationDescription,
					Website = data.Website,
					AddressID = data.AddressID,
					OrganizationID = data.OrganizationID
				};

				var personData = db.QueryFromProcedure("val.PersonsForImplementationGet", new
				{
					ImplementationID = id
				});

				if (personData != null)
				{
					result.ContactIDs = new List<long>();
					foreach (var person in personData)
					{
						result.ContactIDs.Add(person.PersonID);
					}
				}

				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error on Implementation GET");
				throw;
			}
		}

		public bool Exists(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.ImplementationExists",
					new
					{
						implementationId = id
					},
					new
					{
						exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to validate existence of implementation.");
				return false;
			}
		}
		
		public bool IsUsed(long id)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.ImplementationIsUsed",
					new
					{
						implementationId = id
					},
					new
					{
						inUse = false
					});

				return data.inUse;
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Unable to determine if implementation in use.");
				return false;
			}
		}

		public (long TotalCount, List<Implementation> Organizations) GetFilteredList(List<OrClause> orClauses, long offset, long limit)
		{
			//Build the query to get all the matching org IDs
			string query = "SELECT P.id FROM val.PRODUCT_INFORMATION P INNER JOIN ref.MODULE_TYPE M ON P.module_type = M.id";

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
								andStrings.Add($"P.module_name {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "version":
								andStrings.Add($"P.module_version {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "website":
								andStrings.Add($"P.product_url {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "description":
								andStrings.Add($"P.implementation_description {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "type":
								andStrings.Add($"M.[name] {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "vendorId":
								andStrings.Add($"P.module_version {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, long.Parse(andClause.Value))}");
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

			query += " ORDER BY P.id";

			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var implementations = new List<Implementation>();

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

					var data = db.QueryMultipleFromProcedure("val.ImplementationFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the implementations
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawImplementations = resultSet.Select(x => (x.Id, x.OrganizationId, x.Name, x.Version, x.Type, x.Website, x.Description, x.AddressId)).ToList();

					//Move to the second result set, the contacts
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawContacts = resultSet.Select(x => (x.ImplementationId, x.PersonId, x.OrderIndex)).ToList();

					//Build the list of Implementation objects
					foreach (var rawImplementation in rawImplementations.OrderBy(x => x.Id))
					{
						var contacts = rawContacts.Where(x => x.ImplementationId == rawImplementation.Id)?.OrderBy(x => x.OrderIndex)?.Select(x => (long)x.PersonId)?.ToList();

						implementations.Add(new Implementation
						{
							ID = rawImplementation.Id,
							Name = rawImplementation.Name,
							Description = rawImplementation.Description,
							Type = (ImplementationType)rawImplementation.Type,
							Version = rawImplementation.Version,
							Website = rawImplementation.Website,
							OrganizationID = rawImplementation.OrganizationId,
							AddressID = rawImplementation.AddressId,
							ContactIDs = contacts.Count > 0 ? contacts : null,
						});
					}
				}

				return (totalRecords, implementations);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unable to get implementation list");
				throw;
			}
		}
	}
}
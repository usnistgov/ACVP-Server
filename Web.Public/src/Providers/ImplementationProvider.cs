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
				var data = db.SingleFromProcedure("dbo.ImplementationGet", inParams: new
				{
					ImplementationId = id
				});

				if (data == null)
				{
					return null;
				}
				
				var result = new Implementation
				{
					ID = id,
					Name = data.ImplementationName,
					Type = (ImplementationType)data.ImplementationTypeId,
					Version = data.ImplementationVersion,
					Description = data.ImplementationDescription,
					Website = data.Url,
					AddressID = data.AddressId,
					OrganizationID = data.OrganizationId
				};

				var contactData = db.QueryFromProcedure("dbo.ImplementationContactsGet", inParams: new
				{
					ImplementationID = id
				});

				if (contactData != null)
				{
					result.ContactIDs = new List<long>();
					foreach (var contact in contactData)
					{
						result.ContactIDs.Add(contact.PersonId);
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
				var data = db.ExecuteProcedure("dbo.ImplementationExists", inParams:
					new
					{
						ImplementationId = id
					}, outParams:
					new
					{
						Exists = false
					});

				return data.Exists;
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
				var data = db.ExecuteProcedure("dbo.ImplementationIsUsed",
					new
					{
						ImplementationId = id
					},
					new
					{
						InUse = false
					});

				return data.InUse;
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
			string query = "SELECT I.ImplementationId FROM dbo.Implementations I INNER JOIN dbo.ImplementationTypes T ON T.ImplementationTypeId = I.ImplementationTypeId";

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
								andStrings.Add($"I.ImplementationName {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "version":
								andStrings.Add($"I.ImplementationVersion {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "website":
								andStrings.Add($"I.[Url] {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "description":
								andStrings.Add($"I.ImplementationDescription {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "type":
								andStrings.Add($"T.ImplementationTypeName {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, andClause.Value)}");
								break;
							case "vendorId":
								andStrings.Add($"I.OrganizationId {FilterHelpers.GenerateOperatorAndValue(andClause.Operator, long.Parse(andClause.Value))}");
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

			query += " ORDER BY I.ImplementationId";

			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				//Create the objects to hold the final data
				long totalRecords;
				var implementations = new List<Implementation>();

				//Get all the org IDs that match the query
				long[] allIDs = db.Query(query).Select(x => (long)x.ImplementationId).ToArray();

				//Set the total records value since we have them all
				totalRecords = allIDs.Length;

				//If we didn't find any, can stop here
				if (totalRecords > 0)
				{
					//Get the page of IDs we care about for the rest
					Index startIndex = (Index)offset;
					Index endIndex = (Index)Math.Min(offset + limit, totalRecords); //end of a range is exclusive, which is weird given the start is inclusive

					long[] pagedIDs = allIDs[startIndex..endIndex];

					var data = db.QueryMultipleFromProcedure("dbo.ImplementationFilteredListDataGet", inParams: new
					{
						IDs = string.Join(",", pagedIDs)
					});

					//Get the enumerator to manually iterate over the results
					using var enumerator = data.GetEnumerator();

					//Move to the first result set, the implementations
					enumerator.MoveNext();
					var resultSet = enumerator.Current;

					var rawImplementations = resultSet.Select(x => (x.ImplementationId, x.OrganizationId, x.ImplementationName, x.ImplementationVersion, x.ImplementationTypeId, x.Url, x.ImplementationDescription, x.AddressId)).ToList();

					//Move to the second result set, the contacts
					enumerator.MoveNext();
					resultSet = enumerator.Current;

					var rawContacts = resultSet.Select(x => (x.ImplementationId, x.PersonId, x.OrderIndex)).ToList();

					//Build the list of Implementation objects
					foreach (var rawImplementation in rawImplementations.OrderBy(x => x.ImplementationId))
					{
						var contacts = rawContacts.Where(x => x.ImplementationId == rawImplementation.ImplementationId)?.OrderBy(x => x.OrderIndex)?.Select(x => (long)x.PersonId)?.ToList();

						implementations.Add(new Implementation
						{
							ID = rawImplementation.ImplementationId,
							Name = rawImplementation.ImplementationName,
							Description = rawImplementation.ImplementationDescription,
							Type = (ImplementationType)rawImplementation.ImplementationTypeId,
							Version = rawImplementation.ImplementationVersion,
							Website = rawImplementation.Url,
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
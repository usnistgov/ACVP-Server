using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using Serilog;
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
					TypeString = data.ImplementationType,
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
				Log.Error(ex, "Error on Implementation GET");
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
				_logger.LogError("Unable to validate existence of implementation.", e);
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
						exists = false
					});

				return data.exists;
			}
			catch (Exception e)
			{
				_logger.LogError("Unable to determine if implementation in use.", e);
				return false;
			}
		}

		public (long TotalCount, List<Implementation> Organizations) GetFilteredList(string filter, long offset, long limit, string orDelimiter, string andDelimiter)
		{
			var db = new MightyOrm(_acvpPublicConnectionString);

			try
			{
				var data = db.QueryMultipleFromProcedure("val.ImplementationFilteredListGet", inParams: new
				{
					Filter = filter,
					Limit = limit,
					Offset = offset,
					ORdelimiter = orDelimiter,
					ANDdelimiter = andDelimiter
				});

				//Create the objects to hold the final data
				long totalRecords;
				var implementations = new List<Implementation>();

				//Get the enumerator to manually iterate over the results
				using var enumerator = data.GetEnumerator();

				//Move to the first result set, the total records
				enumerator.MoveNext();
				var resultSet = enumerator.Current;

				totalRecords = resultSet.First().TotalRecords;

				//Move to the second result set, the implementations
				enumerator.MoveNext();
				resultSet = enumerator.Current;

				var rawImplementations = resultSet.Select(x => (x.Id, x.OrganizationId, x.Name, x.Version, x.Type, x.Website, x.Description, x.AddressId)).ToList();

				//Move to the third result set, the contacts
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

				return (totalRecords, implementations);
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to get implementation list", ex);
				throw;
			}
		}
	}
}
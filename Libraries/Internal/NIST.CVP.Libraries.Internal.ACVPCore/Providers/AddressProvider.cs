using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class AddressProvider : IAddressProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<DependencyProvider> _logger;

		public AddressProvider(IConnectionStringFactory connectionStringFactory, ILogger<DependencyProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result Delete(long addressID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.AddressDelete", inParams: new
				{
					AddressId = addressID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllForOrganization(long organizationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.AddressDeleteAllForOrganization", inParams: new
				{
					OrganizationId = organizationID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public List<Address> GetAllForOrganization(long organizationID)
		{
			List<Address> addresses = new List<Address>();

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.QueryFromProcedure("dbo.AddressesForOrganizationGet", inParams: new
				{
					OrganizationID = organizationID
				});

				foreach (var row in data)
				{
					addresses.Add(new Address
					{
						ID = row.AddressId,
						OrganizationID = organizationID,
						Street1 = row.Street1,
						Street2 = row.Street2,
						Street3 = row.Street3,
						Locality = row.Locality,
						Region = row.Region,
						PostalCode = row.PostalCode,
						Country = row.Country
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return addresses;
		}

		public Address Get(long addressID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.AddressGet", inParams: new
				{
					AddressId = addressID
				});

				return new Address
				{
					ID = data.AddressId,
					OrganizationID = data.OrganizationId,
					Street1 = data.Street1,
					Street2 = data.Street2,
					Street3 = data.Street3,
					Locality = data.Locality,
					Region = data.Region,
					PostalCode = data.PostalCode,
					Country = data.Country
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}


		public InsertResult Insert(long organizationID, string street1, string street2, string street3, string locality, string region, string postalCode, string country, int orderIndex)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ScalarFromProcedure("dbo.AddressInsert", inParams: new
				{
					OrganizationId = organizationID,
					OrderIndex = orderIndex,
					Street1 = street1,
					Street2 = street2,
					Street3 = street3,
					Locality = locality,
					Region = region,
					PostalCode = postalCode,
					Country = country
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert Address");
				}
				else
				{
					return new InsertResult((long)data);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result Update(long addressID, string street1, string street2, string street3, string locality, string region, string postalCode, string country, int orderIndex, bool street1Updated, bool street2Updated, bool street3Updated, bool localityUpdated, bool regionUpdated, bool postalCodeUpdated, bool countryUpdated)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.AddressUpdate", inParams: new
				{
					AddressId = addressID,
					Street1 = street1,
					Street2 = street2,
					Street3 = street3,
					Locality = locality,
					Region = region,
					PostalCode = postalCode,
					Country = country,
					OrderIndex = orderIndex,
					Street1Updated = street1Updated,
					Street2Updated = street2Updated,
					Street3Updated = street3Updated,
					LocalityUpdated = localityUpdated,
					RegionUpdated = regionUpdated,
					PostalCodeUpdated = postalCodeUpdated,
					CountryUpdated = countryUpdated
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public bool AddressIsUsedOtherThanOrg(long addressID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.AddressIsUsedOtherThanOrg", inParams: new
				{
					AddressId = addressID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return true;    //Default to true so we don't try do delete when we shouldn't
			}
		}

		public bool AddressExists(long addressID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.AddressExists", inParams: new
				{
					AddressId = addressID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;    //Default to false so we don't try do use it when we don't know if it exists
			}
		}
	}
}

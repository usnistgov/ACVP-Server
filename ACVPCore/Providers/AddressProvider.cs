using System;
using System.Collections.Generic;
using System.Linq;
using ACVPCore.Models;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
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
				db.ExecuteProcedure("val.AddressDelete", inParams: new
				{
					AddressID = addressID
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
				db.ExecuteProcedure("val.AddressDeleteAllForOrganization", inParams: new
				{
					OrganizationID = organizationID
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
			var db = new MightyOrm<Address>(_acvpConnectionString);

			try
			{
				return db.QueryFromProcedure("val.AddressesForOrganizationGet", inParams: new
				{
					OrganizationID = organizationID
				}).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new List<Address>();
			}
		}

		public Address Get(long addressID)
		{
			var db = new MightyOrm<Address>(_acvpConnectionString);

			try
			{
				return db.SingleFromProcedure("val.AddressGet", inParams: new
				{
					AddressId = addressID
				});
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
				var data = db.ScalarFromProcedure("val.AddressInsert", inParams: new
				{
					OrganizationID = organizationID,
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
				db.ExecuteProcedure("val.AddressUpdate", inParams: new
				{
					AddressID = addressID,
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
				return (bool)db.ScalarFromProcedure("val.AddressIsUsedOtherThanOrg", inParams: new
				{
					AddressID = addressID
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
				return (bool)db.ScalarFromProcedure("val.AddressExists", inParams: new
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

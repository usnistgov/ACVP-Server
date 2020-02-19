using System;
using ACVPCore.Models;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
{
	public class ImplementationProvider : IImplementationProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<DependencyProvider> _logger;

		public ImplementationProvider(IConnectionStringFactory connectionStringFactory, ILogger<DependencyProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Implementation Get(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ImplementationGet", inParams: new
				{
					implementationID = implementationID
				});

				if (data != null)
				{

					Organization organization = new Organization
					{
						ID = data.organization_id,
						Name = data.organization_name,
						Url = data.organization_url,
						VoiceNumber = data.organization_voice_number,
						FaxNumber = data.organization_fax_number,
						Parent = (data.organization_parent_id == null) ? null : new Organization(){ ID = data.organization_parent_id }
					};

					Address address = new Address
					{
						ID = data.address_id,
						Street1 = data.address_street1,
						Street2 = data.address_street2,
						Street3 = data.address_street3,
						Locality = data.address_locality,
						Region = data.address_region,
						PostalCode = data.address_postal_code,
						Country = data.address_country
					};

					return new Implementation
					{
						ID = implementationID,
						Vendor = organization,
						Address = address,
						URL =  data.product_url,
						Name = data.module_name,
						Type = Enum.Parse(typeof(ACVPCore.Models.Implementation.ModuleType), data.module_type),
						Version = data.module_version,
						Description = data.module_description,
						ITAR = data.product_itar
					};

				}
				else
				{
					return null;
				}

				data = db.SingleFromProcedure("val.ImplementationGet", inParams: new
				{
					DependencyID = implementationID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}

		public Result Delete(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.ImplementationDelete @0", implementationID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllContacts(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.ImplementationContactsDeleteAll @0", implementationID);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(string name, string description, ImplementationType type, string version, string website, long organizationID, long addressID, bool isITAR)
		{
			if (string.IsNullOrWhiteSpace(name)) return new InsertResult("Invalid name value");
			if (string.IsNullOrWhiteSpace(version)) return new InsertResult("Invalid version value");
			if (string.IsNullOrWhiteSpace(description)) return new InsertResult("Invalid description value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ImplementationInsert", inParams: new
				{
					Name = name,
					Description = description,
					Type = type,
					Version = version,
					Website = website,
					OrganizationID = organizationID,
					AddressID = addressID,
					IsITAR = isITAR
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert Implementation");
				}
				else
				{
					return new InsertResult((long)data.ImplementationID);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result InsertContact(long implementationID, long personID, int orderIndex)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//There is no ID on the record, so don't return anything
				db.ExecuteProcedure("val.ImplementationContactInsert", inParams: new
				{
					ImplementationID = implementationID,
					PersonID = personID,
					OrderIndex = orderIndex
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result Update(long implementationID, string name, string description, ImplementationType type, string version, string website, long? organizationID, long? addressID, bool nameUpdated, bool descriptionUpdated, bool typeUpdated, bool versionUpdated, bool websiteUpdated, bool organizationIDUpdated, bool addressIDUpdated)
		{
			if (nameUpdated && string.IsNullOrWhiteSpace(name)) return new Result("Invalid name value");
			if (versionUpdated && string.IsNullOrWhiteSpace(version)) return new Result("Invalid version value");
			if (descriptionUpdated && string.IsNullOrWhiteSpace(description)) return new Result("Invalid description value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("val.ImplementationUpdate", inParams: new
				{
					ImplementationID = implementationID,
					Name = name,
					Description = description,
					Type = type,
					Version = version,
					Website = website,
					OrganizationID = organizationID,
					AddressID = addressID,
					NameUpdated = nameUpdated,
					DescriptionUpdated = descriptionUpdated,
					TypeUpdated = typeUpdated,
					VersionUpdated = versionUpdated,
					WebsiteUpdated = websiteUpdated,
					OrganizationIDUpdated = organizationIDUpdated,
					AddressIDUpdated = addressIDUpdated
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public bool ImplementationIsUsed(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.ImplementationIsUsed", inParams: new
				{
					ImplementationID = implementationID
				});

				return data.IsUsed;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return true;    //Default to true so we don't try do delete when we shouldn't
			}
		}

		public bool ImplementationExists(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("val.ImplementationExists", inParams: new
				{
					ImplementationId = implementationID
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

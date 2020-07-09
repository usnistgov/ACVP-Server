using System;
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class ImplementationProvider : IImplementationProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<ImplementationProvider> _logger;

		public ImplementationProvider(IConnectionStringFactory connectionStringFactory, ILogger<ImplementationProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Implementation Get(long implementationID)
		{
			var db = new MightyOrm(_acvpConnectionString);
			try
			{
				var data = db.SingleFromProcedure("dbo.ImplementationGet", inParams: new
				{
					ImplementationID = implementationID
				});

				if (data != null)
				{
					return new Implementation
					{
						ID = implementationID,
						URL = data.Url,
						Name = data.ImplementationName,
						Type = (ImplementationType)data.ImplementationTypeId,
						Version = data.ImplementationVersion,
						Description = data.ImplementationDescription,
						ITAR = data.ITAR,
						Vendor = new Organization
						{
							ID = data.OrganizationId,
							Name = data.OrganizationName,
							Url = data.OrganizationUrl,
							VoiceNumber = data.VoiceNumber,
							FaxNumber = data.FaxNumber,
							Parent = (data.ParentOrganizationId == null) ? null : new OrganizationLite() { ID = data.ParentOrganizationId }
						},
						Address = new Address
						{
							ID = data.AddressId,
							Street1 = data.Street1,
							Street2 = data.Street2,
							Street3 = data.Street3,
							Locality = data.Locality,
							Region = data.Region,
							PostalCode = data.PostalCode,
							Country = data.Country
						}
					};
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}

		public PagedEnumerable<Implementation> GetImplementations(ImplementationListParameters param)
		{
			long totalRecords = 0;
			var db = new MightyOrm(_acvpConnectionString);
			try
			{
				var data = db.QueryWithExpando("dbo.ImplementationsGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					ImplementationId = param.Id,
					Name = param.Name,
					Description = param.Description
				},
					outParams: new
					{
						totalRecords = (long)0
					});

				List<Implementation> implementations = new List<Implementation>();

				if (data != null)
				{
					foreach (var row in data.Data)
					{
						implementations.Add(new Implementation
						{
							ID = row.ImplementationId,
							URL = row.Url,
							Name = row.ImplementationName,
							Type = (ImplementationType)row.ImplementationTypeId,
							Version = row.ImplementationVersion,
							Description = row.ImplementationDescription,
							ITAR = row.ITAR,
							Vendor = new Organization
							{
								ID = row.OrganizationId,
								Name = row.OrganizationName,
								Url = row.OrganizationUrl,
								VoiceNumber = row.VoiceNumber,
								FaxNumber = row.FaxNumber,
								Parent = (row.ParentOrganizationId == null) ? null : new OrganizationLite() { ID = row.ParentOrganizationId }
							},
							Address = new Address
							{
								ID = row.AddressId,
								Street1 = row.Street1,
								Street2 = row.Street2,
								Street3 = row.Street3,
								Locality = row.Locality,
								Region = row.Region,
								PostalCode = row.PostalCode,
								Country = row.Country
							}
						});
					}

					totalRecords = (long)data.ResultsExpando.totalRecords;
				}
				else
				{
					return null;
				}

				return implementations.ToPagedEnumerable(param.PageSize, param.Page, totalRecords);
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
				db.ExecuteProcedure("dbo.ImplementationDelete", inParams: new { ImplementationId = implementationID });
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
				db.ExecuteProcedure("dbo.ImplementationContactsDeleteAll", inParams: new { ImplementationId = implementationID });
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
				var data = db.SingleFromProcedure("dbo.ImplementationInsert", inParams: new
				{
					Name = name,
					Description = description,
					ImplementationTypeId = type,
					Version = version,
					Website = website,
					OrganizationId = organizationID,
					AddressId = addressID,
					IsITAR = isITAR
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert Implementation");
				}
				else
				{
					return new InsertResult((long)data.ImplementationId);
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
				db.ExecuteProcedure("dbo.ImplementationContactInsert", inParams: new
				{
					ImplementationId = implementationID,
					PersonId = personID,
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
				db.ExecuteProcedure("dbo.ImplementationUpdate", inParams: new
				{
					ImplementationId = implementationID,
					Name = name,
					Description = description,
					ImplementationTypeId = type,
					Version = version,
					Website = website,
					OrganizationId = organizationID,
					AddressId = addressID,
					NameUpdated = nameUpdated,
					DescriptionUpdated = descriptionUpdated,
					ImplementationTypeIdUpdated = typeUpdated,
					VersionUpdated = versionUpdated,
					WebsiteUpdated = websiteUpdated,
					OrganizationIdUpdated = organizationIDUpdated,
					AddressIdUpdated = addressIDUpdated
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
				return (bool)db.ScalarFromProcedure("dbo.ImplementationIsUsed", inParams: new
				{
					ImplementationId = implementationID
				});
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
				return (bool)db.ScalarFromProcedure("dbo.ImplementationExists", inParams: new
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

		public List<Person> GetContacts(long implementationID)
		{
			List<Person> result = new List<Person>();
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.QueryFromProcedure("dbo.ImplementationContactsGet", inParams: new { ImplementationID = implementationID });

				foreach (var row in data)
				{
					result.Add(new Person
					{
						ID = row.PersonId,
						Name = row.FullName
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return result;
		}
	}
}

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPCore.Providers
{
	public class PersonProvider : IPersonProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<PersonProvider> _logger;

		public PersonProvider(IConnectionStringFactory connectionStringFactory, ILogger<PersonProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}
		public Person Get(long personID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			var personResult = new Person();

			personResult.Organization = new Organization();
			personResult.PhoneNumbers = new List<PersonPhone>();
			personResult.EmailAddresses = new List<string>();

			try
			{
				// First Section - Basic Personal data
				var personData = db.SingleFromProcedure("dbo.PersonGet", inParams: new
				{
					PersonId = personID
				});

				personResult.ID = personID;
				personResult.Name = personData.FullName;

				//TODO - a query to get a person should not be bringing back all this data about an org
				personResult.Organization.ID = personData.OrganizationId;
				personResult.Organization.Name = personData.OrganizationName;
				personResult.Organization.Url = personData.OrganizationUrl;
				personResult.Organization.VoiceNumber = personData.VoiceNumber;
				personResult.Organization.FaxNumber = personData.FaxNumber;

				// Second section - Personal Email Data, a one-to-many relationship
				personResult.EmailAddresses = GetEmailAddresses(personID);

				// Third section - Personal Phone Data, a one-to-many relationship
				var personPhoneData = db.QueryFromProcedure("dbo.PersonGetPhones", inParams: new
				{
					PersonId = personID
				});

				foreach (var phone in personPhoneData)
				{
					personResult.PhoneNumbers.Add(new PersonPhone
					{
						Type = phone.PhoneNumberType,
						Number = phone.PhoneNumber
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return personResult;
		}

		public List<string> GetEmailAddresses(long personID)
		{
			List<string> result = new List<string>();
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.QueryFromProcedure("dbo.PersonGetEmails", inParams: new
				{
					PersonID = personID
				});

				foreach (var row in data)
				{
					result.Add(row.EmailAddress);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return result;
		}

		public Result Delete(long personID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.PersonDelete", inParams: new { PersonId = personID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllEmails(long personID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.PersonEmailDeleteAll", inParams: new { PersonId = personID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public Result DeleteAllPhoneNumbers(long personID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.PersonPhoneDeleteAll", inParams: new { PersonId = personID });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public InsertResult Insert(string name, long organizationID)
		{
			if (string.IsNullOrWhiteSpace(name)) return new InsertResult("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.PersonInsert", inParams: new
				{
					Name = name,
					OrganizationId = organizationID
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert Person");
				}
				else
				{
					return new InsertResult((long)data.PersonId);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result InsertEmailAddress(long personID, string emailAddress, int orderIndex)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				//There is no ID on the record, so don't return anything
				db.ExecuteProcedure("dbo.PersonEmailInsert", inParams: new
				{
					PersonId = personID,
					EmailAddress = emailAddress,
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

		public Result InsertPhoneNumber(long personID, string type, string number, int orderIndex)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.PersonPhoneInsert", inParams: new
				{
					PersonId = personID,
					OrderIndex = orderIndex,
					Type = type,
					Number = number
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new InsertResult(ex.Message);
			}
		}

		public Result Update(long personID, string name, long? organizationID, bool nameUpdated, bool organizationIDUpdated)
		{
			if (nameUpdated && string.IsNullOrWhiteSpace(name)) return new Result("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.PersonUpdate", inParams: new
				{
					PersonId = personID,
					Name = name,
					OrganizationId = organizationID,
					NameUpdated = nameUpdated,
					OrganizationIdUpdated = organizationIDUpdated
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public bool PersonIsUsed(long personID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.PersonIsUsed", inParams: new
				{
					PersonID = personID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return true;    //Default to true so we don't try do delete when we shouldn't
			}
		}

		public bool PersonExists(long personID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				return (bool)db.ScalarFromProcedure("dbo.PersonExists", inParams: new
				{
					PersonId = personID
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return false;    //Default to false so we don't try do use it when we don't know if it exists
			}
		}

		public PagedEnumerable<PersonLite> Get(PersonListParameters param)
		{
			var db = new MightyOrm<PersonLite>(_acvpConnectionString);

			List<PersonLite> result = new List<PersonLite>();
			long totalRecords = 0;

			try
			{
				var dbResult = db.QueryWithExpando("dbo.PersonsGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					PersonId = param.Id,
					Name = param.Name,
					OrganizationName = param.OrganizationName
				}, outParams: new
				{
					totalRecords = (long)0
				});

				result = dbResult.Data;
				totalRecords = dbResult.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return result.ToPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}

		public List<PersonLite> GetForOrganization(long organizationID)
		{
			List<PersonLite> result = new List<PersonLite>();
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.QueryFromProcedure("dbo.PersonsForOrgGet", inParams: new { OrganizationId = organizationID });

				foreach (var row in data)
				{
					result.Add(new PersonLite
					{
						ID = row.PersonId,
						Name = row.FullName
					});
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return result;
		}
	}
}

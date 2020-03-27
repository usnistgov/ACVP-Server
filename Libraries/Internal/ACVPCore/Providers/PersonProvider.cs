using System;
using System.Collections.Generic;
using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.Results;

namespace ACVPCore.Providers
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
			personResult.Notes = new List<PersonNote>();

			try
			{
				// First Section - Basic Personal data
				var personData = db.SingleFromProcedure("val.PersonGet", inParams: new
				{
					PersonID = personID
				});

				personResult.ID = personData.person_id;
				personResult.Name = personData.person_name;

				personResult.Organization.ID = personData.organization_id;
				personResult.Organization.Name = personData.organization_name;
				personResult.Organization.Url = personData.organization_url;
				personResult.Organization.VoiceNumber = personData.organization_voice;
				personResult.Organization.FaxNumber = personData.organization_fax;

				// Second section - Personal Email Data, a one-to-many relationship
				var personEmailData = db.QueryFromProcedure("val.PersonGetEmails", inParams: new
				{
					PersonID = personID
				});

				foreach (var email in personEmailData) 
				{ 
					personResult.EmailAddresses.Add(email.email_address); 
				}

				// Second section - Personal Phone Data, a one-to-many relationship
				var personPhoneData = db.QueryFromProcedure("val.PersonGetPhones", inParams: new
				{
					PersonID = personID
				});

				foreach (var phone in personPhoneData)
				{
					personResult.PhoneNumbers.Add(new PersonPhone {
						Type = phone.phone_number_type,
						Number = phone.phone_number 
					});
				}

				// Third Section - PErsonal Notes
				var personNoteData = db.QueryFromProcedure("val.PersonGetNotes", inParams: new
				{
					PersonID = personID
				});

				foreach (var note in personNoteData)
				{
					personResult.Notes.Add(new PersonNote
					{
						ID = note.id,
						Time = note.note_date,
						Subject = note.note_subject,
						Body = note.note
					});
				}
			}
			catch(Exception ex)
			{
				_logger.LogError(ex.Message);
			}

			return personResult;
		}
		public Result Delete(long personID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.Execute("val.PersonDelete @0", personID);
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
				db.Execute("val.PersonEmailDeleteAll @0", personID);
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
				db.Execute("val.PersonPhoneDeleteAll @0", personID);
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
				var data = db.SingleFromProcedure("val.PersonInsert", inParams: new
				{
					Name = name,
					OrganizationID = organizationID
				});

				if (data == null)
				{
					return new InsertResult("Failed to insert Person");
				}
				else
				{
					return new InsertResult((long)data.PersonID);
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
				db.ExecuteProcedure("val.PersonEmailInsert", inParams: new
				{
					PersonID = personID,
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
				//There is no ID on the record, so don't return anything
				db.ExecuteProcedure("val.PersonPhoneInsert", inParams: new
				{
					PersonID = personID,
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
			if (string.IsNullOrWhiteSpace(name)) return new Result("Invalid name value");

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("val.PersonUpdate", inParams: new
				{
					PersonID = personID,
					Name = name,
					OrganizationID = organizationID,
					NameUpdated = nameUpdated,
					OrganizationIDUpdated = organizationIDUpdated
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
				var data = db.SingleFromProcedure("val.PersonIsUsed", inParams: new
				{
					PersonID = personID
				});

				return data.IsUsed;
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
				return (bool)db.ScalarFromProcedure("val.PersonExists", inParams: new
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
				var dbResult = db.QueryWithExpando("val.PersonsGet", inParams: new
				{
					PageSize = param.PageSize,
					PageNumber = param.Page,
					Id = param.Id,
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
			
			return result.WrapPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}
	}
}

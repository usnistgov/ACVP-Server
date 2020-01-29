using System;
using ACVPCore.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

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
	}
}

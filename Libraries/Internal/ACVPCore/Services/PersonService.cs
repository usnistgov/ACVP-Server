using ACVPCore.Models;
using ACVPCore.Models.Parameters;
using ACVPCore.Providers;
using ACVPCore.Results;
using NIST.CVP.Enumerables;
using NIST.CVP.Results;

namespace ACVPCore.Services
{
	public class PersonService : IPersonService
	{
		private readonly IPersonProvider _personProvider;

		public PersonService(IPersonProvider personProvider)
		{
			_personProvider = personProvider;
		}

		public Person Get(long personID)
		{
			return _personProvider.Get(personID);
		}
		public DeleteResult Delete(long personID)
		{
			Result result;

			//Check to see if the dependency is used, in which case it can't be deleted
			if (PersonIsUsed(personID))
			{
				return new DeleteResult(DeleteResult.ErrorReason.IsInUse);
			}

			//Delete all person phone numbers
			result = _personProvider.DeleteAllPhoneNumbers(personID);

			if (!result.IsSuccess)
			{
				return new DeleteResult(result);
			}

			//Delete all person emails
			result = _personProvider.DeleteAllEmails(personID);

			if (!result.IsSuccess)
			{
				return new DeleteResult(result);
			}

			//Delete the Person
			result = _personProvider.Delete(personID);

			return new DeleteResult(result);

		}

		public PersonResult Create(PersonCreateParameters parameters)
		{
			//Insert the person record
			InsertResult personInsertResult = _personProvider.Insert(parameters.Name, parameters.OrganizationID);

			if (!personInsertResult.IsSuccess)
			{
				return new PersonResult(personInsertResult.ErrorMessage);
			}

			//Insert the email addresses. Using a for loop instead of a foreach because the order of the needs to be specified
			if (parameters.EmailAddresses != null)
			{
				Result emailResult;
				for (int i = 0; i < parameters.EmailAddresses.Count; i++)
				{
					emailResult = _personProvider.InsertEmailAddress(personInsertResult.ID, parameters.EmailAddresses[i], i);
				}
			}

			//Insert the phone numbers. Using a for loop instead of a foreach because the order of the needs to be specified
			if (parameters.PhoneNumbers != null)
			{
				Result phoneNumberResult;
				for (int i = 0; i < parameters.PhoneNumbers.Count; i++)
				{
					phoneNumberResult = _personProvider.InsertPhoneNumber(personInsertResult.ID, parameters.PhoneNumbers[i].Type, parameters.PhoneNumbers[i].Number, i);
				}
			}

			return new PersonResult(personInsertResult.ID);
		}

		public PersonResult Update(PersonUpdateParameters parameters)
		{
			//Update the person record if needed. Phone numbers go as a pair
			if (parameters.NameUpdated || parameters.OrganizationIDUpdated)
			{
				Result personUpdateResult = _personProvider.Update(parameters.ID, parameters.Name, parameters.OrganizationID, parameters.NameUpdated, parameters.OrganizationIDUpdated);

				if (!personUpdateResult.IsSuccess)
				{
					return new PersonResult(personUpdateResult.ErrorMessage);
				}
			}

			//Do the email addresses if needed. This is a full replacement
			if (parameters.EmailAddressesUpdated)
			{
				//Delete all the existing ones
				_personProvider.DeleteAllEmails(parameters.ID);

				//Add everything passed in
				Result emailResult;
				for (int i = 0; i < parameters.EmailAddresses.Count; i++)
				{
					emailResult = _personProvider.InsertEmailAddress(parameters.ID, parameters.EmailAddresses[i], i);
				}
			}

			//Do the phone numbers if needed. This is a full replacement
			if (parameters.PhoneNumbersUpdated)
			{
				//Delete all the existing ones
				_personProvider.DeleteAllPhoneNumbers(parameters.ID);

				//Add everything passed in
				Result phoneResult;
				for (int i = 0; i < parameters.PhoneNumbers.Count; i++)
				{
					phoneResult = _personProvider.InsertPhoneNumber(parameters.ID, parameters.PhoneNumbers[i].Type, parameters.PhoneNumbers[i].Number, i);
				}
			}

			//Even though it is kind of stupid, return a result object that includes the URL, as I think that's what is expected to go into the workflow item
			return new PersonResult(parameters.ID);
		}
		public bool PersonIsUsed(long personID)
		{
			return _personProvider.PersonIsUsed(personID);
		}

		public bool PersonExists(long personID)
		{
			return _personProvider.PersonExists(personID);
		}

		public PagedEnumerable<PersonLite> Get(PersonListParameters param)
		{
			return _personProvider.Get(param);
		}
	}
}

using System.Collections.Generic;
using Web.Public.Models;
using Web.Public.Providers;

namespace Web.Public.Services
{
	public class PersonService : IPersonService
	{
		private readonly IPersonProvider _personProvider;

		public PersonService(IPersonProvider personProvider)
		{
			_personProvider = personProvider;
		}

		public Person Get(long personID) => _personProvider.Get(personID);

		public (long TotalCount, List<Person> Persons) GetFilteredList(string filter, PagingOptions pagingOptions, string orDelimiter, string andDelimiter)
			=> _personProvider.GetFilteredList(filter, pagingOptions.Offset, pagingOptions.Limit, orDelimiter, andDelimiter);
	}
}
using System.Collections.Generic;
using Web.Public.Models;

namespace Web.Public.Providers
{
	public interface IOEProvider
	{
		OperatingEnvironmentWithDependencies Get(long id);
		bool Exists(long id);
		bool IsUsed(long id);
		(long TotalCount, List<OperatingEnvironment> OEs) GetFilteredList(List<OrClause> orClauses, long offset, long limit);
		List<long> GetForValidation(long validationID);
	}
}
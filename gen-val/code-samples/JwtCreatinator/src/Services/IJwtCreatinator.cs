using System.Collections.Generic;
using JwtCreatinator.Models;

namespace JwtCreatinator.Services
{
	public interface IJwtCreatinator
	{
		List<JwtRenewResponse> CreateJwts(List<JwtRenewRequest> requests);
	}
}
using System.Collections.Generic;
using System.Text.Json;

namespace Web.Public.ClaimsVerifiers
{
	public class TestSessionClaimsVerifier : ClaimsVerifierBase
	{
		private readonly long _tsId;

		public TestSessionClaimsVerifier(long tsId)
		{
			_tsId = tsId;
		}

		public override bool AreClaimsValid(IDictionary<string, string> claims)
		{
			try
			{
				var tsIdFromClaims = JsonSerializer.Deserialize<long>(claims["ts"]);
				
				return _tsId == tsIdFromClaims;
			}
			catch
			{
				return false;
			}
		}
	}
}
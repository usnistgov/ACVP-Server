using System.Collections.Generic;

namespace Web.Public.ClaimsVerifiers
{
    public abstract class ClaimsVerifierBase
    {
        public abstract bool AreClaimsValid(IDictionary<string, string> claims);
    }
}
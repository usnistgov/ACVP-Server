using System.Collections.Generic;

namespace Web.Public.ClaimsVerifiers
{
    public class TestSessionClaimsVerifier : ClaimsVerifierBase
    {
        private readonly long _tsID;

        public TestSessionClaimsVerifier(long tsID)
        {
            _tsID = tsID;
        }
        
        public override bool AreClaimsValid(IDictionary<string, string> claims)
        {
            try
            {
                var tsIDFromClaims = long.Parse(claims["ts"]);
                return _tsID == tsIDFromClaims;
            }
            catch
            {
                return false;
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Web.Public.ClaimsVerifiers
{
    public class VectorSetClaimsVerifier : ClaimsVerifierBase
    {
        private readonly long _tsID;
        private readonly long _vsID;
        
        public VectorSetClaimsVerifier(long tsID, long vsID)
        {
            _tsID = tsID;
            _vsID = vsID;
        }
        
        public override bool AreClaimsValid(IDictionary<string, string> claims)
        {
            try
            {
                var tsIDFromClaims = long.Parse(claims["tsId"]);
                var vsIDListFromClaims = claims["vsId"].Trim(new [] {'[', ']'}).Split(",").Select(long.Parse).ToList();

                return _tsID == tsIDFromClaims && vsIDListFromClaims.Contains(_vsID);
            }
            catch
            {
                return false;
            }
        }
    }
}
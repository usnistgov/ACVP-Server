using System.Collections.Generic;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF
{
    public class KdfMultiExpansionResult
    {
        public List<NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.KdfResult> Results { get; }

        public KdfMultiExpansionResult(List<NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.KdfResult> results)
        {
            Results = results;
        }
    }
}

using System;

namespace NIST.CVP.Crypto.Oracle.Exceptions
{
    public class OrleansGrainFailedAfterRetries : Exception
    {
        private const string ERROR_MESSAGE =
            "Orleans Grain did not fulfill expectations after retry attempts.";

        public OrleansGrainFailedAfterRetries()
            : base(ERROR_MESSAGE)
        {
            
        }
    }
}

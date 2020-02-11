using System;

namespace NIST.CVP.Orleans.Grains.Interfaces.Exceptions
{
    public class OriginalClusterNodeSuicideException : Exception
    {
        private const string ERROR_MESSAGE =
            "The original node the grain request began on killed itself. Reattempt the call from the beginning.";

        public OriginalClusterNodeSuicideException()
            : base(ERROR_MESSAGE) { }
    }
}
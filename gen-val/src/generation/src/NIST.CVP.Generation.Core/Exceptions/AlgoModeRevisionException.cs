using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.Core.Exceptions
{
    public class AlgoModeRevisionException : Exception
    {
        public AlgoModeRevisionException(string message)
        : base(message)
        {
            
        }
    }
}

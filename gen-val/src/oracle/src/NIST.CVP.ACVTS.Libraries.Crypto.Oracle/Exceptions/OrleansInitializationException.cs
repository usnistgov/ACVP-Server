using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Oracle.Exceptions
{
    public class OrleansInitializationException : Exception
    {
        private const string ERROR_MESSAGE =
            "Orleans Client failed to initialize after several attempts. Is the Orleans cluster running and accessible?";

        public OrleansInitializationException()
            : base(ERROR_MESSAGE)
        {

        }
    }
}

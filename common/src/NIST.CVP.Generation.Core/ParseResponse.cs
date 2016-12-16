using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public class ParseResponse<T> 
    {
        public T ParsedObject { get; private set; }
        public string ErrorMessage { get; private set; }

        public ParseResponse(T parsedObject)
        {
            ParsedObject = parsedObject;
        }
        public ParseResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
        public bool Success { get { return string.IsNullOrEmpty(ErrorMessage); } }

    }
}

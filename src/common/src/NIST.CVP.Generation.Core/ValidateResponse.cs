using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public class ValidateResponse
    {
        
        public string ErrorMessage { get; private set; }

        public ValidateResponse()
        {
            
        }
        public ValidateResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }
    }
}

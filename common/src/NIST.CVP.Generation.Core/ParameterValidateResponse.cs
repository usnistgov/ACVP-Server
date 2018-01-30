using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public class ParameterValidateResponse
    {
        
        public ParameterValidateResponse() { }

        public ParameterValidateResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}

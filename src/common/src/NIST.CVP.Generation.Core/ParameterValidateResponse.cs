using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.Core
{
    public class ParameterValidateResponse
    {
        public List<string> Errors { get; }

        public string ErrorMessage
        {
            get
            {
                if (Errors == null)
                {
                    return null;
                }

                if (!Errors.Any())
                {
                    return null;
                }

                return string.Join(";", Errors);
            }
        }

        public ParameterValidateResponse() { }

        public ParameterValidateResponse(List<string> errors)
        {
            Errors = errors;
        }

        public bool Success => !Errors.Any();
    }
}

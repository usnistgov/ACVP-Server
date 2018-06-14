using System;
using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;

namespace NIST.CVP.Generation.KMAC
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}

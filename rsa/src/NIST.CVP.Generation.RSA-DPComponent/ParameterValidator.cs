using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_DPComponent
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        public ParameterValidateResponse Validate(Parameters parameters)
        {
            return new ParameterValidateResponse();
        }
    }
}

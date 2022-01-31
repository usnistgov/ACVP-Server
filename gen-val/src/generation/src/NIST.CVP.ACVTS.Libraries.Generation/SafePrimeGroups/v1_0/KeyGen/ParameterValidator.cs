using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyGen
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        private static readonly List<SafePrime> ValidSafePrimeGroups = EnumHelpers.GetEnumsWithoutDefault<SafePrime>();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errorList = new List<string>();

            errorList.AddIfNotNullOrEmpty(ValidateArray(parameters.SafePrimeGroups, ValidSafePrimeGroups, "Safe Prime Groups"));

            return new ParameterValidateResponse();
        }
    }
}

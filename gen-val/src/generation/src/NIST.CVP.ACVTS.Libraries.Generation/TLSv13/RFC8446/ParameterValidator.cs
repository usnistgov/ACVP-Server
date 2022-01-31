using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.TLSv13.RFC8446
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        private static readonly AlgoMode ValidAlgoMode = AlgoMode.Tls_v1_3_v1_0;
        private static readonly List<HashFunctions> ValidHashFunctions = new List<HashFunctions>()
        {
            HashFunctions.Sha2_d256,
            HashFunctions.Sha2_d384
        };
        private static readonly List<TlsModes1_3> ValidTlsModes = EnumHelpers.GetEnumsWithoutDefault<TlsModes1_3>();

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            List<string> errors = new List<string>();

            ValidateAlgoModeRevision(parameters, errors);
            ValidateHashAlgs(parameters.HmacAlg, errors);
            ValidateRunningModes(parameters.RunningMode, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateAlgoModeRevision(Parameters parameters, List<string> errors)
        {
            ValidateAlgoMode(parameters, new[] { ValidAlgoMode }, errors);
        }

        private void ValidateHashAlgs(HashFunctions[] parametersHashAlg, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(
                ValidateArray(parametersHashAlg, ValidHashFunctions, "hashAlg"));
        }

        private void ValidateRunningModes(TlsModes1_3[] runningModes, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(
                ValidateArray(runningModes, ValidTlsModes, "runningModes"));
        }
    }
}

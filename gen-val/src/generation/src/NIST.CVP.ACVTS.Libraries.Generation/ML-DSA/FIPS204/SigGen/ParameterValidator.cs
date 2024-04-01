using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static DilithiumParameterSet[] VALID_PARAMETER_SETS = EnumHelpers.GetEnums<DilithiumParameterSet>().Except(new [] { DilithiumParameterSet.None }).ToArray();
    
    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.ML_DSA_SigGen_FIPS204 }, errors);

        if (!parameters.ParameterSets.Distinct().Any())
        {
            errors.Add("Expected at least one valid ML-DSA parameter set");
        }

        if (!parameters.Deterministic.Distinct().Any())
        {
            errors.Add("Expected at least one deterministic option");
        }

        if (errors.Any())
        {
            return new ParameterValidateResponse(errors);
        }

        return new ParameterValidateResponse();
    }
}

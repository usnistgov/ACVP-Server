using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.v1_0.KeyGen;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static DilithiumParameterSet[] VALID_PARAMETER_SETS = EnumHelpers.GetEnums<DilithiumParameterSet>().Except(new [] { DilithiumParameterSet.None }).ToArray();
    
    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.ML_DSA_KeyGen_FIPS204 }, errors);

        if (!parameters.ParameterSets.Distinct().Any() ||
            !parameters.ParameterSets.Distinct().All(parameterSet => VALID_PARAMETER_SETS.Contains(parameterSet)))
        {
            errors.Add("Expected at least one valid ML-DSA parameter set");
        }

        if (errors.Any())
        {
            return new ParameterValidateResponse(errors);
        }

        return new ParameterValidateResponse();
    }
}


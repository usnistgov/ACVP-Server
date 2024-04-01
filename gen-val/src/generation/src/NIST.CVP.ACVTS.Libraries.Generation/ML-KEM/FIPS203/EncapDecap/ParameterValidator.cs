using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Kyber;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ML_KEM.FIPS203.EncapDecap;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static KyberParameterSet[] VALID_PARAMETER_SETS = EnumHelpers.GetEnums<KyberParameterSet>().Except(new [] { KyberParameterSet.None }).ToArray();
    public static KyberFunction[] VALID_FUNCTIONS = EnumHelpers.GetEnums<KyberFunction>().Except(new[] { KyberFunction.None }).ToArray();
    
    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.ML_KEM_EncapDecap_FIPS203 }, errors);

        if (!parameters.ParameterSets.Distinct().Any())
        {
            errors.Add("Expected at least one valid ML-KEM parameter set");
        }

        if (!parameters.Functions.Distinct().Any())
        {
            errors.Add("Expected at least one valid ML-KEM function");
        }

        if (errors.Any())
        {
            return new ParameterValidateResponse(errors);
        }

        return new ParameterValidateResponse();
    }
}

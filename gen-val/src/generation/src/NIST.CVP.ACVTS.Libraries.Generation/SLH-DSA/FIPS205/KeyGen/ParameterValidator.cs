using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.KeyGen;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static SlhdsaParameterSet[] VALID_PARAMETER_SETS = EnumHelpers.GetEnums<SlhdsaParameterSet>().Except(new [] { SlhdsaParameterSet.None }).ToArray();
    
    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.SLH_DSA_KeyGen_FIPS205 }, errors);

        if (parameters.ParameterSets == null)
        {
            errors.Add("parameterSet was not provided.");
            return new ParameterValidateResponse(errors);
        }
        
        if (!parameters.ParameterSets.Distinct().Any())
            errors.Add("Expected at least one valid SLH-DSA parameter set");

        // ParameterSets shouldn't contain any repeats
        if (parameters.ParameterSets.Length != parameters.ParameterSets.Distinct().Count())
            errors.Add($"{nameof(parameters.ParameterSets)} must not contain the same SLH-DSA parameter set more than once");
        
        return errors.Any() ? new ParameterValidateResponse(errors) : new ParameterValidateResponse();  
    }
}

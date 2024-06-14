using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigVer;

public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
{
    public static readonly int MinMsgLen = 8;
    public static readonly int MaxMsgLen = 65536;
    
    public ParameterValidateResponse Validate(Parameters parameters)
    {
        var errors = new List<string>();

        ValidateAlgoMode(parameters, new[] { AlgoMode.SLH_DSA_SigVer_FIPS205 }, errors);
        ValidateCapabilities(parameters, errors);
        
        return errors.Any() ? new ParameterValidateResponse(errors) : new ParameterValidateResponse();  
    }
    
        private void ValidateCapabilities(Parameters parameters, List<string> errors)
    {
        // 1) was Capabilities included in the registration?
        if (parameters.Capabilities == null)
        {
            errors.Add($"{nameof(parameters.Capabilities)} was not provided.");
            return;
        }
        
        var capabilitiesElementType = parameters.Capabilities.GetType().GetElementType();
        var capabilitiesElementTypeString = capabilitiesElementType != null ? capabilitiesElementType.ToString() : "Capability"; 
        
        // 2) was Capabilities included, but empty?
        if (!parameters.Capabilities.Distinct().Any())
        {
            errors.Add($"Expected {nameof(parameters.Capabilities)} to contain at least one {capabilitiesElementTypeString}");
            return;
        }

        // 3) examine each Capability that was provided
        var shouldReturn = false;
        foreach (var capability in parameters.Capabilities)
        {
            // i) Were ParameterSets and MessageLength provided?
            if (capability.ParameterSets == null)
            {
                errors.Add($"{nameof(capability.ParameterSets)} was not provided");
                shouldReturn = true;
            }
            if (capability.MessageLength == null)
            {
                errors.Add($"{nameof(capability.MessageLength)} was not provided");
                shouldReturn = true;
            }

            if (shouldReturn)
                return;
            
            // ii) is ParameterSets non-empty?
            if (!capability.ParameterSets.Distinct().Any())
            {
                errors.Add($"Expected {nameof(capability.ParameterSets)} to contain at least one valid SLH-DSA parameter set");
                shouldReturn = true;
            }
            
            // iii) are the MessageLength values valid?
            ValidateDomain(capability.MessageLength, errors, "message length", MinMsgLen, MaxMsgLen);
            ValidateMultipleOf(capability.MessageLength, errors, 8, "complete byte messages");
            
            if (shouldReturn)
                return;
            
            //iv) ParameterSets shouldn't contain any repeats
            if (capability.ParameterSets.Length != capability.ParameterSets.Distinct().Count())
                errors.Add($"{nameof(capability.ParameterSets)} must not contain the same SLH-DSA parameter set more than once");
        }
    }
}

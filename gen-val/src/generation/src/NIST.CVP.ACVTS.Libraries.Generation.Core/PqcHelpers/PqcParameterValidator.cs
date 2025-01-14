using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.PqcHelpers;

public class PqcParameterValidator : ParameterValidatorBase
{
    public static readonly HashFunctions[] ValidHashAlgs = EnumHelpers.GetEnums<HashFunctions>().Except(new [] { HashFunctions.None, HashFunctions.Sha1 }).ToArray();

    public const int MinContextLen = 0;
    public const int MaxContextLen = 255 * 8;    // Max is 255 bytes, value can only be on a byte boundary
    public const int MinMsgLen = 8;
    public const int MaxMsgLen = 65536;
    
    protected void ValidateSignatureInterfacesAndPreHash(PqcParametersBase parameters, List<string> errors)
    { 
        // Ensure there is at least one value provided, and that "None" is not provided
        if (!parameters.SignatureInterfaces.Any() || parameters.SignatureInterfaces.Contains(SignatureInterface.None))
        {
            errors.Add("Expected at least one valid signature interface option");
        }
        
        if (parameters.SignatureInterfaces.Contains(SignatureInterface.External))
        {
            // Ensure there is at least one value provided, and that "None" is not provided
            if (!parameters.PreHash.Any() || parameters.PreHash.Contains(PreHash.None))
            {
                errors.Add("Expected at least one valid pre-hash option");
            }
        }
        
        // Internal + prehash is not a valid combination, so reject if only internal is included and prehash is selected or hash is provided or context is provided
        if (parameters.SignatureInterfaces.Contains(SignatureInterface.Internal) && parameters.SignatureInterfaces.Length == 1)
        {
            if (parameters.PreHash.Any())
            {
                errors.Add("Expected no pre-hash options with only internal interface");
            }
        }
    }

    protected void ValidateCapability(PqcCapabilityBase capability, PqcParametersBase parameters, List<string> errors)
    {
        // Check that if preHash is selected, the hashes provided are valid
        if (parameters.PreHash.Contains(PreHash.PreHash))
        {
            if (!capability.HashAlgs.Any())
            {
                errors.Add("No hash algs provided for pre-hash");
            }
            else
            {
                var result = ValidateArray(capability.HashAlgs, ValidHashAlgs, "valid hash algs");
                errors.AddIfNotNullOrEmpty(result);    
            }
        }
        
        // Require no hashAlgs property when preHash is not present
        if (!parameters.PreHash.Contains(PreHash.PreHash) && capability.HashAlgs.Any())
        {
            errors.Add("No hash algs expected when only pure pre-hash is selected");
        }

        if (parameters.SignatureInterfaces.Contains(SignatureInterface.External))
        {
            // ContextLengths is only needed on External signature interfaces
            if (capability.ContextLength == null)
            {
                errors.Add("context length expected");
            }
            else
            {
                ValidateDomain(capability.ContextLength, errors, "context length", MinContextLen, MaxContextLen);
                ValidateMultipleOf(capability.ContextLength, errors, 8, "complete byte contexts");    
            }
        }
        
        // Internal + prehash is not a valid combination, so reject if only internal is included and prehash is selected or hash is provided or context is provided
        if (parameters.SignatureInterfaces.Contains(SignatureInterface.Internal) && parameters.SignatureInterfaces.Length == 1)
        {
            if (capability.HashAlgs.Any())
            {
                errors.Add("no hash options expected with only internal interface");
            }

            if (capability.ContextLength != null)
            {
                errors.Add("no context lengths expected with only internal interface");
            }
        }
        
        if (capability.MessageLength == null)
        {
            errors.Add($"{nameof(capability.MessageLength)} was not provided");
            return;
        }
        
        ValidateDomain(capability.MessageLength, errors, "message length", MinMsgLen, MaxMsgLen);
        ValidateMultipleOf(capability.MessageLength, errors, 8, "complete byte messages");
    }
}

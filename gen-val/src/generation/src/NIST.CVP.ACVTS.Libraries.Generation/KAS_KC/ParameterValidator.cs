using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        private static readonly AlgoMode _validAlgoMode = AlgoMode.KAS_KC_Sp800_56;
        private static readonly int[] _allowedAesKeyLens = { 128, 192, 256 };
        private static readonly int _minKeyLen = 112;
        private static readonly int _maxKeyLen = 2048;
        private static readonly int _minMacLen = 64;
        private static readonly int _maxMacLen = 2048;

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!ValidateAlgoMode(parameters, new[] { _validAlgoMode }, errors))
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateKasRole(parameters.KasRole, errors);
            ValidateKeyConfirmationMethod(parameters.KeyConfirmationMethod, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateKasRole(KeyAgreementRole[] kasRole, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateArray(kasRole,
                EnumHelpers.GetEnumsWithoutDefault<KeyAgreementRole>(), nameof(kasRole)));
        }

        private void ValidateKeyConfirmationMethod(KeyConfirmationMethod keyConfirmationMethod, List<string> errors)
        {
            if (keyConfirmationMethod == null)
            {
                errors.Add($"{nameof(keyConfirmationMethod)} was null.");
                return;
            }

            ValidateKeyConfirmationDirections(keyConfirmationMethod.KeyConfirmationDirections, errors);
            ValidateKeyConfirmationRoles(keyConfirmationMethod.KeyConfirmationRoles, errors);
            ValidateMacMethods(keyConfirmationMethod.MacMethods, errors);
        }

        private void ValidateKeyConfirmationDirections(KeyConfirmationDirection[] keyConfirmationDirections, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateArray(keyConfirmationDirections,
                EnumHelpers.GetEnumsWithoutDefault<KeyConfirmationDirection>(), "keyConfirmationDirection"));
        }

        private void ValidateKeyConfirmationRoles(KeyConfirmationRole[] keyConfirmationRoles, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateArray(keyConfirmationRoles,
                EnumHelpers.GetEnumsWithoutDefault<KeyConfirmationRole>(), "keyConfirmationRoles"));
        }

        private void ValidateMacMethods(MacMethods macMethods, List<string> errors)
        {
            if (macMethods == null)
            {
                errors.Add($"{nameof(macMethods)} was null.");
                return;
            }

            var registeredMacMethods = macMethods.GetRegisteredMacMethods().ToList();

            if (!registeredMacMethods.Any())
            {
                errors.Add($"No {nameof(macMethods)} were registered.");
                return;
            }

            foreach (var registeredMacMethod in registeredMacMethods)
            {
                ValidateMacMethod(registeredMacMethod, errors);
            }
        }

        private void ValidateMacMethod(MacOptionsBase registeredMacMethod, List<string> errors)
        {
            if (registeredMacMethod.KeyLen % BitString.BITSINBYTE != 0)
            {
                errors.Add($"keyLen mod 8 for {registeredMacMethod.MacType}");
            }

            if (registeredMacMethod.MacLen % BitString.BITSINBYTE != 0)
            {
                errors.Add($"macLen mod 8 for {registeredMacMethod.MacType}");
            }

            if (registeredMacMethod.KeyLen < _minKeyLen)
            {
                errors.Add($"keyLen must be at least {_minKeyLen} bits for {registeredMacMethod.MacType}");
            }

            if (registeredMacMethod.MacLen < _minMacLen)
            {
                errors.Add($"macLen must be at least {_minMacLen} bits for {registeredMacMethod.MacType}");
            }

            switch (registeredMacMethod.MacType)
            {
                case KeyAgreementMacType.CmacAes:
                    ValidateMacMethodAes(registeredMacMethod, errors);
                    break;
                case KeyAgreementMacType.Kmac_128:
                case KeyAgreementMacType.Kmac_256:
                    ValidateMacMethodKmac(registeredMacMethod, errors);
                    break;
                default:
                    ValidateMacMethodHash(registeredMacMethod, errors);
                    break;
            }
        }

        private void ValidateMacMethodAes(MacOptionsBase registeredMacMethod, List<string> errors)
        {
            if (!_allowedAesKeyLens.Contains(registeredMacMethod.KeyLen))
            {
                errors.Add($"invalid keyLen of {registeredMacMethod.KeyLen} for {registeredMacMethod.MacType}");
            }

            if (registeredMacMethod.MacLen > 128)
            {
                errors.Add($"{registeredMacMethod.MacType} cannot produce a mac length of {registeredMacMethod.MacLen} bits.");
            }
        }


        private void ValidateMacMethodKmac(MacOptionsBase registeredMacMethod, List<string> errors)
        {
            if (registeredMacMethod.KeyLen > _maxKeyLen)
            {
                errors.Add($"keyLen of {registeredMacMethod.KeyLen} exceeds max testable keyLen of {_maxMacLen}");
            }

            if (registeredMacMethod.MacLen > _maxMacLen)
            {
                errors.Add($"macLen of {registeredMacMethod.MacLen} exceeds max testable macLen of {_maxMacLen}");
            }
        }

        private void ValidateMacMethodHash(MacOptionsBase registeredMacMethod, List<string> errors)
        {
            ModeValues mode = ModeValues.SHA1;
            DigestSizes digestSize = DigestSizes.NONE;

            EnumMapping.GetHashFunctionOptions(registeredMacMethod.MacType, ref mode, ref digestSize);
            var hashAttributes = ShaAttributes.GetShaAttributes(mode, digestSize);

            if (registeredMacMethod.MacLen > hashAttributes.outputLen)
            {
                errors.Add($"macLen of {registeredMacMethod.MacLen} exceeds output length of hash {registeredMacMethod.MacType}");
            }
        }
    }
}

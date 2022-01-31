using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Common;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_SSC.Sp800_56Br2.Ifc
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {
        private static readonly List<KeyAgreementRole> ValidKeyAgreementRoles =
            EnumHelpers.GetEnumsWithoutDefault<KeyAgreementRole>();

        private static readonly List<IfcKeyGenerationMethod> ValidKeyGenerationMethods =
            EnumHelpers.GetEnumsWithoutDefault<IfcKeyGenerationMethod>();

        private static readonly int[] ValidModulo = ParameterSetDetails.RsaModuloDetails.Keys.ToArray();

        private static readonly Dictionary<HashFunctions, int> HashFunctionEstimatedSecurityStrengths = new Dictionary<HashFunctions, int>()
        {
            { HashFunctions.Sha1, 80 },
            { HashFunctions.Sha2_d224, 112 },
            { HashFunctions.Sha2_d256, 128 },
            { HashFunctions.Sha2_d384, 192 },
            { HashFunctions.Sha2_d512, 256 },
            { HashFunctions.Sha2_d512t224, 112 },
            { HashFunctions.Sha2_d512t256, 128 },
            { HashFunctions.Sha3_d224, 112 },
            { HashFunctions.Sha3_d256, 128 },
            { HashFunctions.Sha3_d384, 192 },
            { HashFunctions.Sha3_d512, 256 },
        };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            var errors = new List<string>();

            if (!ValidateAlgoMode(parameters, new[] { AlgoMode.KAS_IFC_SSC_Sp800_56Br2 }, errors))
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateSchemes(parameters, errors);
            ValidateKeyGenerationMethod(parameters, errors);
            ValidateModulos(parameters.Modulo, errors);

            if (errors.Any())
            {
                return new ParameterValidateResponse(errors);
            }

            ValidateHashFunctionZ(parameters, errors);

            return new ParameterValidateResponse(errors);
        }

        private void ValidateSchemes(Parameters parameters, List<string> errors)
        {
            if (parameters.Scheme == null)
            {
                errors.Add("Scheme is required.");
                return;
            }

            var registeredSchemes = parameters.Scheme.GetRegisteredSchemes();

            if (!registeredSchemes.Any())
            {
                errors.Add("No Schemes were registered");
                return;
            }

            if (errors.Any())
            {
                return;
            }

            ValidateScheme(parameters.Scheme.Kas1, errors);
            ValidateScheme(parameters.Scheme.Kas2, errors);
        }

        private void ValidateScheme(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme == null)
                return;

            ValidateKeyAgreementRoles(scheme.KasRole, errorResults);
        }

        private void ValidateKeyAgreementRoles(KeyAgreementRole[] schemeRoles, List<string> error)
        {
            error.AddIfNotNullOrEmpty(ValidateArray(schemeRoles, ValidKeyAgreementRoles, "Key Agreement Roles"));
        }

        private void ValidateKeyGenerationMethod(Parameters parameters, List<string> errorResults)
        {
            if (errorResults.AddIfNotNullOrEmpty(
                ValidateArray(parameters.KeyGenerationMethods, ValidKeyGenerationMethods,
                    "keyGenerationMethods")))
            {
                return;
            }

            // Validate a fixed public exponent is provided for "1" key generation methods
            var methodsRequiringFixedPublicExponent = new[]
            {
                IfcKeyGenerationMethod.RsaKpg1_basic,
                IfcKeyGenerationMethod.RsaKpg1_crt,
                IfcKeyGenerationMethod.RsaKpg1_primeFactor
            };

            var requiresFixedPublicExponent = parameters.KeyGenerationMethods.Intersect(methodsRequiringFixedPublicExponent).Any();
            if (requiresFixedPublicExponent)
            {
                if (!RsaKeyHelper.IsValidExponent(parameters.FixedPublicExponent))
                {
                    errorResults.Add("Valid fixed public exponent required for the registered key generation modes.");
                }
            }

            // If only "2" key generation methods are supplied, ensure a public exponent is not supplied.
            var methodsNotRequiringFixedPublicExponent = new[]
            {
                IfcKeyGenerationMethod.RsaKpg2_basic,
                IfcKeyGenerationMethod.RsaKpg2_crt,
                IfcKeyGenerationMethod.RsaKpg2_primeFactor
            };

            if (!requiresFixedPublicExponent &&
                parameters.KeyGenerationMethods.Intersect(methodsNotRequiringFixedPublicExponent).Any() &&
                parameters.FixedPublicExponent != 0)
            {
                errorResults.Add("Unexpected fixed public exponent");
            }
        }

        private void ValidateModulos(int[] modulo, List<string> errors)
        {
            errors.AddIfNotNullOrEmpty(ValidateArray(modulo, ValidModulo, "Modulos"));
        }

        private void ValidateHashFunctionZ(Parameters parameters, List<string> errors)
        {
            // If we're not hashing Z, we can't compare security strengths of the domain parameter generation to the hash.
            if (parameters.HashFunctionZ == HashFunctions.None)
            {
                return;
            }

            // Need to ensure that the registered hash's security strength can be covered within the registered domain modulo security strengths.
            var registeredModuloJoinedWithSecurityStrengths =
                from registeredModulo
                    in parameters.Modulo.ToList()

                join validModulo in ParameterSetDetails.RsaModuloDetails on registeredModulo equals validModulo.Key
                select new
                {
                    Modulo = registeredModulo,
                    SecurityStrength = validModulo.Value
                };

            var hash = HashFunctionEstimatedSecurityStrengths.First(w => w.Key == parameters.HashFunctionZ);

            errors.AddRange(
                registeredModuloJoinedWithSecurityStrengths
                    .Where(x => x.SecurityStrength > hash.Value)
                    .Select(registeredModulo =>
                        $"{nameof(hash)} {hash.Key} is invalid as its security strength is too low to be used in conjunction with {registeredModulo.Modulo}"));
        }

    }
}

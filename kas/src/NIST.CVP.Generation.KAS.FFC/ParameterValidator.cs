using NIST.CVP.Crypto.KAS.Enums;
using System.Collections.Generic;
using NIST.CVP.Crypto.KAS.Helpers;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class ParameterValidator : ParameterValidatorBase
    {
        public override string Algorithm => "KAS-FFC";

        public override string[] ValidFunctions => new string[]
        {
            "dpGen",
            "dpVal",
            "keyPairGen",
            "fullVal",
            "keyRegen"
        };

        protected override void ValidateSchemes(Parameters parameters, List<string> errorResults)
        {
            if (parameters.Scheme == null)
            {
                errorResults.Add("Scheme is required.");
                return;
            }

            ValidateAtLeastOneSchemePresent(parameters.Scheme, errorResults);
            ValidateDhEphemScheme(parameters.Scheme.FfcDhEphem, errorResults);
            ValidateMqv1Scheme(parameters.Scheme.FfcMqv1, errorResults);
        }

        protected override void ValidateAtLeastOneParameterSetPresent(NoKdfNoKc kasMode, List<string> errorResults)
        {
            if (kasMode.ParameterSet == null)
            {
                errorResults.Add("ParameterSet must be provided.");
                return;
            }

            if (kasMode.ParameterSet.Fb == null && kasMode.ParameterSet.Fc == null)
            {
                errorResults.Add("At least one paramter set must be provided.");
            }
        }

        protected override void ValidateParameterSets(NoKdfNoKc kasMode, bool macRequired, List<string> errorResults)
        {
            ValidateParameterSetFfc(kasMode.ParameterSet.Fb, macRequired, FfcParameterSet.Fb, errorResults);
            ValidateParameterSetFfc(kasMode.ParameterSet.Fc, macRequired, FfcParameterSet.Fc, errorResults);
        }

        private void ValidateAtLeastOneSchemePresent(Schemes parametersScheme, List<string> errorResults)
        {
            if (parametersScheme.FfcDhEphem == null && 
                parametersScheme.FfcMqv1 == null &&
                parametersScheme.FfcDhHybrid1 == null &&
                parametersScheme.FfcDhHybridOneFlow == null &&
                parametersScheme.FfcDhOneFlow == null &&
                parametersScheme.FfcMqv2 == null &&
                parametersScheme.FfcDhStatic == null)
            {
                errorResults.Add("No schemes are present in the registration.");
            }
        }

        private void ValidateDhEphemScheme(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme == null)
            {
                return;
            }

            ValidateKeyAgreementRoles(scheme.Role, errorResults);

            ValidateAtLeastOneKasModePresent(scheme, errorResults);
            if (errorResults.Count > 0)
            {
                return;
            }

            // kdfKc is invalid for dhEphem
            if (scheme.KdfKc != null && scheme is FfcDhEphem)
            {
                errorResults.Add("Key Confirmation not possible with dhEphem.");
                return;
            }

            ValidateNoKdfNoKc(scheme.NoKdfNoKc, errorResults);
            ValidateKdfNoKc(scheme.KdfNoKc, errorResults);
            ValidateKdfKc(scheme.KdfKc, errorResults);
        }

        private void ValidateMqv1Scheme(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme == null)
            {
                return;
            }

            ValidateKeyAgreementRoles(scheme.Role, errorResults);

            ValidateAtLeastOneKasModePresent(scheme, errorResults);
            if (errorResults.Count > 0)
            {
                return;
            }

            ValidateNoKdfNoKc(scheme.NoKdfNoKc, errorResults);
            ValidateKdfNoKc(scheme.KdfNoKc, errorResults);
            ValidateKdfKc(scheme.KdfKc, errorResults);
        }

        private void ValidateParameterSetFfc(ParameterSetBase parameterSet, bool macRequired, FfcParameterSet parameterSetType, List<string> errorResults)
        {
            if (parameterSet == null)
            {
                return;
            }

            var parameterSetDetails = ParameterSetDetails.GetDetailsForFfcParameterSet(parameterSetType);

            ValidateHashFunctions(parameterSet.HashAlg, parameterSetType.ToString(), 
                parameterSetDetails.minHashLength, errorResults);
            ValidateMacOptions(parameterSet.MacOption, macRequired, parameterSetDetails.minMacLength,
                parameterSetDetails.minMacKeyLength, errorResults);
        }
    }
}
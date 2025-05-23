﻿using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.ECC
{
    public class ParameterValidator : ParameterValidatorBase
    {
        public override string Algorithm => "KAS-ECC";
        public override string Mode => null;

        public override string[] ValidFunctions => new string[]
        {
            "dpGen",
            "dpVal",
            "keyPairGen",
            "fullVal",
            "partialVal",
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

            ValidateScheme(parameters.Scheme.EccFullUnified, errorResults);
            ValidateScheme(parameters.Scheme.EccFullMqv, errorResults);
            ValidateScheme(parameters.Scheme.EccEphemeralUnified, errorResults);
            ValidateScheme(parameters.Scheme.EccOnePassUnified, errorResults);
            ValidateScheme(parameters.Scheme.EccOnePassMqv, errorResults);
            ValidateScheme(parameters.Scheme.EccOnePassDh, errorResults);
            ValidateScheme(parameters.Scheme.EccStaticUnified, errorResults);
        }

        protected override void ValidateAtLeastOneParameterSetPresent(NoKdfNoKc kasMode, List<string> errorResults)
        {
            if (kasMode.ParameterSet == null)
            {
                errorResults.Add("ParameterSet must be provided.");
                return;
            }

            if (kasMode.ParameterSet.Eb == null
                && kasMode.ParameterSet.Ec == null
                && kasMode.ParameterSet.Ed == null
                && kasMode.ParameterSet.Ee == null
            )
            {
                errorResults.Add("At least one parameter set must be provided.");
            }
        }

        protected override void ValidateParameterSets(NoKdfNoKc kasMode, bool macRequired, List<string> errorResults)
        {
            ValidateParameterSetEcc(kasMode.ParameterSet.Eb, macRequired, EccParameterSet.Eb, errorResults);
            ValidateParameterSetEcc(kasMode.ParameterSet.Ec, macRequired, EccParameterSet.Ec, errorResults);
            ValidateParameterSetEcc(kasMode.ParameterSet.Ed, macRequired, EccParameterSet.Ed, errorResults);
            ValidateParameterSetEcc(kasMode.ParameterSet.Ee, macRequired, EccParameterSet.Ee, errorResults);
        }

        private void ValidateAtLeastOneSchemePresent(Schemes parametersScheme, List<string> errorResults)
        {
            if (parametersScheme.EccEphemeralUnified == null &&
                parametersScheme.EccFullMqv == null &&
                parametersScheme.EccFullUnified == null &&
                parametersScheme.EccOnePassDh == null &&
                parametersScheme.EccOnePassUnified == null &&
                parametersScheme.EccOnePassMqv == null &&
                parametersScheme.EccStaticUnified == null)
            {
                errorResults.Add("No schemes are present in the registration.");
            }
        }

        private void ValidateScheme(SchemeBase scheme, List<string> errorResults)
        {
            if (scheme == null)
            {
                return;
            }

            ValidateKeyAgreementRoles(scheme.KasRole, errorResults);

            ValidateAtLeastOneKasModePresent(scheme, errorResults);
            if (errorResults.Count > 0)
            {
                return;
            }

            // kdfKc is invalid for EphemeralUnified
            if (scheme.KdfKc != null && scheme is EccEphemeralUnified)
            {
                errorResults.Add("Key Confirmation not possible with ephemeralUnified.");
                return;
            }

            ValidateDkmNonceTypeProvidedStaticScheme(scheme, errorResults);

            ValidateNoKdfNoKc(scheme.NoKdfNoKc, errorResults);
            ValidateKdfNoKc(scheme.KdfNoKc, errorResults);
            ValidateKdfKc(scheme.KdfKc, errorResults);
        }

        private void ValidateDkmNonceTypeProvidedStaticScheme(SchemeBase scheme, List<string> errorResults)
        {
            if (!(scheme is EccStaticUnified))
            {
                return;
            }

            ValidateDkmNonceTypeProvidedStaticScheme(scheme.KdfNoKc, errorResults);
            ValidateDkmNonceTypeProvidedStaticScheme(scheme.KdfKc, errorResults);
        }

        private void ValidateDkmNonceTypeProvidedStaticScheme(KdfNoKc kdfNoKc, List<string> errorResults)
        {
            if (kdfNoKc == null)
            {
                return;
            }

            errorResults.AddIfNotNullOrEmpty(ValidateArray(kdfNoKc.DkmNonceTypes, ValidNonceTypes, "Dkm Nonce Types"));
        }

        private void ValidateParameterSetEcc(ParameterSetBase parameterSet, bool macRequired, EccParameterSet parameterSetType, List<string> errorResults)
        {
            if (parameterSet == null)
            {
                return;
            }

            var parameterSetDetails = ParameterSetDetails.GetDetailsForEccParameterSet(parameterSetType);

            ValidateCurve(
                parameterSet.Curve, parameterSetDetails.minLengthN, parameterSetDetails.maxLengthN,
                errorResults);
            ValidateHashFunctions(parameterSet.HashAlg, parameterSetType.ToString(),
                parameterSetDetails.minHashLength, errorResults);
            ValidateMacOptions(parameterSet.MacOption, macRequired, parameterSetDetails.minMacLength,
                parameterSetDetails.minMacKeyLength, errorResults);
        }

        private void ValidateCurve(string parameterSetCurveName, int minLenN, int maxLenN, List<string> errorResults)
        {
            Curve curve;
            try
            {
                curve = EnumHelpers.GetEnumFromEnumDescription<Curve>(parameterSetCurveName);
            }
            catch (Exception)
            {
                errorResults.Add("Invalid Curve name");
                return;
            }

            var curveAttribute = CurveAttributesHelper.GetCurveAttribute(curve);
            if (curveAttribute.DegreeOfPolynomial < minLenN ||
                curveAttribute.DegreeOfPolynomial > maxLenN)
            {
                errorResults.Add("Curve not valid for parameterset");
            }
        }
    }
}

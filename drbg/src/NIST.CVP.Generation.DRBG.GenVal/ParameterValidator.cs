using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DRBG.Enums;

namespace NIST.CVP.Generation.DRBG.GenVal
{
    public class ParameterValidator : ParameterValidatorBase, IParameterValidator<Parameters>
    {

        private int _seedLength;

        private long _minimumEntropy;
        private long _maximumEntropy;

        private long _minimumNonce;
        private long _maximumNonce;

        private long _minimumPersonalizationString;
        private long _maximumPersonalizationString;

        private long _minimumAdditionalInput;
        private long _maximumAdditionalInput;



        /// <summary>
        /// algorithm, DrbgMechanism, mode, DrbgMode, maxSecurityStrength
        /// TODO Update this with strongly typed tuple implementation from C# 7
        /// </summary>
        public static readonly List<Tuple<string, DrbgMechanism, string, DrbgMode, int>> _securityStrengthsForModes = new List
            <Tuple<string, DrbgMechanism, string, DrbgMode, int>>()
            {
                new Tuple<string, DrbgMechanism, string, DrbgMode, int>("ctrDRBG", DrbgMechanism.Counter, "AES-128",
                    DrbgMode.AES128, 128),
                new Tuple<string, DrbgMechanism, string, DrbgMode, int>("ctrDRBG", DrbgMechanism.Counter, "AES-192",
                    DrbgMode.AES192, 192),
                new Tuple<string, DrbgMechanism, string, DrbgMode, int>("ctrDRBG", DrbgMechanism.Counter, "AES-256",
                    DrbgMode.AES256, 256)
            };

        public ParameterValidateResponse Validate(Parameters parameters)
        {
            DrbgMechanism drbgMechanism = 0;
            DrbgMode drbgMode = 0;
            int securityStrength = 0;

            List<string> errorResults = new List<string>();

            ValidateAndGetOptions(parameters, errorResults, ref drbgMechanism, ref drbgMode, ref securityStrength);

            // Cannot validate the rest of the parameters as they are dependant on the successful validation of the mechanism and mode.
            if (errorResults.Count > 0)
            {
                return new ParameterValidateResponse(string.Join(";", errorResults));
            }

            SetDrbgValidationAttributes(parameters, drbgMechanism, drbgMode, securityStrength);

            // TODO All values should be mod 8 to make validation easier?
            ValidateEntropy(parameters, errorResults);
            ValidateNonce(parameters, errorResults);
            ValidatePersonalizationString(parameters, errorResults);
            ValidateAdditionalInput(parameters, errorResults);
            ValidateSeedMaterial(parameters, errorResults);

            return new ParameterValidateResponse();
        }


        private void ValidateAndGetOptions(Parameters parameters, List<string> errorResults,
            ref DrbgMechanism drbgMechanism, ref DrbgMode drbgMode, ref int securityStrength)
        {
            var found = _securityStrengthsForModes
                .FirstOrDefault(w => w.Item1.Equals(parameters.Algorithm, StringComparison.OrdinalIgnoreCase) &&
                                     w.Item3.Equals(parameters.Mode, StringComparison.OrdinalIgnoreCase));

            if (found == null)
            {
                drbgMechanism = found.Item2;
                drbgMode = found.Item4;
                securityStrength = found.Item5;
            }
            else
            {
                errorResults.Add("Invalid Algorithm/Mode provided.");
            }
        }

        /// <summary>
        /// Set the min/max values that are to be validated against - based on Parameters
        /// </summary>
        private void SetDrbgValidationAttributes(Parameters parameters, DrbgMechanism drbgMechanism, DrbgMode drbgMode,
            int securityStrength)
        {
            int outLength = 128;
            int keyLength = 0;
            switch (drbgMode)
            {
                case DrbgMode.AES128:
                    keyLength = 128;
                    break;
                case DrbgMode.AES192:
                    keyLength = 192;
                    break;
                case DrbgMode.AES256:
                    keyLength = 256;
                    break;
            }

            _seedLength = outLength + keyLength;
            _minimumAdditionalInput = 0;
            _minimumPersonalizationString = 0;

            if (parameters.DerFuncEnabled)
            {
                _minimumEntropy = securityStrength;
                _maximumEntropy = (long) 1 << 35;
                _maximumPersonalizationString = (long) 1 << 35;
                _maximumAdditionalInput = (long) 1 << 35;
            }
            else
            {
                _minimumEntropy = _seedLength;
                _maximumEntropy = _seedLength;
                _maximumPersonalizationString = _seedLength;
                _maximumAdditionalInput = _seedLength;
            }
        }

        private void ValidateEntropy(Parameters parameters, List<string> errorResults)
        {
            var entropy = parameters.EntropyInputLen.GetDomainMinMax();

            var result = ValidateRange(new long[] {_minimumEntropy, _maximumEntropy}, entropy.Minimum, entropy.Maximum,
                "Entropy Range");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateNonce(Parameters parameters, List<string> errorResults)
        {
            var nonce = parameters.NonceLen.GetDomainMinMax();

            var result = ValidateRange(new long[] {_minimumNonce, _maximumNonce}, nonce.Minimum, nonce.Maximum,
                "Nonce Range");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidatePersonalizationString(Parameters parameters, List<string> errorResults)
        {
            var personalizationString = parameters.PersoStringLen.GetDomainMinMax();

            var result = ValidateRange(new long[] {_minimumPersonalizationString, _maximumPersonalizationString},
                personalizationString.Minimum, personalizationString.Maximum, "Personalization String Range");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }
        }

        private void ValidateAdditionalInput(Parameters parameters, List<string> errorResults)
        {
            var additionalInput = parameters.AdditionalInputLen.GetDomainMinMax();

            var result = ValidateRange(new long[] {_minimumAdditionalInput, _maximumAdditionalInput},
                additionalInput.Minimum, additionalInput.Maximum, "Additional Input Range");
            if (!string.IsNullOrEmpty(result))
            {
                errorResults.Add(result);
            }

            if (parameters.DerFuncEnabled)
            {
                result = ValidateMultipleOf(new long[] {additionalInput.Minimum, additionalInput.Maximum}, 8,
                    "Additional Input (mod)");
                if (!string.IsNullOrEmpty(result))
                {
                    errorResults.Add(result);
                }
            }
        }

        private void ValidateSeedMaterial(Parameters parameters, List<string> errorResults)
        {
            if (parameters.DerFuncEnabled)
            {
                var entropy = parameters.EntropyInputLen.GetValues(2).ToList();
                var nonce = parameters.NonceLen.GetValues(2).ToList();
                var personalizationString = parameters.PersoStringLen.GetValues(2).ToList();

                List<long> allSummed = new List<long>();

                // Add all combinations of the min and max together from entropy, nonce, and personalization
                foreach (var e in entropy.Where(w => w == entropy.Min() || w == entropy.Max()))
                {
                    foreach (var n in nonce.Where(w => w == nonce.Min() || w == nonce.Max()))
                    {
                        foreach (var p in personalizationString.Where(w => w == personalizationString.Min() || w == personalizationString.Max()))
                        {
                            allSummed.Add(e + n + p);
                        }
                    }
                }

                if (allSummed.Any(a => a % 8 != 0))
                {
                    errorResults.Add($"Invalid {nameof(entropy)} + {nameof(nonce)} + {nameof(personalizationString)} modulus.");
                }
            }
        }
    }
}

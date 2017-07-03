using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.DRBG;
using System;
using System.Linq;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.DRBG
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public const int _MAX_BIT_SIZE = 1024;

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = new List<ITestGroup>();

            CreateGroups(groups, parameters);

            return groups;
        }

        private void CreateGroups(List<ITestGroup> groups, Parameters parameters)
        {
            // We don't want to generate test groups that have 1 << 35 sized lengths (as that is the cap in some cases)
            // Set to a maximum value to generate.
            parameters.EntropyInputLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
            parameters.NonceLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
            parameters.PersoStringLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
            parameters.AdditionalInputLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);

            foreach (var entropyLen in parameters.EntropyInputLen.GetDomainMinMaxAsEnumerable())
            {
                foreach (var nonceLen in parameters.NonceLen.GetDomainMinMaxAsEnumerable())
                {
                    foreach (var persoStringLen in parameters.PersoStringLen.GetDomainMinMaxAsEnumerable())
                    {
                        foreach (var additionalInputLen in parameters.AdditionalInputLen.GetDomainMinMaxAsEnumerable())
                        {
                            if (DrbgSpecToDomainMapping.Map
                                .TryFirst(
                                    w => w.mechanism.Equals(parameters.Algorithm, StringComparison.OrdinalIgnoreCase) &&
                                         w.mode.Equals(parameters.Mode, StringComparison.OrdinalIgnoreCase),
                                    out var result))
                            {
                                DrbgParameters dp = new DrbgParameters()
                                {
                                    Mechanism = result.drbgMechanism,
                                    Mode = result.drbgMode,
                                    SecurityStrength = result.maxSecurityStrength,

                                    DerFuncEnabled = parameters.DerFuncEnabled,
                                    PredResistanceEnabled = parameters.PredResistanceEnabled,
                                    ReseedImplemented = parameters.ReseedImplemented,

                                    EntropyInputLen = entropyLen,
                                    NonceLen = nonceLen,
                                    PersoStringLen = persoStringLen,
                                    AdditionalInputLen = additionalInputLen,

                                    ReturnedBitsLen = parameters.ReturnedBitsLen
                                };

                                TestGroup tg = new TestGroup
                                {
                                    DrbgParameters = dp
                                };

                                groups.Add(tg);
                            }
                            else
                            {
                                throw new ArgumentException("Invalid Algorithm/Mode provided.");
                            }
                        }
                    }
                }
            }
        }
    }
}
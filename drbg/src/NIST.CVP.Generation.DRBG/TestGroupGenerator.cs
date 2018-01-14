using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Crypto.DRBG;
using System;
using System.Linq;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.DRBG.Helpers;
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
            foreach (var predResistance in parameters.PredResistanceEnabled)
            {
                foreach (var capability in parameters.Capabilities)
                {
                    var attributes = DrbgAttributesHelper.GetDrbgAttributes(parameters.Algorithm, capability.Mode, capability.DerFuncEnabled);

                    // We don't want to generate test groups that have 1 << 35 sized lengths (as that is the cap in some cases)
                    // Set to a maximum value to generate.
                    capability.EntropyInputLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
                    capability.NonceLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
                    capability.PersoStringLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);
                    capability.AdditionalInputLen.SetMaximumAllowedValue(_MAX_BIT_SIZE);

                    foreach (var entropyLen in capability.EntropyInputLen.GetDomainMinMaxAsEnumerable())
                    {
                        foreach (var nonceLen in capability.NonceLen.GetDomainMinMaxAsEnumerable())
                        {
                            foreach (var persoStringLen in capability.PersoStringLen.GetDomainMinMaxAsEnumerable())
                            {
                                foreach (var additionalInputLen in capability.AdditionalInputLen.GetDomainMinMaxAsEnumerable())
                                {
                                    var dp = new DrbgParameters
                                    {
                                        Mechanism = attributes.Mechanism,
                                        Mode = attributes.Mode,
                                        SecurityStrength = attributes.MaxSecurityStrength,

                                        DerFuncEnabled = capability.DerFuncEnabled,
                                        PredResistanceEnabled = predResistance,
                                        ReseedImplemented = parameters.ReseedImplemented,

                                        EntropyInputLen = entropyLen,
                                        NonceLen = nonceLen,
                                        PersoStringLen = persoStringLen,
                                        AdditionalInputLen = additionalInputLen,

                                        ReturnedBitsLen = capability.ReturnedBitsLen
                                    };

                                    var tg = new TestGroup
                                    {
                                        DrbgParameters = dp
                                    };

                                    groups.Add(tg);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public const int _MAX_BIT_SIZE = 1024;

        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = new List<ITestGroup>();

            CreateGroups(groups, parameters);

            var testVector = new TestVectorSet
            {
                TestGroups = groups,
                Algorithm = parameters.Algorithm,
                Mode = parameters.Mode,
                IsSample = parameters.IsSample
            };

            return testVector;
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
                            var mapped = DrbgSpecToDomainMapping.Map
                                .FirstOrDefault(
                                    w => w.Item1.Equals(parameters.Algorithm, StringComparison.OrdinalIgnoreCase) &&
                                         w.Item3.Equals(parameters.Mode, StringComparison.OrdinalIgnoreCase));

                            if (mapped == null)
                            {
                                throw new ArgumentException("Invalid Algorithm/Mode provided.");
                            }

                            DrbgParameters dp = new DrbgParameters()
                            {
                                Mechanism = mapped.Item2,
                                Mode = mapped.Item4,
                                SecurityStrength = mapped.Item5,

                                DerFuncEnabled = parameters.DerFuncEnabled,
                                // Prediction resistance test only when reseed implemented
                                PredResistanceEnabled = parameters.ReseedImplemented && parameters.PredResistanceEnabled,
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
                    }
                }
            }
        }
    }
}

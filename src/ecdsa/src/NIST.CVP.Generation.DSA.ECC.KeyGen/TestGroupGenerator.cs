using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private EccCurveFactory _curveFactory;

        public TestGroupGenerator()
        {
            _curveFactory = new EccCurveFactory();
        }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var curveParam in parameters.Curve)
            {
                foreach (var secretModeParam in parameters.SecretGenerationMode)
                {
                    var curveEnum = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveParam);
                    var curve = _curveFactory.GetCurve(curveEnum);

                    var secretModeEnum = EnumHelpers.GetEnumFromEnumDescription<SecretGenerationMode>(secretModeParam);
                    var domainParams = new EccDomainParameters(curve, secretModeEnum);

                    var testGroup = new TestGroup
                    {
                        DomainParameters = domainParams
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}

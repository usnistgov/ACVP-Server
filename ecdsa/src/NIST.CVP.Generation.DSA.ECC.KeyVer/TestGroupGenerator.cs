using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Helpers;
using NIST.CVP.Generation.DSA.ECC.KeyVer.TestCaseExpectations;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private EccCurveFactory _curveFactory = new EccCurveFactory();

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var curveName in parameters.Curve)
            {
                var curveEnum = EnumHelpers.GetEnumFromEnumDescription<Curve>(curveName);
                var curve = _curveFactory.GetCurve(curveEnum);

                var testGroup = new TestGroup
                {
                    DomainParameters = new EccDomainParameters(curve),
                    TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample)
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}

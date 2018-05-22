using NIST.CVP.Crypto.Common.KDF.Components.SNMP;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SNMP
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISnmpFactory _snmpFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISnmpFactory snmpFactory)
        {
            _random800_90 = random800_90;
            _snmpFactory = snmpFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var snmp = _snmpFactory.GetInstance();
            return new TestCaseGenerator(_random800_90, snmp);
        }
    }
}

using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CFB1;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class BitOrientedGenerator<TParameters, TTestVectorSet, TTestGroup, TTestCase> : Generator<TParameters, TTestVectorSet, TTestGroup, TTestCase>
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        public BitOrientedGenerator(
            ITestVectorFactory<TParameters> testVectorFactory, 
            IParameterParser<TParameters> parameterParser, 
            IParameterValidator<TParameters> parameterValidator, 
            ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> iTestCaseGeneratorFactoryFactory
        ) 
            : base(testVectorFactory, parameterParser, parameterValidator, iTestCaseGeneratorFactoryFactory)
        {
            _jsonConverters.Add(new BitOrientedBitStringConverter());
        }
    }
}

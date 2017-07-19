using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Crypto.AES_CFB1
{
    public class BitOrientedGenerator<TParameters, TTestVectorSet> : Generator<TParameters,TTestVectorSet>
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet
    {
        public BitOrientedGenerator(ITestVectorFactory<TParameters> testVectorFactory, IParameterParser<TParameters> parameterParser, IParameterValidator<TParameters> parameterValidator, ITestCaseGeneratorFactoryFactory<TTestVectorSet> iTestCaseGeneratorFactoryFactory) 
            : base(testVectorFactory, parameterParser, parameterValidator, iTestCaseGeneratorFactoryFactory)
        {
            _jsonConverters.Add(new BitOrientedBitStringConverter());
        }
    }
}

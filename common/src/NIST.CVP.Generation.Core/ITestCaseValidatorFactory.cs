using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseValidatorFactory<in TTestVectorSet, in TTestCase>
        where TTestVectorSet : ITestVectorSet
        where TTestCase : ITestCase
    {
        IEnumerable<ITestCaseValidator<TTestCase>> GetValidators(TTestVectorSet testVectorSet, IEnumerable<TTestCase> suppliedResults);
    }
}

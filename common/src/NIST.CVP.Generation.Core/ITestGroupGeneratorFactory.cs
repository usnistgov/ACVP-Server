using System.Collections;
using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public interface ITestGroupGeneratorFactory<in TParameters>
        where TParameters : IParameters
    {
        IEnumerable<ITestGroupGenerator<TParameters>> GetTestGroupGenerators();
    }
}
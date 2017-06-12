using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public interface IGeneratedDataTestGroupFactory<in TParameters, out TTestGroup>
        where TParameters : IParameters
        where TTestGroup : ITestGroup
    {
        IEnumerable<TTestGroup> BuildGDTTestGroups(TParameters parameters);
    }
}

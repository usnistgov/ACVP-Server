using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public interface ITestVectorFactory
    {
        ITestVectorSet BuildTestVectorSet(IParameters parameters);
    }
}

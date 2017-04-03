using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA3
{
    public interface ISHA3Factory
    {
        SHA3Wrapper GetSHA(HashFunction hashFunction);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public interface IPQGenerator
    {
        PQGenerateResult Generate(int L, int N, int seedLen);
    }
}

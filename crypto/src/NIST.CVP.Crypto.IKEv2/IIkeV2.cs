using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.IKEv2
{
    public interface IIkeV2
    {
        IkeResult GenerateIke(BitString ni, BitString nr, BitString gir, BitString girNew, BitString spii, BitString spir, int dkmLength);
    }
}

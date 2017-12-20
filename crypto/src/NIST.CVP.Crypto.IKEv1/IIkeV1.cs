using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.IKEv1
{
    public interface IIkeV1
    {
        IkeResult GenerateIke(BitString ni, BitString nr, BitString gxy, BitString cky_i, BitString cky_r, BitString presharedKey = null);
    }
}

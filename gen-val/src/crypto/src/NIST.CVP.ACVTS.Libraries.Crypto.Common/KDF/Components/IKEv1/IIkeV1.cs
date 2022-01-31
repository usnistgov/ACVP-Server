using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv1
{
    public interface IIkeV1
    {
        IkeResult GenerateIke(BitString ni, BitString nr, BitString gxy, BitString cky_i, BitString cky_r, BitString presharedKey = null);
    }
}

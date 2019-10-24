using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.RSA.Signatures
{
    public class MaskFactory : IMaskFactory
    {
        private readonly IShaFactory _shaFactory;

        public MaskFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }
        
        public IMaskFunction GetMaskInstance(PssMaskTypes maskType, HashFunction hashFunction = null)
        {
            switch (maskType)
            {
                case PssMaskTypes.MGF1:
                    var shaMgf = _shaFactory.GetShaInstance(hashFunction);
                    return new Mgf1Mask(shaMgf);
                
                case PssMaskTypes.SHAKE128:
                    var shake128 = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d128));
                    return new ShakeMask(shake128);
                
                case PssMaskTypes.SHAKE256:
                    var shake256 = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHAKE, DigestSizes.d256));
                    return new ShakeMask(shake256);
                
                default:
                    throw new ArgumentException($"Invalid {nameof(maskType)} provided");
            }
        }
    }
}
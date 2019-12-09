using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KDF.OneStep
{
    public class KdfKmac : KdfBase
    {
        private static readonly BitString Customization = new BitString(Encoding.ASCII.GetBytes("KDF"));
        private readonly IKmacFactory _kmacFactory;
        private readonly int _capacity;

        public KdfKmac(IKmacFactory kmacFactory, int capacity)
        {
            _kmacFactory = kmacFactory;
            _capacity = capacity;
        }

        protected override int OutputLength => KeyDataLength;
        protected override BigInteger MaxInputLength => -1;
        protected override BitString H(BitString message, BitString salt)
        {
            var kmac = _kmacFactory.GetKmacInstance(_capacity, false);
            return kmac.Generate(salt, message, Customization, KeyDataLength).Mac;
        }
    }
}
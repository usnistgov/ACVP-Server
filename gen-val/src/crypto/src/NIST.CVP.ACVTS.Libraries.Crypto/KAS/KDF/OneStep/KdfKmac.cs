using System.Numerics;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep
{
    public class KdfKmac : KdfBase
    {
        private static readonly BitString Customization = new BitString(Encoding.ASCII.GetBytes("KDF"));
        private readonly IKmacFactory _kmacFactory;
        private readonly int _capacity;

        public KdfKmac(IKmacFactory kmacFactory, int capacity, bool useCounter)
        {
            _kmacFactory = kmacFactory;
            _capacity = capacity;
            UseCounter = useCounter;
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

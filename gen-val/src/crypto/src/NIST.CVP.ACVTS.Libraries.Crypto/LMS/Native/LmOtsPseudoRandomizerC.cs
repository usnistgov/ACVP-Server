using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native
{
    public class LmOtsPseudoRandomizerC : ILmOtsRandomizerC
    {
        private readonly IShaFactory _shaFactory;

        private static readonly byte[] RandomizerConst = { 0xFF, 0xFD, 0xFF };

        public LmOtsPseudoRandomizerC(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public byte[] GetRandomizerValueC(ILmOtsPrivateKey privateKey)
        {
            var sha = LmsHelpers.GetSha(_shaFactory, privateKey.LmOtsAttribute.Mode);
            var bufferLength = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(privateKey.LmOtsAttribute.Mode);
            var buffer = new byte[bufferLength];

            var c = new byte[privateKey.LmOtsAttribute.N];

            // This is the same "pieces" that go into the construction of the key, just in a different order and
            // with a different constant.  Should still be unique to the key.
            // C = H(I || Q || 0xFFFDFF || SEED)
            sha.Init();
            sha.Update(privateKey.I, privateKey.I.BitLength());
            sha.Update(privateKey.Q, privateKey.Q.BitLength());
            sha.Update(RandomizerConst, RandomizerConst.BitLength());
            sha.Update(privateKey.Seed, privateKey.Seed.BitLength());
            sha.Final(buffer, buffer.BitLength());

            Array.Copy(buffer, c, c.Length);

            return c;
        }
    }
}

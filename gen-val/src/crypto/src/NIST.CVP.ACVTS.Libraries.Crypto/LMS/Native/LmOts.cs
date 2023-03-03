using System;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native
{
    public class LmOts : ILmOtsSigner, ILmOtsVerifier
    {
        private readonly IShaFactory _shaFactory;

        public LmOts(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public byte[] Sign(ILmOtsPrivateKey privateKey, ILmOtsRandomizerC randomizerC, byte[] message)
        {
            var sha = LmsHelpers.GetSha(_shaFactory, privateKey.LmOtsAttribute.Mode);
            var bufferSizeBytes = AttributesHelper.GetBufferByteLengthBasedOnOneWayFunction(privateKey.LmOtsAttribute.Mode);
            var buffer = new byte[bufferSizeBytes];

            var c = randomizerC.GetRandomizerValueC(privateKey);

            var qChecksumConcatenation = LmsHelpers.QChecksumConcatenation(privateKey.I, privateKey.Q, c, message, privateKey.LmOtsAttribute, sha, buffer);

            var y = new byte[privateKey.LmOtsAttribute.P][];
            var x = privateKey.X;
            for (var i = 0; i < privateKey.LmOtsAttribute.P; i++)
            {
                var tmp = new byte[privateKey.LmOtsAttribute.N];
                
                // a is guaranteed to be 1 byte in length
                var a = LmsHelpers.Coef(qChecksumConcatenation, i, privateKey.LmOtsAttribute.W);
                Array.Copy(x[i], tmp, tmp.Length);
                
                for (byte[] j = {0}; j[0] < a; j[0]++)
                {
                    // tmp = H(I || u32str(q) || u16str(i) || u8str(j) || tmp)
                    sha.Init();
                    sha.Update(privateKey.I, privateKey.I.BitLength());
                    sha.Update(privateKey.Q, privateKey.Q.BitLength());
                    sha.Update(i, 16);
                    sha.Update(j, 8);
                    sha.Update(tmp, tmp.BitLength());
                    sha.Final(buffer, buffer.BitLength());

                    Array.Copy(buffer, tmp, tmp.Length);
                }

                y[i] = tmp;
            }

            // Return u32str(type) || C || y[0] || ... || y[p-1]
            var signature = new byte[privateKey.LmOtsAttribute.NumericIdentifier.Length + c.Length + y.Select(s => s.Length).Sum()];
            Array.Copy(privateKey.LmOtsAttribute.NumericIdentifier, 0, signature, 0, 4);
            Array.Copy(c, 0, signature, 4, c.Length);
            var signatureStartIndexOfY = 4 + c.Length;
            for (var i = 0; i < y.Length; i++)
            {
                Array.Copy(
                    y[i], 0,
                    signature, (signatureStartIndexOfY) + (privateKey.LmOtsAttribute.N * i),
                    privateKey.LmOtsAttribute.N);
            }

            return signature;
        }

        public bool Verify(ILmOtsPublicKey publicKey, byte[] signature, byte[] message)
        {
            var fullKey = publicKey.Key;
            if (fullKey.Length < 4)
            {
                throw new ArgumentException($"{nameof(publicKey)} must be at least 4 bytes.");
            }

            var pubType = fullKey.Take(4).ToArray();
            
            var mode = AttributesHelper.GetLmOtsModeFromTypeCode(pubType);
            var attribute = AttributesHelper.GetLmOtsAttribute(mode);

            var publicKeyExpectedBytes = attribute.N + 24;
            if (fullKey.Length != publicKeyExpectedBytes)
            {
                throw new ArgumentException($"{nameof(publicKey)} was expected to be {publicKeyExpectedBytes} bytes, was {fullKey.Length}.");
            }

            var i = fullKey.Skip(4).Take(16).ToArray();
            var q = fullKey.Skip(4 + 16).Take(4).ToArray();
            var k = fullKey.Skip(4 + 16 + 4).ToArray();

            var sha = LmsHelpers.GetSha(_shaFactory, attribute.Mode);
            var keyCandidate = LmsHelpers.GetLmOtsPublicKeyCandidate(sha, attribute, signature, message, i, q);

            return k.SequenceEqual(keyCandidate);
        }
    }
}

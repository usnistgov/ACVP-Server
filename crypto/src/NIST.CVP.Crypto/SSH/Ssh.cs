using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.SSH
{
    public class Ssh : ISsh
    {
        private readonly int _ivLength;
        private readonly int _keyLength;
        private readonly ISha _hash;

        public Ssh(ISha hash, int ivLength, int keyLength)
        {
            _hash = hash;
            _ivLength = ivLength;
            _keyLength = keyLength;
        }

        public KdfResult DeriveKey(BitString k, BitString h, BitString sessionId)
        {
            var iv1 = PseudoRandomFunction(k, h, 'A', sessionId, _ivLength);
            var iv2 = PseudoRandomFunction(k, h, 'B', sessionId, _ivLength);

            var key1 = PseudoRandomFunction(k, h, 'C', sessionId, _keyLength);
            var key2 = PseudoRandomFunction(k, h, 'D', sessionId, _keyLength);

            var iKey1 = PseudoRandomFunction(k, h, 'E', sessionId, _hash.HashFunction.OutputLen);
            var iKey2 = PseudoRandomFunction(k, h, 'F', sessionId, _hash.HashFunction.OutputLen);

            var clientToServer = new OneWayResult
            {
                InitialIv = iv1,
                EncryptionKey = key1,
                IntegrityKey = iKey1
            };

            var serverToClient = new OneWayResult
            {
                InitialIv = iv2,
                EncryptionKey = key2,
                IntegrityKey = iKey2
            };

            return new KdfResult(serverToClient, clientToServer);
        }

        private BitString PseudoRandomFunction(BitString k, BitString h, char letter, BitString sessionId, int L)
        {
            var N = L.CeilingDivide(_hash.HashFunction.OutputLen);
            var charArray = new[] {letter};
            
            var message = k.ConcatenateBits(h).ConcatenateBits(new BitString(Encoding.ASCII.GetBytes(charArray))).ConcatenateBits(sessionId);
            var kOut = _hash.HashMessage(message).Digest;

            for (var i = 0; i < N; i++)
            {
                var hashIn = k.ConcatenateBits(h).ConcatenateBits(kOut);
                var kI = _hash.HashMessage(hashIn).Digest;
                kOut = kOut.ConcatenateBits(kI);
            }

            return kOut.GetMostSignificantBits(L);
        }
    }
}

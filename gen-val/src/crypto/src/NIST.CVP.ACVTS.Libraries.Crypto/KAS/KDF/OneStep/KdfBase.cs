using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep
{
    public abstract class KdfBase : IKdfOneStep
    {
        protected abstract int OutputLength { get; }
        protected abstract BigInteger MaxInputLength { get; }
        protected abstract BitString H(BitString message, BitString salt);

        protected int KeyDataLength;
        protected bool UseCounter;

        public virtual KdfResult DeriveKey(BitString z, int keyDataLength, BitString fixedInfo, BitString salt)
        {
            KeyDataLength = keyDataLength;

            // 1. reps =  keydatalen / hashlen.
            var reps = KeyDataLength.CeilingDivide(OutputLength);

            // 2. If reps > (23^2 −1), then return an error indicator without performing the remaining actions.
            if (reps > ((BigInteger)1 << 32) - 1)
            {
                throw new ArgumentException($"{nameof(reps)} exceeds (2^32)-1");
            }

            if (!UseCounter & reps != 1)
            {
                throw new ArgumentException("When utilizing NoCounter, reps should always be 1.");
            }

            // 3. Initialize a 32-bit, big-endian bit string counter as 1 (i.e. 0x00000001).
            var counter = new BitString(32).BitStringAddition(BitString.One());
            if (!UseCounter)
            {
                counter = new BitString(0);
            }

            var messageToH = counter.ConcatenateBits(z).ConcatenateBits(fixedInfo);

            // 4. If counter || Z || FixedInfo is more than max_H_inputlen bits long,
            // then return an error indicator without performing the remaining actions
            if (MaxInputLength != -1 && messageToH.BitLength > MaxInputLength)
            {
                throw new ArgumentException($"{nameof(messageToH)} exceeds length of {MaxInputLength}");
            }

            BitString k = new BitString(0);

            // 5. For i = 1 to reps by 1, do the following:
            for (int i = 0; i < reps - 1; i++)
            {
                // 5.1 Compute K(i) = H(counter || Z || OtherInfo).
                k = k.ConcatenateBits(H(messageToH, salt));

                // 5.2 Increment counter(modulo 23^2), treating it as an unsigned 32 - bit integer.
                counter = counter.BitStringAddition(BitString.One());

                // Update the message to H with the current counter
                messageToH = counter.ConcatenateBits(z).ConcatenateBits(fixedInfo);
            }

            // 6. Let K_Last be set to K(reps) if (keydatalen / hashlen) is an integer; otherwise, let K_Last
            // be set to the(keydatalen mod hashlen) leftmost bits of K(reps)
            if (KeyDataLength % OutputLength == 0)
            {
                k = k.ConcatenateBits(H(messageToH, salt));
            }
            else
            {
                k = k.ConcatenateBits(
                    H(messageToH, salt).GetMostSignificantBits(KeyDataLength % OutputLength)
                );
            }

            // 7. Set DerivedKeyingMaterial = K(1) || K(2) || … || K(reps-1) || K_Last.
            return new KdfResult(k.GetMostSignificantBits(keyDataLength));
        }
    }
}

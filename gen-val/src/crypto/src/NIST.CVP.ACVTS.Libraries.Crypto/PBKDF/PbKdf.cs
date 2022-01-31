using System;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.PBKDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.PBKDF
{
    public class PbKdf : IPbKdf
    {
        private readonly IHmac _hmac;

        public PbKdf(IHmac hmac)
        {
            _hmac = hmac;
        }

        public KdfResult DeriveKey(BitString salt, string password, int c, int keyLen)
        {
            // Could check keyLen, but the allowed max is larger than an int allows, so we just force positive...
            if (keyLen <= 0)
            {
                return new KdfResult("KeyLen must be greater than 0");
            }

            var passwordBytes = Encoding.ASCII.GetBytes(password);
            var iterationsNeeded = keyLen.CeilingDivide(_hmac.OutputLength);
            var saltBytes = salt.GetPaddedBytes();

            var t = new byte[GetNextMultiple(keyLen, _hmac.OutputLength) / 8];
            var u = new byte[_hmac.OutputLength / 8];
            var t_i = new byte[_hmac.OutputLength / 8];

            _hmac.Init(passwordBytes);

            for (var i = 1; i <= iterationsNeeded; i++)
            {
                var iterationCounter = new byte[4];

                iterationCounter[0] = (byte)(i >> 24);
                iterationCounter[1] = (byte)(i >> 16);
                iterationCounter[2] = (byte)(i >> 8);
                iterationCounter[3] = (byte)i;

                //Array.Copy(saltBytes, u, saltBytes.Length);
                //Array.Copy(iterationCounter, 0, u, saltBytes.Length, 4);

                Array.Clear(t_i, 0, t_i.Length);

                for (var j = 1; j <= c; j++)
                {
                    _hmac.FastInit();

                    if (j == 1)
                    {
                        _hmac.Update(saltBytes, salt.BitLength);
                        _hmac.Update(iterationCounter, iterationCounter.Length * 8);
                    }
                    else
                    {
                        _hmac.Update(u, u.Length * 8);
                    }

                    _hmac.Final(ref u);

                    for (var k = 0; k < u.Length; k++)
                    {
                        t_i[k] ^= u[k];
                    }
                }

                Array.Copy(t_i, 0, t, (i - 1) * t_i.Length, t_i.Length);
            }

            return new KdfResult(new BitString(t).GetMostSignificantBits(keyLen));
        }

        private int GetNextMultiple(int start, int multiple)
        {
            if (start % multiple == 0)
            {
                return start;
            }

            return start + (multiple - start % multiple);
        }
    }
}

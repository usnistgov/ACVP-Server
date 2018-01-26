using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.Crypto.Common.KDF.Components.SSH.Enums;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.SSH
{
    public class SshFactory : ISshFactory
    {
        public ISsh GetSshInstance(HashFunction hash, Cipher cipher)
        {
            var sha = new ShaFactory().GetShaInstance(hash);
            var lengthTuple = GetIvKeyLengthFromCipher(cipher);

            return new Ssh(sha, lengthTuple.ivLength, lengthTuple.keyLength);
        }

        private (int ivLength, int keyLength) GetIvKeyLengthFromCipher(Cipher cipher)
        {
            switch (cipher)
            {
                case Cipher.AES128:
                    return (128, 128);

                case Cipher.AES192:
                    return (128, 192);

                case Cipher.AES256:
                    return (128, 256);

                case Cipher.TDES:
                    return (64, 192);

                default:
                    throw new ArgumentException("Invalid cipher");
            }
        }
    }
}

using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.SSH
{
    public class SshFactory : ISshFactory
    {
        private readonly IShaFactory _shaFactory;

        public SshFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public ISsh GetSshInstance(HashFunction hash, Cipher cipher)
        {
            var sha = _shaFactory.GetShaInstance(hash);
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

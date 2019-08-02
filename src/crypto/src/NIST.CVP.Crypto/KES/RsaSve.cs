using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Math.Helpers;

namespace NIST.CVP.Crypto.KES
{
    public class RsaSve : IRsaSve
    {
        private readonly IRsa _rsa;
        private readonly IEntropyProvider _entropyProvider;

        public RsaSve(IRsa rsa, IEntropyProviderFactory entropyFactory)
        {
            _rsa = rsa;
            _entropyProvider = entropyFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }
        
        public SharedSecretWithEncryptedValueResponse Generate(PublicKey rsaPublicKey)
        {
            //1. Compute the value of nLen = len(n)/8 , the byte length of the modulus n
            var nLen = rsaPublicKey.N.ExactBitLength();

            BigInteger z;
            BitString Z;
            
            // 2. Generation:
            while (true)
            {
                // a. Using the RBG (see Section 5.3), generate Z, a byte string of nLen bytes.
                Z = _entropyProvider.GetEntropy(nLen);

                // b. Convert Z to an integer z (See Appendix B.2):
                z = Z.ToPositiveBigInteger();

                // c. If z does not satisfy 1 < z < (n – 1), then go to step 2a.
                if (z > 1 && z < rsaPublicKey.N - 1)
                {
                    break;
                }
            } 
            
            // 3. RSA encryption:
            // a. Apply the RSAEP encryption primitive (see Section 7.1.1) to the integer z using the
            //     public key (n, e) to produce an integer ciphertext c:
            var c = _rsa.Encrypt(z, rsaPublicKey);
            
            // b. Convert the ciphertext c to a ciphertext byte string C of nLen bytes (see Appendix B.1):
            var C = new BitString(c.CipherText).PadToModulusMsb(nLen);
            
            //  4. Output the string Z as the secret value, and the ciphertext C
            return new SharedSecretWithEncryptedValueResponse(Z, C);
        }

        public SharedSecretResponse Recover(KeyPair rsaKeyPair, BitString ciphertext)
        {
            // 1. nLen = len(n)/8, the byte length of n.
            var nLen = rsaKeyPair.PubKey.N.ExactBitString().BitLength;
            
            // 2. Length checking:
            // If the length of the ciphertext C is not nLen bytes in length, output an indication of a
            // decryption error, and exit without further processing.
            if (ciphertext.BitLength != nLen)
            {
                throw new ArgumentException($"{nameof(nLen)} should match {nameof(ciphertext)} length.");
            }
            
            // 3. RSA decryption:
            // a. Convert the ciphertext C to an integer ciphertext c (see Appendix B.2):
            var c = ciphertext.ToPositiveBigInteger();
            
            // b. Apply the RSADP decryption primitive (see Section 7.1.2) to the ciphertext c using
            // the private key (n, d) to produce an integer z:
            var z = _rsa.Decrypt(c, rsaKeyPair.PrivKey, rsaKeyPair.PubKey).PlainText;
            
            // c. If RSADP indicates that the ciphertext is out of range, output an indication of a
            // decryption error, and exit without further processing.
                // handled within the decrypt operation itself
                
            // d. Convert the integer z to a byte string Z of nLen bytes (see Appendix B.1)
            var Z = new BitString(z).PadToModulusMsb(nLen);
            
            // 4. Output the string Z as the secret value (i.e., the shared secret), or an error indicator.
            return new SharedSecretResponse(Z);
        }
    }
}
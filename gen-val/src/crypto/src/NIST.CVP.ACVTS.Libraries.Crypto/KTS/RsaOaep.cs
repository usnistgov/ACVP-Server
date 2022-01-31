using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KTS;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KTS
{
    public class RsaOaep : IRsaOaep
    {
        private readonly ISha _sha;
        private IMgf _mgf;
        private readonly IRsa _rsa;
        private readonly IEntropyProvider _entropyProvider;

        public RsaOaep(ISha sha, IMgf mgf, IRsa rsa, IEntropyProviderFactory entropyFactory)
        {
            _sha = sha;
            _mgf = mgf;
            _rsa = rsa;
            _entropyProvider = entropyFactory.GetEntropyProvider(EntropyProviderTypes.Random);
        }

        public SharedSecretResponse Encrypt(PublicKey rsaPublicKey, BitString keyingMaterial, BitString additionalInput)
        {
            if (additionalInput == null)
            {
                additionalInput = new BitString(0);
            }

            // 1. nLen = len(n)/8, the byte length of n
            var nLenBits = rsaPublicKey.N.ExactBitLength().ValueToMod(BitString.BITSINBYTE);
            var nLen = nLenBits.CeilingDivide(BitString.BITSINBYTE);

            // 2. Length checking:
            // a. KLen = len(K)/8, the byte length of K.
            var KLen = keyingMaterial.BitLength.CeilingDivide(BitString.BITSINBYTE);

            // b. If KLen > nLen – 2HLen – 2, then output an indication that the keying material is
            // too long, and exit without further processing.
            var HLen = _sha.HashFunction.OutputLen.CeilingDivide(BitString.BITSINBYTE);
            if (KLen > nLen - 2 * HLen - 2)
            {
                throw new ArgumentException($"{nameof(KLen)} length too large.");
            }

            // 3. OAEP encoding:
            // a. Apply the selected hash function to compute: HA = H(A).
            var HA = _sha.HashMessage(additionalInput).Digest;

            // b. Construct a byte string PS consisting of nLen – KLen – 2HLen – 2 zero bytes. The
            // length of PS may be zero.
            var PS = new BitString(new byte[nLen - KLen - 2 * HLen - 2]);

            // c. Concatenate HA, PS, a single byte with a hexadecimal value of 01, and the keying
            // material K to form data DB of nLen – HLen – 1 bytes as follows:
            // DB = HA || PS || 00000001 || K
            var DB = HA.ConcatenateBits(PS).ConcatenateBits(BitString.To8BitString(0x01)).ConcatenateBits(keyingMaterial);

            // d. Using the RBG (see Section 5.3), generate a random byte string mgfSeed of HLen bytes.
            var mgfSeed = _entropyProvider.GetEntropy(_sha.HashFunction.OutputLen);

            // e. Apply the mask-generation function in Section 7.2.2.2 to compute:
            // dbMask = MGF(mgfSeed, nLen – HLen – 1).
            var dbMask = _mgf.Generate(mgfSeed, (nLen - HLen - 1) * BitString.BITSINBYTE);

            // f. Let maskedDB = DB ⊕ dbMask.
            var maskedDB = DB.XOR(dbMask);

            // g. Apply the mask-generation function in Section 7.2.2.2 to compute:
            // mgfSeedMask = MGF(maskedDB, HLen).
            var mgfSeedMask = _mgf.Generate(maskedDB, _sha.HashFunction.OutputLen);

            // h. Let maskedMGFSeed = mgfSeed ⊕ mgfSeedMask.
            var maskedMGFSeed = mgfSeed.XOR(mgfSeedMask);

            // i. Concatenate a single byte with hexadecimal value 00, maskedMGFSeed, and
            // maskedDB to form an encoded message EM of nLen bytes as follows:
            // EM = 00000000 || maskedMGFSeed || maskedDB,
            var EM = BitString.To8BitString(0x00).ConcatenateBits(maskedMGFSeed).ConcatenateBits(maskedDB);

            // 4. RSA encryption:
            // a. Convert the encoded message EM to an integer em (see Appendix B.2):
            // em = BS2I(EM).
            var em = EM.ToPositiveBigInteger();

            // b. Apply RSAEP (see Section 7.1.1) to the integer em using the public key (n, e) to
            // produce a ciphertext integer c:
            // c = RSAEP((n, e), em).
            var c = _rsa.Encrypt(em, rsaPublicKey).CipherText;

            // c. Convert the ciphertext integer c to a ciphertext byte string C of nLen bytes (see
            // Appendix B.1):
            // C = I2BS(c, nLen).
            var C = new BitString(c).PadToModulusMsb(rsaPublicKey.N.ExactBitLength().ValueToMod(BitString.BITSINBYTE));

            return new SharedSecretResponse(C);
        }

        public SharedSecretResponse Decrypt(KeyPair rsaKeyPair, BitString ciphertext, BitString additionalInput)
        {
            if (additionalInput == null)
            {
                additionalInput = new BitString(0);
            }

            // 1. Initializations:
            // a. nLen = the byte length of n. For this Recommendation, nLen ≥ 256.
            var nLenBits = rsaKeyPair.PubKey.N.ExactBitLength().ValueToMod(BitString.BITSINBYTE);
            var nLen = nLenBits.CeilingDivide(BitString.BITSINBYTE);

            var hLenBits = _sha.HashFunction.OutputLen;
            var HLen = hLenBits.CeilingDivide(BitString.BITSINBYTE);

            // b. DecryptErrorFlag = False.

            // 2. Check for erroneous input:
            // a. If the length of the ciphertext C is not nLen bytes, output an indication of erroneous
            // input, and exit without further processing.
            if (ciphertext.BitLength != nLenBits)
            {
                throw new DecryptionFailedException("Ciphertext bit length did not match nLen.");
            }

            // b. Convert the ciphertext byte string C to a ciphertext integer c
            // (see Appendix B.2):
            // c = BS2I(C).
            var c = ciphertext.ToPositiveBigInteger();

            // c. If the ciphertext integer c is not such that 1 < c < (n – 1), output an indication of
            // erroneous input, and exit without further processing.
            if (c <= 1 || c >= rsaKeyPair.PubKey.N - 1)
            {
                throw new DecryptionFailedException("Ciphertext integer c is not such that 1 < c < (n - 1).");
            }

            // 3. RSA decryption:
            // a. Apply RSADP (see Section 7.1.2) to the ciphertext integer c using the private key
            // (n, d) to produce an integer em:
            // em = RSADP((n, d), c).24
            var em = _rsa.Decrypt(c, rsaKeyPair.PrivKey, rsaKeyPair.PubKey).PlainText;

            // b. Convert the integer em to an encoded message EM, a byte string of nLen bytes (see
            // Appendix B.1):
            // EM = I2BS(em, nLen).
            var EM = new BitString(em).PadToModulusMsb(nLenBits);

            // 4. OAEP decoding:
            // a. Apply the selected hash function (see Section 5.1) to compute:
            // HA = H(A).
            // HA is a byte string of HLen bytes.
            var HA = _sha.HashMessage(additionalInput).Digest;

            // b. Separate the encoded message EM into a single byte Y, a byte string
            // maskedMGFSeed′ of HLen bytes, and a byte string maskedDB′ of nLen – HLen – 1
            // bytes as follows:
            // EM = Y || maskedMGFSeed′ || maskedDB′.
            var Y = EM.MSBSubstring(0, BitString.BITSINBYTE);
            var maskedMGFSeed = EM.MSBSubstring(BitString.BITSINBYTE, hLenBits);
            var maskedDB = EM.MSBSubstring(BitString.BITSINBYTE + hLenBits, (nLen - HLen - 1) * BitString.BITSINBYTE);

            // c. Apply the mask-generation function specified in Section 7.2.2.2 to compute:
            // mgfSeedMask′ = MGF(maskedDB′, HLen).
            var mgfSeedMask = _mgf.Generate(maskedDB, hLenBits);

            // d. Let mgfSeed′ = maskedMGFSeed′ ⊕ mgfSeedMask′.
            var mgfSeed = maskedMGFSeed.XOR(mgfSeedMask);

            // e. Apply the mask-generation function specified in Section 7.2.2.2 to compute:
            // dbMask′= MGF(mgfSeed′, nLen – HLen – 1).
            var dbMask = _mgf.Generate(mgfSeed, (nLen - HLen - 1) * BitString.BITSINBYTE);

            // f. Let DB′ = maskedDB′ ⊕ dbMask′.
            var DB = maskedDB.XOR(dbMask);

            // g. Separate DB′ into a byte string HA′ of HLen bytes and a byte string X of nLen –
            // 2HLen – 1 bytes as follows:
            // DB′ = HA′ || X.
            var HA2 = DB.MSBSubstring(0, hLenBits);
            var X = DB.MSBSubstring(hLenBits, (nLen - 2 * HLen - 1) * BitString.BITSINBYTE);

            // 5. Check for RSA-OAEP decryption errors:
            // a. DecryptErrorFlag = False.
            // b. If Y is not the 00 byte (i.e., the bit string 00000000), then DecryptErrorFlag = True.
            if (Y.ToPositiveBigInteger() != BigInteger.Zero)
            {
                throw new DecryptionFailedException("Y did not equal zero.");
            }

            // c. If HA′ does not equal HA, then DecryptErrorFlag = True.
            if (!HA.Equals(HA2))
            {
                throw new DecryptionFailedException("HA` did not equal HA.");
            }

            // d. If X does not have the form PS || 00000001 || K, where PS consists of zero or more
            // consecutive 00 bytes, then DecryptErrorFlag = True.
            var kStartByte = 0;
            for (var i = 0; i < X.BitLength.CeilingDivide(8); i++)
            {
                if (X[i] == 0x00)
                {
                    continue;
                }
                if (X[i] == 0x01)
                {
                    kStartByte = i + 1;
                    break;
                }

                throw new DecryptionFailedException("X did not have the form PS || 00000001 || K, where PS consists of zero or more consecutive 00 bytes");
            }

            var K = X.MSBSubstring(kStartByte * BitString.BITSINBYTE, X.BitLength - kStartByte * BitString.BITSINBYTE);

            return new SharedSecretResponse(K);
        }
    }
}

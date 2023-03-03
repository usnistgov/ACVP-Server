using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native.Keys;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS.Native
{
    public class Hss : IHssSigner, IHssVerifier
    {
        private readonly IHssKeyPairFactory _hssKeyPairFactory;
        private readonly ILmsSigner _lmsSigner;
        private readonly ILmsVerifier _lmsVerifier;
        private readonly ILogger<Hss> _logger;

        public Hss(ILogger<Hss> logger, IHssKeyPairFactory hssKeyPairFactory, ILmsSigner lmsSigner, ILmsVerifier lmsVerifier)
        {
            _logger = logger;
            _hssKeyPairFactory = hssKeyPairFactory;
            _lmsSigner = lmsSigner;
            _lmsVerifier = lmsVerifier;
        }

        public async Task<IHssSignatureResult> Sign(IHssKeyPair keyPair, ILmOtsRandomizerC randomizerC, byte[] message, int preSignIncrementQ)
        {
            /*
			 1. If the message-signing key prv[L-1] is exhausted, regenerate
		        that key pair, together with any parent key pairs that might
		        be necessary.

		        If the root key pair is exhausted, then the HSS key pair is
		        exhausted and MUST NOT generate any more signatures.
			 */
            var L = keyPair.Levels;
            var lmsKeyPairs = await keyPair.PrivateKey.Keys;
            if (!await _hssKeyPairFactory.RegenerateLmsTreesWhereRequired(keyPair, randomizerC, preSignIncrementQ))
            {
                return new HssSignatureResult("HSS key pair has been fully exhausted of keys.");
            }

            // 2. Sign the message.
            var messageSignatureResult = _lmsSigner.Sign(lmsKeyPairs[L - 1].PrivateKey, randomizerC, message);

            // The keypair can expire after signing a message, determine if it's now expired
            if (lmsKeyPairs.All(a => a.PrivateKey.IsExhausted))
            {
                keyPair.IsExhausted = true;
            }

            if (messageSignatureResult.Exhausted)
            {
                return new HssSignatureResult("Failed to sign message");
            }

            var signatures = await keyPair.PublicKey.Signatures;
            signatures[L - 1] = messageSignatureResult.Signature;

            // Since each level of tree can be generated with varying parameters,
            // signatures sizes can also vary per level.
            // Just iterate to count bytes, allocate, then iterate again, max n of o(n2) (o(n)) is 8.
            var totalSignatureBytes = 0 + 4;
            for (var i = 0; i < L - 1; i++)
            {
                totalSignatureBytes += signatures[i].Length;
                // Note the LMS tree to use for the public key is offset +1 from the current level.
                var lmsTree = lmsKeyPairs[i + 1];
                var publicKey = lmsTree.PublicKey.Key;
                totalSignatureBytes += publicKey.Length;
            }

            /*
			 3. Create the list of signed public keys.
		        i = 0;
		        while (i < L-1) {
		          signed_pub_key[i] = sig[i] || pub[i+1]
		          i = i + 1
		        }
			 */
            // 4. Return u32str(L-1) || signed_pub_key[0] || ... || signed_pub_key[L-2] || sig[L-1]

            totalSignatureBytes += signatures[L - 1].Length;
            var signature = new byte[totalSignatureBytes];
            var startIndex = 0;

            // L-1
            Array.Copy((L - 1).GetBytes(), 0, signature, startIndex, 4);
            startIndex += 4;

            for (var i = 0; i < L - 1; i++)
            {
                var sigPiece = signatures[i];
                // signed_pub_key[i] = sig[i] || pub[i+1]
                Array.Copy(sigPiece, 0, signature, startIndex, sigPiece.Length);
                startIndex += sigPiece.Length;
                var pubKeyPiece = lmsKeyPairs[i + 1].PublicKey.Key;
                Array.Copy(pubKeyPiece, 0, signature, startIndex, pubKeyPiece.Length);
                startIndex += pubKeyPiece.Length;
            }

            var lastSig = signatures[L - 1];
            Array.Copy(lastSig, 0, signature, startIndex, lastSig.Length);

            return new HssSignatureResult(signature);
        }

        public async Task<bool> Verify(IHssPublicKey publicKey, byte[] signature, byte[] message)
        {
            /*
			    To verify a signature S and message using the public key pub, perform
			   the following steps:

			     The signature S is parsed into its components as follows:

			     Nspk = strTou32(first four bytes of S)
			     if Nspk+1 is not equal to the number of levels L in pub:
			       return INVALID
			     for (i = 0; i < Nspk; i = i + 1) {
			       siglist[i] = next LMS signature parsed from S
			       publist[i] = next LMS public key parsed from S
			     }
			     siglist[Nspk] = next LMS signature parsed from S

			     key = pub
			     for (i = 0; i < Nspk; i = i + 1) {
			       sig = siglist[i]
			       msg = publist[i]
			       if (lms_verify(msg, key, sig) != VALID):
			         return INVALID
			       key = msg
			     }
			     return lms_verify(message, key, siglist[Nspk])
			 */

            // Nspk = strTou32(first four bytes of S)
            var nspk = (int)new BitString(signature.Take(4).ToArray()).ToPositiveBigInteger();

            // if Nspk+1 is not equal to the number of levels L in pub: return INVALID
            if (nspk + 1 != publicKey.Levels)
            {
                throw new ArgumentException(
                    "Signature is invalid, number of trees as indicated by the signature does not match number of trees as indicated by the public key.");
            }

            /*
			 for (i = 0; i < Nspk; i = i + 1) {
			       siglist[i] = next LMS signature parsed from S
			       publist[i] = next LMS public key parsed from S
			     }
			 */
            var sigList = new byte[nspk + 1][];
            var pubList = new byte[nspk][];

            var currentIndex = 4;
            // Making the assumption that the signature is properly formed could lead to exceptions,
            // catch them and return false if bad signature.
            try
            {
                for (var i = 0; i < nspk; i++)
                {
                    var otsCode = signature.Skip(currentIndex + 4).Take(4).ToArray();
                    var otsAttribute =
                        AttributesHelper.GetLmOtsAttribute(AttributesHelper.GetLmOtsModeFromTypeCode(otsCode));

                    var sigTypeCode = signature.Skip(8 + otsAttribute.N * (otsAttribute.P + 1) + currentIndex)
                        .Take(4)
                        .ToArray();
                    var sigTypeAttribute =
                        AttributesHelper.GetLmsAttribute(AttributesHelper.GetLmsModeFromTypeCode(sigTypeCode));

                    var sigLen = 12 + otsAttribute.N * (otsAttribute.P + 1) + sigTypeAttribute.M * sigTypeAttribute.H;
                    var pubLen = 24 + sigTypeAttribute.M;
                    sigList[i] = signature.Skip(currentIndex).Take(sigLen).ToArray();
                    currentIndex += sigLen;
                    pubList[i] = signature.Skip(currentIndex).Take(pubLen).ToArray();
                    currentIndex += pubLen;
                }

                var otsCodeLast = signature.Skip(currentIndex + 4).Take(4).ToArray();
                var otsAttributeLast =
                    AttributesHelper.GetLmOtsAttribute(AttributesHelper.GetLmOtsModeFromTypeCode(otsCodeLast));

                var sigCodeLast = signature.Skip(8 + otsAttributeLast.N * (otsAttributeLast.P + 1) + currentIndex)
                    .Take(4)
                    .ToArray();
                var sigTypeAttributeLast =
                    AttributesHelper.GetLmsAttribute(AttributesHelper.GetLmsModeFromTypeCode(sigCodeLast));

                var sigLenLast = 12 + otsAttributeLast.N * (otsAttributeLast.P + 1) +
                                  sigTypeAttributeLast.M * sigTypeAttributeLast.H;
                sigList[nspk] = signature.Skip(currentIndex).Take(sigLenLast).ToArray();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed parsing signature");
                return false;
            }

            var key = (await publicKey.Key).Skip(4).ToArray();
            byte[] msg = null;
            for (var i = 0; i < nspk; i++)
            {
                var sig = sigList[i];
                msg = pubList[i];

                if (!_lmsVerifier.Verify(new LmsPublicKey(key), sig, msg).Success)
                    return false;

                key = msg;
            }

            return _lmsVerifier.Verify(new LmsPublicKey(key), sigList[nspk], message).Success;
        }
    }
}

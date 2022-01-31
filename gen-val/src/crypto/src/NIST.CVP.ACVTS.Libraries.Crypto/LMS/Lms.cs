using System.Threading;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.LMS
{
    public class Lms
    {
        #region Fields

        private readonly BitString D_LEAF = new BitString("8282");
        private readonly BitString D_INTR = new BitString("8383");
        private readonly int _m;
        private readonly int _h;
        private readonly BitString _I;
        private readonly BitString _typecode;
        private readonly BitString _lmotsTypecode;
        private readonly IEntropyProviderFactory _entropyFactory = new EntropyProviderFactory();
        private readonly IEntropyProvider _entropyProvider;
        private Lmots _lmots;
        private LmsTree _tree;
        private readonly BitString SEED;
        private int _pieceSize;
        private readonly bool _isRandom;
        private readonly ISha _sha256;

        // Used to determine how many publicKeys to compute for each call to PubPiece
        private const int H5_PIECE_SIZE = 32;       // creates 1 thread
        private const int H10_PIECE_SIZE = 32;      // creates 32 threads
        private const int H15_PIECE_SIZE = 1024;    // creates 32 threads
        private const int H20_PIECE_SIZE = 4096;    // creates 256 threads
        private const int H25_PIECE_SIZE = 32768;   // creates 1024 threads

        #endregion Fields

        #region Constructors

        // From an email from Scott Fluhrer:
        // To generate the I value for the LMS tree, we will adapt algorithm A, 
        // by setting the I value input to be the all-zeros value, the q value 
        // to be 0 and the i value to be 65535; we will use the first 16 bytes of the hash output.
        // Algorithm A:
        // x_q[i] = H(I || u32str(q) || u16str(i) || u8str(0xff) || SEED).
        // UPDATE: I value is now generated separate from SEED. Child I values still computed the same
        public Lms(LmsType lmsType, LmotsType lmotsType,
            EntropyProviderTypes entropyType = EntropyProviderTypes.Random, BitString seed = null, BitString I = null)
        {
            var (m, h, typecode) = LmsModeMapping.GetValsFromType(lmsType);
            _m = m;
            _h = h;
            _typecode = typecode;
            var param = LmotsModeMapping.GetValsFromType(lmotsType);
            _lmotsTypecode = param.typecode;
            _entropyProvider = _entropyFactory.GetEntropyProvider(entropyType);
            _entropyProvider.AddEntropy(seed);
            SEED = _entropyProvider.GetEntropy(_m * 8);
            _entropyProvider.AddEntropy(I);
            _I = _entropyProvider.GetEntropy(128);
            _lmots = new Lmots(lmotsType, entropyType);
            _isRandom = entropyType == EntropyProviderTypes.Random;
            _sha256 = new NativeFastSha2_256();

            // For optimization the balance between interop calls and asynchronization
            if (_h == 5)
            {
                _pieceSize = H5_PIECE_SIZE;
            }
            else if (_h == 10)
            {
                _pieceSize = H10_PIECE_SIZE;
            }
            else if (_h == 15)
            {
                _pieceSize = H15_PIECE_SIZE;
            }
            else if (_h == 20)
            {
                _pieceSize = H20_PIECE_SIZE;
            }
            else
            {
                _pieceSize = H25_PIECE_SIZE;
            }
        }

        #endregion Constructors

        #region Accessors
        public BitString GetI()
        {
            return _I.GetDeepCopy();
        }

        public BitString GetSeed()
        {
            return SEED.GetDeepCopy();
        }

        public int GetH()
        {
            return _h;
        }

        public byte[] GetTree()
        {
            return _tree.GetTreeBytes();
        }

        public int GetLmotsN()
        {
            return _lmots.GetN();
        }

        public int GetLmotsP()
        {
            return _lmots.GetP();
        }
        #endregion Accessors

        #region KeyGeneration

        /// <summary>
        /// Generates a LMS key pair asynchronously. Safe to call inside of Orleans.
        /// </summary>
        /// <returns>Valid Lms Key Pair</returns>
        public async Task<LmsKeyPair> GenerateLmsKeyPairAsync()
        {
            var ots_priv = GenerateOtsPriv();

            var pub = await GeneratePub(ots_priv, _pieceSize);

            var pubKey = GenerateLmsPublicKey(pub);

            var keyPair = new LmsKeyPair(new LmsPrivateKey(ots_priv, pub, _lmots.GetN(), _lmots.GetP()), pubKey);

            return keyPair;
        }

        #region Helpers

        /// <summary>
        /// Wrapper function for interop call to create OTS_PRIV
        /// </summary>
        /// <returns>OTS_PRIV as a byte array</returns>
        private byte[] GenerateOtsPriv()
        {
            return LmsDllLoader.GenOtsPrivLms(_h, _I.ToBytes(), SEED.ToBytes(), _lmots.GetN(), _lmots.GetP(), _lmotsTypecode.ToBytes());
        }

        /// <summary>
        /// Generates pub as described in RFC 8554
        /// </summary>
        /// <param name="ots_priv"></param>
        /// <param name="pieceSize">Size of each piece generated given in number of public keys</param>
        /// <returns>pub as a byte array</returns>
        private async Task<byte[]> GeneratePub(byte[] ots_priv, long pieceSize)
        {
            var tasks = new Task[(1 << _h) / pieceSize];
            var pub = new byte[(1 << _h) * _lmots.GetN()];
            for (long i = 0; i < tasks.Length; i++)
            {
                tasks[i] = GeneratePubPiece(i * pieceSize, i * pieceSize + pieceSize, ots_priv, pub);
            }
            await Task.WhenAll(tasks);

            return pub;
        }

        /// <summary>
        /// A wrapper for the interop function that is safe within Orleans.
        /// The task factory forces the new thread to be on the NonOrleansThreadScheduler
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="ots_priv"></param>
        /// <param name="pub_piece"></param>
        private async Task GeneratePubPiece(long start, long end, byte[] ots_priv, byte[] pub)
        {
            var taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskContinuationOptions.None, TaskScheduler.Current);

            await taskFactory.StartNew(() =>
            {
                LmsDllLoader.GenPubPiece(start, end, _h, _m, ots_priv, _lmots.GetP(), _lmots.GetN(), _lmots.GetW(), pub);
            });
        }

        /// <summary>
        /// Builds tree from pub and also the public key as a BitString
        /// </summary>
        /// <param name="pub">A bunch of last 32 bytes of public keys</param>
        /// <returns>LMS public key</returns>
        private BitString GenerateLmsPublicKey(byte[] pub)
        {
            var tree = LmsDllLoader.BuildTreeWithPub(_h, _m, _I.ToBytes(), pub, _lmots.GetP(), _lmots.GetN(), _lmots.GetW());

            _tree = new LmsTree(_h, _m, tree);

            var publicKey = _typecode.ConcatenateBits(_lmotsTypecode)
                .ConcatenateBits(_I)
                .ConcatenateBits(_tree.GetRoot());

            return publicKey;
        }

        #endregion Helpers

        #endregion KeyGeneration

        #region Signature Generation

        public BitString GenerateLmsSignature(BitString msg, LmsPrivateKey privateKey)
        {
            // 0. Use pub to generate tree if it is not null
            if (privateKey.Pub != null)
            {
                var tree = LmsDllLoader.BuildTreeWithPub(_h, _m, _I.ToBytes(), privateKey.Pub, _lmots.GetP(), _lmots.GetN(), _lmots.GetW());

                _tree = new LmsTree(_h, _m, tree);
            }

            // 1. Generate ots with next available leaf (in private key)
            var q = privateKey.Q;
            if (q * ((_lmots.GetN() * _lmots.GetP()) + 24) >= privateKey.OTS_PRIV.Length)
            {
                return null;
            }
            var lmots_signature = _lmots.GenerateLmotsSignature(msg, new BitString(privateKey.GetLmotsPrivateKeyQ(q)), SEED);

            // 2. Increment q in private key to ensure no reusablility of key
            // privateKey.UpdateQ();
            // currently done in hss

            // 3. Determine path[]
            var path = new BitString("");
            int currentIndex = (1 << _h) + q;
            for (int i = 0; i < _h; i++)
            {
                path = path.ConcatenateBits(_tree.GetSibling(currentIndex));
                currentIndex /= 2;
            }

            // 4. u32str(q) || lmots_signature || u32str(type) || path[0] || path[1] || path[2] || ... || path[h - 1]
            return new BitString(q, 32)
                .ConcatenateBits(lmots_signature)
                .ConcatenateBits(_typecode)
                .ConcatenateBits(path);
        }

        #endregion Signature Generation

        #region Signature Verification

        public LmsVerificationResult VerifyLmsSignature(BitString msg, BitString publicKey, BitString signature)
        {
            // 1. If the public key is not at least eight bytes long, return INVALID.
            if (publicKey.BitLength < 64)
            {
                return new LmsVerificationResult("Validation failed. Public key wrong length.");
            }

            // 2. Parse pubtype, I, and T[1] from the public key as follows:
            // a. pubtype = strTou32(first 4 bytes of public key)
            var pubType = publicKey.MSBSubstring(0, 32);

            // b. ots_typecode = strTou32(next 4 bytes of public key)
            var ots_typecode = publicKey.MSBSubstring(32, 32);

            // c. Set m according to pubtype, based on Table 2.
            var m = LmsModeMapping.GetMFromCode(pubType);

            // d. If the public key is not exactly 24 + m bytes long, return INVALID.
            if (publicKey.BitLength != (24 + m) * 8)
            {
                return new LmsVerificationResult("Validation failed. Public key wrong length.");
            }

            // e. I = next 16 bytes of the public key
            var I = publicKey.MSBSubstring(64, 128);

            // f. T[1] = next m bytes of the public key
            var root = publicKey.MSBSubstring(192, m * 8);

            // 3. Compute the LMS Public Key Candidate Tc from the signature, message, identifier,
            //    pubtype, and ots_typecode, using Algorithm 6a.

            // Algorithm 6a:

            // 1. If the signature is not at least eight bytes long, return INVALID.
            if (signature.BitLength < 64)
            {
                return new LmsVerificationResult("Validation failed. Signature wrong length.");
            }

            // 2.  Parse sigtype, q, lmots_signature, and path from the signature as follows:
            // a. q = strTou32(first 4 bytes of signature)
            var q = signature.MSBSubstring(0, 32);

            // b. otssigtype = strTou32(next 4 bytes of signature)
            var otssigtype = signature.MSBSubstring(32, 32);

            // c. If otssigtype is not the OTS typecode from the public key, return INVALID.
            if (!otssigtype.Equals(ots_typecode))
            {
                return new LmsVerificationResult("Validation failed. OTS Code incongruent.");
            }

            // d. Set n, p according to otssigtype and Table 1; if the signature is not 
            //    at least 12 + n* (p + 1) bytes long, return INVALID.
            var n = LmotsModeMapping.GetNFromCode(otssigtype);
            var p = LmotsModeMapping.GetPFromCode(otssigtype);
            if (signature.BitLength < (12 + n * (p - 1)) * 8)
            {
                return new LmsVerificationResult("Validation failed. Signature wrong length.");
            }

            // e. lmots_signature = bytes 4 through 7 + n* (p + 1) of signature
            var lmots_signature = signature.MSBSubstring(32, (4 + n * (p + 1)) * 8);

            // f. sigtype = strTou32(bytes 8 + n* (p + 1)) through 11 + n* (p + 1) of signature)
            var sigtype = signature.MSBSubstring((8 + n * (p + 1)) * 8, 32);

            // g. If sigtype is not the LM typecode from the public key, return INVALID.
            if (!sigtype.Equals(pubType))
            {
                return new LmsVerificationResult("Verification failed. Type mismatch.");
            }

            // h. Set m, h according to sigtype and Table 2.
            // m already set from above
            var h = LmsModeMapping.GetHFromCode(sigtype);

            // i. If q >= 2^h or the signature is not exactly 12 + n* (p + 1) + m* h bytes long, return INVALID.
            if (q.ToPositiveBigInteger() >= (1 << h) || signature.BitLength != (12 + n * (p + 1) + m * h) * 8)
            {
                return new LmsVerificationResult("Verification failed. Signature wrong length.");
            }

            // j. Set path as follows:
            //    path[0] = next m bytes of signature
            //    path[1] = next m bytes of signature
            //    ...
            //    path[h-1] = next m bytes of signature
            var path = new BitString[h];
            for (int i = 0; i < h; i++)
            {
                path[i] = signature.MSBSubstring(((12 + n * (p + 1)) * 8) + (i * m * 8), m * 8);
            }

            // 3.  Kc = candidate public key computed by applying Algorithm 4b to the signature lmots_signature, 
            //     the message, and the identifiers I, q
            var Kc = _lmots.Algorithm4b(lmots_signature, msg, otssigtype, I, q);
            if (Kc == null)
            {
                return new LmsVerificationResult("Verification failed. Algorithm 4b failed.");
            }

            // 4. Compute the candidate LMS root value Tc as described in rfc 8554
            var node_num = (1 << h) + q.ToPositiveBigInteger();

            var tmp = _sha256.HashMessage(I
                .ConcatenateBits(new BitString(node_num, 32))
                .ConcatenateBits(D_LEAF)
                .ConcatenateBits(Kc)).Digest;

            for (int i = 0; node_num > 1; node_num /= 2)
            {
                if (node_num % 2 == 1)
                {
                    tmp = _sha256.HashMessage(I
                        .ConcatenateBits(new BitString(node_num / 2, 32))
                        .ConcatenateBits(D_INTR)
                        .ConcatenateBits(path[i])
                        .ConcatenateBits(tmp)).Digest;
                }
                else
                {
                    tmp = _sha256.HashMessage(I
                        .ConcatenateBits(new BitString(node_num / 2, 32))
                        .ConcatenateBits(D_INTR)
                        .ConcatenateBits(tmp)
                        .ConcatenateBits(path[i])).Digest;
                }
                i++;
            }

            // 5. Return Tc
            var Tc = tmp;

            // END Algorithm 6a

            // 4. If Tc is equal to T[1], return VALID; otherwise, return INVALID.
            if (Tc.Equals(root))
            {
                return new LmsVerificationResult();
            }
            else
            {
                return new LmsVerificationResult("Verification failed. Signature invalid.");
            }
        }
        #endregion Signature Verification
    }
}

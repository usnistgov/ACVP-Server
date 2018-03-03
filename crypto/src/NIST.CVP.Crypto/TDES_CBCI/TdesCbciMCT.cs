using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.Common.Symmetric.TDES.Enums;

namespace NIST.CVP.Crypto.TDES_CBCI
{
    public class TdesCbciMCT : ITDES_CBCI_MCT //TODO convert to interface
    {
        private readonly IMonteCarloKeyMaker _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        //private const int NUMBER_OF_CASES = 5;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases => NUMBER_OF_CASES;

        public TdesCbciMCT(IMonteCarloKeyMaker keyMaker)
        {

            _keyMaker = keyMaker;
        }
        public MCTResult<AlgoArrayResponseWithIvs> MCTEncrypt(BitString keyBits, BitString iv, BitString data)
        {
            if (data.BitLength != 192)
            {
                throw new ArgumentException("Data must be 192 bits");
            }
            var ivs = SetupIvs(iv);
            var pts = new[] {data.MSBSubstring(0, 64), data.MSBSubstring(64, 64), data.MSBSubstring(128, 64)};

            var responses = new List<AlgoArrayResponseWithIvs>{
                new AlgoArrayResponseWithIvs {
                    IV1 = ivs[0],
                    IV2 = ivs[1],
                    IV3 = ivs[2],
                    Keys = keyBits,
                    PlainText = data
                }
            };
            var lastCipherTexts = new List<BitString>();
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        var encryptionResult = new BitString(EncryptWorker(responses[i].Keys, ivs[k].XOR(pts[k]).ToBytes()));
                        pts[k] = ivs[k].GetDeepCopy();
                        ivs[k] = encryptionResult.GetDeepCopy();
                    }
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, ivs[NUMBER_OF_ITERATIONS - 1 - j].GetDeepCopy());
                    }
                }

                responses.Last().CipherText = ivs[0].ConcatenateBits(ivs[1]).ConcatenateBits(ivs[2]);

                responses.Add(new AlgoArrayResponseWithIvs {
                        IV1 = ivs[0],
                        IV2 = ivs[1],
                        IV3 = ivs[2],
                        Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastCipherTexts.ToList()).ToOddParityBitString(),
                        PlainText = pts[0].ConcatenateBits(pts[1].ConcatenateBits(pts[2]))});
            }
            responses.RemoveAt(responses.Count() - 1);
            return new MCTResult<AlgoArrayResponseWithIvs>(responses);
        }

        public MCTResult<AlgoArrayResponseWithIvs> MCTDecrypt(BitString keyBits, BitString iv, BitString data)
        {
            if (data.BitLength != 192)
            {
                throw new ArgumentException("Data must be 192 bits");
            }
            var ivs = SetupIvs(iv);
            var cts = new[] { data.MSBSubstring(0, 64), data.MSBSubstring(64, 64), data.MSBSubstring(128, 64) };

            var responses = new List<AlgoArrayResponseWithIvs>{
                new AlgoArrayResponseWithIvs {
                    IV1 = ivs[0],
                    IV2 = ivs[1],
                    IV3 = ivs[2],
                    Keys = keyBits,
                    CipherText = data
                }
            };
            var lastPlainTexts = new List<BitString>();
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - NUMBER_OF_OUTPUTS_TO_SAVE;
            for (var i = 0; i < NumberOfCases; i++)
            {
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        var encryptionResult = new BitString(DecryptWorker(responses[i].Keys, cts[k].ToBytes())).XOR(ivs[k]);
                        ivs[k] = cts[k].GetDeepCopy();
                        cts[k] = encryptionResult.GetDeepCopy();
                    }
                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, cts[NUMBER_OF_ITERATIONS - 1 - j].GetDeepCopy());
                    }
                }

                responses.Last().PlainText = cts[0].ConcatenateBits(cts[1]).ConcatenateBits(cts[2]);

                responses.Add(new AlgoArrayResponseWithIvs
                {
                    IV1 = ivs[0],
                    IV2 = ivs[1],
                    IV3 = ivs[2],
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastPlainTexts.ToList()).ToOddParityBitString(),
                    CipherText = cts[0].ConcatenateBits(cts[1].ConcatenateBits(cts[2]))
                });
            }
            responses.RemoveAt(responses.Count - 1);
            return new MCTResult<AlgoArrayResponseWithIvs>(responses);
        }

        private BitString[] SetupIvs(BitString iv)
        {
            //TODO can be moved to the TDES project
            return new[]{ iv,
                iv.AddWithModulo(new BitString("5555555555555555"), 64),
                iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64)};
        }

        private byte[] EncryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Encryption);
            byte[] interm1 = context.Schedule[0].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[2].Apply(interm2);
            return output;
        }

        private byte[] DecryptWorker(BitString keyBits, byte[] input)
        {
            var keys = new TDESKeys(keyBits);
            var context = new TDESContext(keys, FunctionValues.Decryption);
            byte[] interm1 = context.Schedule[2].Apply(input);
            byte[] interm2 = context.Schedule[1].Apply(interm1);
            byte[] output = context.Schedule[0].Apply(interm2);
            return output;
        }
    }
}

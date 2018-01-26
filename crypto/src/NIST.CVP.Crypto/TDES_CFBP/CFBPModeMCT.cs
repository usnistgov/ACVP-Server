using NIST.CVP.Crypto.Common;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Symmetric.TDES;

namespace NIST.CVP.Crypto.TDES_CFBP
{
    public class CFBPModeMCT : ICFBPModeMCT
    {
        private readonly IMonteCarloKeyMaker _keyMaker;
        private readonly ICFBPMode _cfbpMode;

        private const int NUMBER_OF_CASES = 400;
        //private const int NUMBER_OF_CASES = 5;
        private const int NUMBER_OF_ITERATIONS = 10000;


        protected virtual int NumberOfCases { get { return NUMBER_OF_CASES; } }
        public int Shift { get; set; }
        public ICFBPMode ModeOfOperation { get; set; }
        public Algo Algo { get; set; }

        public CFBPModeMCT(IMonteCarloKeyMaker keyMaker, ICFBPMode cfbpMode, Algo algo)
        {
            switch (algo)
            {
                case Algo.TDES_CFBP1:
                    Shift = 1;
                    break;
                case Algo.TDES_CFBP8:
                    Shift = 8;
                    break;
                case Algo.TDES_CFBP64:
                    Shift = 64;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algo), algo, null);
            }
            Algo = algo;
            _keyMaker = keyMaker;
            _cfbpMode = cfbpMode;
        }

        public MCTResult<AlgoArrayResponseWithIvs> MCTEncrypt(BitString keyBits, BitString iv, BitString data)
        {
            var responses = new List<AlgoArrayResponseWithIvs>{
                new AlgoArrayResponseWithIvs {
                    IV1 = iv,
                    IV2 = iv.AddWithModulo(new BitString("5555555555555555"), 64),
                    IV3 = iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64),
                    Keys = keyBits,
                    PlainText = data
                }
            };
            var holdouts = new BitString[3];
            var numberOfOutputsToSave = 192 / Shift;
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;

            for (var i = 0; i < NumberOfCases; i++)
            {
                var keysForThisRound = responses[i].Keys;
                BitString output = null;
                var lastCipherTexts = new List<BitString>();
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    switch (j)
                    {
                        case 0:
                            iv = responses[i].IV1;
                            break;
                        case 1:
                            iv = responses[i].IV2;
                            break;
                        case 2:
                            iv = responses[i].IV3;
                            break;
                        default:
                            iv = iv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(holdouts[2]);
                            break;
                    }


                    output = _cfbpMode.BlockEncrypt(keysForThisRound, iv, data).Result;

                    holdouts[2] = holdouts[1];
                    holdouts[1] = holdouts[0];
                    holdouts[0] = output;

                    data = iv.MSBSubstring(0, Shift);

                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastCipherTexts.Insert(0, output.GetDeepCopy());
                    }

                }
                responses[i].CipherText = output;

                var newIv = iv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(output);
                responses.Add(new AlgoArrayResponseWithIvs()
                {
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastCipherTexts.ToList())
                        .ToOddParityBitString(),
                    PlainText = data,
                    IV1 = newIv,
                    IV2 = newIv.AddWithModulo(new BitString("5555555555555555"), 64),
                    IV3 = newIv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64),
                });
            }
            responses.RemoveAt(responses.Count - 1);
            return new MCTResult<AlgoArrayResponseWithIvs>(responses);
        }

        public MCTResult<AlgoArrayResponseWithIvs> MCTDecrypt(BitString keyBits, BitString iv, BitString data)
        {
            var responses = new List<AlgoArrayResponseWithIvs>{
                new AlgoArrayResponseWithIvs {
                    IV1 = iv,
                    IV2 = iv.AddWithModulo(new BitString("5555555555555555"), 64),
                    IV3 = iv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64),
                    Keys = keyBits,
                    CipherText = data
                }
            };
            var holdouts = new BitString[3];
            var numberOfOutputsToSave = 192 / Shift;
            var indexAtWhichToStartSaving = NUMBER_OF_ITERATIONS - numberOfOutputsToSave;

            for (var i = 0; i < NumberOfCases; i++)
            {
                var keysForThisRound = responses[i].Keys;
                BitString output = null;
                var lastPlainTexts = new List<BitString>();
                for (var j = 0; j < NUMBER_OF_ITERATIONS; j++)
                {
                    switch (j)
                    {
                        case 0:
                            iv = responses[i].IV1;
                            break;
                        case 1:
                            iv = responses[i].IV2;
                            break;
                        case 2:
                            iv = responses[i].IV3;
                            break;
                        default:
                            iv = iv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(holdouts[2]);
                            break;
                    }


                    output = _cfbpMode.BlockDecrypt(keysForThisRound, iv, data).Result;

                    holdouts[2] = holdouts[1];
                    holdouts[1] = holdouts[0];
                    holdouts[0] = data;

                    data = data.XOR(output);

                    if (j >= indexAtWhichToStartSaving)
                    {
                        lastPlainTexts.Insert(0, output.GetDeepCopy());
                    }

                }
                responses[i].PlainText = output;

                var newIv = iv.MSBSubstring(Shift, 64 - Shift).ConcatenateBits(data.XOR(output));
                responses.Add(new AlgoArrayResponseWithIvs()
                {
                    Keys = _keyMaker.MixKeys(new TDESKeys(responses[i].Keys.GetDeepCopy()), lastPlainTexts.ToList())
                        .ToOddParityBitString(),
                    CipherText = data,
                    IV1 = newIv,
                    IV2 = newIv.AddWithModulo(new BitString("5555555555555555"), 64),
                    IV3 = newIv.AddWithModulo(new BitString("AAAAAAAAAAAAAAAA"), 64),
                });
            }
            responses.RemoveAt(responses.Count - 1);
            return new MCTResult<AlgoArrayResponseWithIvs>(responses);
        }

    }
}

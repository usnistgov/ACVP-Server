using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;
using NLog;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Crypto.TDES_ECB
{
    public class TDES_ECB_MCT : ITDES_ECB_MCT
    {
        private readonly ITDES_ECB _algo;
        private readonly IMonteCarloKeyMaker _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        //private const int NUMBER_OF_CASES = 5;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases { get { return NUMBER_OF_CASES; } }

        public TDES_ECB_MCT(ITDES_ECB algo, IMonteCarloKeyMaker keyMaker)
        {
            _algo = algo;
            _keyMaker = keyMaker;
        }

        public Common.Symmetric.TDES.MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            BitString originalPlainTextForOutput = data.GetDeepCopy();
            List<BitString> lastCipherTexts = new List<BitString>();
            AlgoArrayResponse tempAlgoArrayResponse = new AlgoArrayResponse()
            {
                Keys = keyBits.GetDeepCopy(),
                PlainText = originalPlainTextForOutput.GetDeepCopy()
            };

            for (int outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                if (originalPlainTextForOutput == null)
                {
                    originalPlainTextForOutput = tempAlgoArrayResponse.PlainText.GetDeepCopy();
                }

                SymmetricCipherResult encryptionResult = null;
                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    encryptionResult = _algo.BlockEncrypt(tempAlgoArrayResponse.Keys, tempAlgoArrayResponse.PlainText);
                    if (!encryptionResult.Success)
                    {
                        ThisLogger.Warn(encryptionResult.ErrorMessage);
                        {
                            return new Common.Symmetric.TDES.MCTResult<AlgoArrayResponse>(encryptionResult.ErrorMessage);
                        }
                    }

                    SaveOutputForKeyMixing(encryptionResult.Result.GetDeepCopy(), lastCipherTexts);
                    tempAlgoArrayResponse.PlainText = encryptionResult.Result.GetDeepCopy();
                }

                // Inner loop complete, save response
                responses.Add(
                    new AlgoArrayResponse()
                    {
                        Keys = tempAlgoArrayResponse.Keys.GetDeepCopy(),
                        PlainText = originalPlainTextForOutput.GetDeepCopy(),
                        CipherText = encryptionResult.Result.GetDeepCopy()
                    });

                originalPlainTextForOutput = null;

                // Setup next loop values
                tempAlgoArrayResponse.Keys =
                    _keyMaker.MixKeys(new TDESKeys(tempAlgoArrayResponse.Keys.GetDeepCopy()), lastCipherTexts)
                        .ToOddParityBitString();
                tempAlgoArrayResponse.PlainText = encryptionResult.Result.GetDeepCopy();
            }

            return new Common.Symmetric.TDES.MCTResult<AlgoArrayResponse>(responses);
        }

        public Common.Symmetric.TDES.MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            BitString originalCipherTextForOutput = data.GetDeepCopy();
            List<BitString> lastPlainTexts = new List<BitString>();
            AlgoArrayResponse tempAlgoArrayResponse = new AlgoArrayResponse()
            {
                Keys = keyBits.GetDeepCopy(),
                CipherText = originalCipherTextForOutput.GetDeepCopy()
            };

            for (int outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                if (originalCipherTextForOutput == null)
                {
                    originalCipherTextForOutput = tempAlgoArrayResponse.CipherText.GetDeepCopy();
                }

                SymmetricCipherResult decryptionResult = null;
                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    decryptionResult = _algo.BlockDecrypt(tempAlgoArrayResponse.Keys, tempAlgoArrayResponse.CipherText);
                    if (!decryptionResult.Success)
                    {
                        ThisLogger.Warn(decryptionResult.ErrorMessage);
                        {
                            return new Common.Symmetric.TDES.MCTResult<AlgoArrayResponse>(decryptionResult.ErrorMessage);
                        }
                    }

                    SaveOutputForKeyMixing(decryptionResult.Result.GetDeepCopy(), lastPlainTexts);
                    tempAlgoArrayResponse.CipherText = decryptionResult.Result.GetDeepCopy();
                }

                // Inner loop complete, save response
                responses.Add(
                    new AlgoArrayResponse()
                    {
                        Keys = tempAlgoArrayResponse.Keys.GetDeepCopy(),
                        PlainText = decryptionResult.Result.GetDeepCopy(),
                        CipherText = originalCipherTextForOutput.GetDeepCopy()
                    });

                originalCipherTextForOutput = null;

                // Setup next loop values
                tempAlgoArrayResponse.Keys =
                    _keyMaker.MixKeys(new TDESKeys(tempAlgoArrayResponse.Keys.GetDeepCopy()), lastPlainTexts)
                        .ToOddParityBitString();
                tempAlgoArrayResponse.CipherText = decryptionResult.Result.GetDeepCopy();
            }

            return new Common.Symmetric.TDES.MCTResult<AlgoArrayResponse>(responses);
        }

        private void SaveOutputForKeyMixing(BitString output, List<BitString> lastAlgoOutputs)
        {
            lastAlgoOutputs.Insert(0, output);
            if (lastAlgoOutputs.Count > NUMBER_OF_OUTPUTS_TO_SAVE)
            {
                lastAlgoOutputs.RemoveRange(NUMBER_OF_OUTPUTS_TO_SAVE, lastAlgoOutputs.Count - NUMBER_OF_OUTPUTS_TO_SAVE);
            }
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}

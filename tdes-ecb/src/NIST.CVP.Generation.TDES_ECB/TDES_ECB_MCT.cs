using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TDES_ECB_MCT : ITDES_ECB_MCT
    {
        private readonly ITDES_ECB _algo;
        private readonly IMonteCarloKeyMaker _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases { get { return NUMBER_OF_CASES; } }

        public TDES_ECB_MCT(ITDES_ECB algo, IMonteCarloKeyMaker keyMaker)
        {
            _algo = algo;
            _keyMaker = keyMaker;
        }

        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data)
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

                EncryptionResult encryptionResult = null;
                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    encryptionResult = _algo.BlockEncrypt(tempAlgoArrayResponse.Keys, tempAlgoArrayResponse.PlainText);
                    if (!encryptionResult.Success)
                    {
                        ThisLogger.Warn(encryptionResult.ErrorMessage);
                        {
                            return new MCTResult<AlgoArrayResponse>(encryptionResult.ErrorMessage);
                        }
                    }

                    SaveOutputForKeyMixing(encryptionResult.CipherText.GetDeepCopy(), lastCipherTexts);
                    tempAlgoArrayResponse.PlainText = encryptionResult.CipherText.GetDeepCopy();
                }

                // Inner loop complete, save response
                responses.Add(
                    new AlgoArrayResponse()
                    {
                        Keys = tempAlgoArrayResponse.Keys.GetDeepCopy(),
                        PlainText = originalPlainTextForOutput.GetDeepCopy(),
                        CipherText = encryptionResult.CipherText.GetDeepCopy()
                    });

                originalPlainTextForOutput = null;

                // Setup next loop values
                tempAlgoArrayResponse.Keys =
                    _keyMaker.MixKeys(new TDESKeys(tempAlgoArrayResponse.Keys.GetDeepCopy()), lastCipherTexts)
                        .ToOddParityBitString();
                tempAlgoArrayResponse.PlainText = encryptionResult.CipherText.GetDeepCopy();
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data)
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

                DecryptionResult decryptionResult = null;
                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    decryptionResult = _algo.BlockDecrypt(tempAlgoArrayResponse.Keys, tempAlgoArrayResponse.CipherText);
                    if (!decryptionResult.Success)
                    {
                        ThisLogger.Warn(decryptionResult.ErrorMessage);
                        {
                            return new MCTResult<AlgoArrayResponse>(decryptionResult.ErrorMessage);
                        }
                    }

                    SaveOutputForKeyMixing(decryptionResult.PlainText.GetDeepCopy(), lastPlainTexts);
                    tempAlgoArrayResponse.CipherText = decryptionResult.PlainText.GetDeepCopy();
                }

                // Inner loop complete, save response
                responses.Add(
                    new AlgoArrayResponse()
                    {
                        Keys = tempAlgoArrayResponse.Keys.GetDeepCopy(),
                        PlainText = decryptionResult.PlainText.GetDeepCopy(),
                        CipherText = originalCipherTextForOutput.GetDeepCopy()
                    });

                originalCipherTextForOutput = null;

                // Setup next loop values
                tempAlgoArrayResponse.Keys =
                    _keyMaker.MixKeys(new TDESKeys(tempAlgoArrayResponse.Keys.GetDeepCopy()), lastPlainTexts)
                        .ToOddParityBitString();
                tempAlgoArrayResponse.CipherText = decryptionResult.PlainText.GetDeepCopy();
            }

            return new MCTResult<AlgoArrayResponse>(responses);
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

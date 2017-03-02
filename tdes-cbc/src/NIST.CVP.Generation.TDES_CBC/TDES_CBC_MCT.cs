using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.TDES;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TDES_CBC_MCT : ITDES_CBC_MCT
    {
        private readonly ITDES_CBC _algo;
        private readonly IMonteCarloKeyMaker _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases { get { return NUMBER_OF_CASES; } }

        public TDES_CBC_MCT(ITDES_CBC algo, IMonteCarloKeyMaker keyMaker)
        {
            _algo = algo;
            _keyMaker = keyMaker;
        }

        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data, BitString iv)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            BitString originalPlainTextForOutput = data.GetDeepCopy();
            BitString originalIVForOutput = iv.GetDeepCopy();

            List<BitString> lastCipherTexts = new List<BitString>();
            AlgoArrayResponse tempAlgoArrayResponse = new AlgoArrayResponse()
            {
                Keys = keyBits.GetDeepCopy(),
                PlainText = originalPlainTextForOutput.GetDeepCopy(),
                IV = iv.GetDeepCopy()
            };

            var cv = new BitString(64);
            var previousCipherText = new BitString(64);
            var copyOfPreviousCipherText = new BitString(64);


            for (int outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                if (originalPlainTextForOutput == null)
                {
                    originalPlainTextForOutput = tempAlgoArrayResponse.PlainText.GetDeepCopy();
                }

                if (outerLoop == 0)
                {
                    cv = iv.GetDeepCopy(); //For the first outerloop interation, set CV0 to IV.
                }
                
                EncryptionResult encryptionResult = null;


                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    encryptionResult = _algo.BlockEncrypt(tempAlgoArrayResponse.Keys, tempAlgoArrayResponse.PlainText, cv);
                    if (!encryptionResult.Success)
                    {
                        ThisLogger.Warn(encryptionResult.ErrorMessage);
                        {
                            return new MCTResult<AlgoArrayResponse>(encryptionResult.ErrorMessage);
                        }
                    }

                    SaveOutputForKeyMixing(encryptionResult.CipherText.GetDeepCopy(), lastCipherTexts);

                    if (innerLoop == 0)
                    {
                        tempAlgoArrayResponse.PlainText = cv.GetDeepCopy(); //Pj+1 = CV0
                    }
                    else
                    {

                        tempAlgoArrayResponse.PlainText = previousCipherText.GetDeepCopy(); //Pj+1 = Cj-1
                        copyOfPreviousCipherText = previousCipherText;
                    }

                    previousCipherText = encryptionResult.CipherText;
                    cv = encryptionResult.CipherText.GetDeepCopy(); //CVj+1 = Cj
                }

                // Inner loop complete, save response
                responses.Add(
                    new AlgoArrayResponse() //Send keys, P0, Cj, CV0
                    {
                        Keys = tempAlgoArrayResponse.Keys.GetDeepCopy(),
                        PlainText = originalPlainTextForOutput.GetDeepCopy(),
                        CipherText = encryptionResult.CipherText.GetDeepCopy(),
                        IV = iv.GetDeepCopy() //This will only return original IV.
                    });
                
                originalPlainTextForOutput = null;

                // Setup next loop values
                tempAlgoArrayResponse.Keys =
                    _keyMaker.MixKeys(new TDESKeys(tempAlgoArrayResponse.Keys.GetDeepCopy()), lastCipherTexts)
                        .ToOddParityBitString();
                tempAlgoArrayResponse.PlainText = copyOfPreviousCipherText.GetDeepCopy(); //P0 = Cj-1
                cv = encryptionResult.CipherText.GetDeepCopy();    
            }

            return new MCTResult<AlgoArrayResponse>(responses);
        }

        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data, BitString iv)
        {
            List<AlgoArrayResponse> responses = new List<AlgoArrayResponse>();

            BitString originalCipherTextForOutput = data.GetDeepCopy();
            List<BitString> lastPlainTexts = new List<BitString>();

            var cv = new BitString(64);
            var copyOfPreviousCipherText = new BitString(64);

            AlgoArrayResponse tempAlgoArrayResponse = new AlgoArrayResponse()
            {
                Keys = keyBits.GetDeepCopy(),
                CipherText = originalCipherTextForOutput.GetDeepCopy(),
                IV = iv.GetDeepCopy()
            };

            for (int outerLoop = 0; outerLoop < NumberOfCases; outerLoop++)
            {
                if (originalCipherTextForOutput == null)
                {
                    originalCipherTextForOutput = tempAlgoArrayResponse.CipherText.GetDeepCopy();
                }

                if(outerLoop == 0)
                {
                    cv = iv.GetDeepCopy(); //For the first outerloop interation, set CV0 to IV.
                }
                DecryptionResult decryptionResult = null;
                for (int innerLoop = 0; innerLoop < NUMBER_OF_ITERATIONS; innerLoop++)
                {
                    decryptionResult = _algo.BlockDecrypt(tempAlgoArrayResponse.Keys, tempAlgoArrayResponse.CipherText, cv);
                    if (!decryptionResult.Success)
                    {
                        ThisLogger.Warn(decryptionResult.ErrorMessage);
                        {
                            return new MCTResult<AlgoArrayResponse>(decryptionResult.ErrorMessage);
                        }
                    }

                    SaveOutputForKeyMixing(decryptionResult.PlainText.GetDeepCopy(), lastPlainTexts);
                    copyOfPreviousCipherText = tempAlgoArrayResponse.CipherText;

                    cv = tempAlgoArrayResponse.CipherText.GetDeepCopy(); //CVj+1 = Cj;
                    tempAlgoArrayResponse.CipherText = decryptionResult.PlainText.GetDeepCopy();
                }

                // Inner loop complete, save response
                responses.Add(
                    new AlgoArrayResponse()
                    {
                        Keys = tempAlgoArrayResponse.Keys.GetDeepCopy(),
                        PlainText = decryptionResult.PlainText.GetDeepCopy(),
                        CipherText = originalCipherTextForOutput.GetDeepCopy(),
                        IV = iv.GetDeepCopy()  //This will only return original IV.
                    });

                originalCipherTextForOutput = null;

                // Setup next loop values
                tempAlgoArrayResponse.Keys =
                    _keyMaker.MixKeys(new TDESKeys(tempAlgoArrayResponse.Keys.GetDeepCopy()), lastPlainTexts)
                        .ToOddParityBitString();

                cv = copyOfPreviousCipherText.GetDeepCopy();
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

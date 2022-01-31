using System;
using System.Collections;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE
{
    public class CSHAKE_MCT : ICSHAKE_MCT
    {
        private readonly ICSHAKE _iCSHAKE;
        private int NUM_OF_RESPONSES = 100;
        private bool _customizationHex;

        public CSHAKE_MCT(ICSHAKE iCSHAKE)
        {
            _iCSHAKE = iCSHAKE;
        }

        #region MonteCarloAlgorithm Pseudocode
        /*
            INPUT: The initial Msg is the length of the digest size

            MCT(Msg, MaxOutLen, MinOutLen, OutLenIncrement)
            {
              Range = (MaxOutLen – MinOutLen + 1);
              OutputLen = MaxOutLen;
              FunctionName = "";
              Customization = "";

              Output[0] = Msg;
              for (j = 0; j < 100; j++) 
              {
                for (i = 1; i < 1001; i++) 
                {
                  InnerMsg = Left(Output[i-1] || ZeroBits(128), 128);
                  Output[i] = CSHAKE(InnerMsg, OutputLen, FunctionName, Customization);
                  Rightmost_Output_bits = Right(Output[i], 16);
                  OutputLen = MinOutLen + (floor((Rightmost_Output_bits % Range) / OutLenIncrement) * OutLenIncrement);
                  Customization = BitsToString(InnerMsg || Rightmost_Output_bits);
                }

                OutputJ[j] = Output[1000];
              }

              return OutputJ;
            }
         */
        #endregion MonteCarloAlgorithm Pseudocode

        public MctResult<AlgoArrayResponseWithCustomization> MCTHash(HashFunction function, BitString message, MathDomain domain, bool customizationHex, bool isSample)
        {
            _customizationHex = customizationHex;

            if (isSample)
            {
                NUM_OF_RESPONSES = 3;
            }

            var responses = new List<AlgoArrayResponseWithCustomization>();
            var i = 0;
            var j = 0;
            var min = domain.GetDomainMinMax().Minimum;
            var max = domain.GetDomainMinMax().Maximum;
            var increment = domain.GetDomainMinMax().Increment;

            //var outputLen = (int)System.Math.Floor((double)max / 8) * 8;
            var outputLen = max;
            var customization = "";
            var functionName = "";
            var range = (max - min) + 1;
            var innerMessage = message.GetDeepCopy();

            try
            {
                for (i = 0; i < NUM_OF_RESPONSES; i++)
                {
                    var innerDigest = new BitString(0);
                    var iterationResponse = new AlgoArrayResponseWithCustomization() { };
                    iterationResponse.Message = innerMessage;
                    iterationResponse.Customization = customization;

                    for (j = 0; j < 1000; j++)
                    {
                        // Might not have 128 bits to pull from so we pad with 0                        
                        innerMessage = BitString.ConcatenateBits(innerMessage, BitString.Zeroes(128));
                        innerMessage = BitString.MSBSubstring(innerMessage, 0, 128);
                        function.DigestLength = outputLen;

                        var innerResult = _iCSHAKE.HashMessage(function, innerMessage, customization, functionName);
                        innerDigest = innerResult.Digest.GetDeepCopy();

                        // Will always have 16 bits to pull from
                        var rightmostBitString = BitString.Substring(innerDigest, 0, 16);
                        var rightmostBits = rightmostBitString.Bits;

                        //outputLen = min + (8 * rightmostBits.ToInt()) % range;
                        outputLen = min + (int)System.Math.Floor((double)(rightmostBits.ToInt() % range) / increment) * increment;
                        customization = GetStringFromBytes(BitString.ConcatenateBits(innerMessage, rightmostBitString).ToBytes());

                        innerMessage = innerDigest.GetDeepCopy();
                    }

                    iterationResponse.Digest = innerDigest.GetDeepCopy();
                    responses.Add(iterationResponse);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"i count {i}, j count {j}");
                ThisLogger.Error(ex);
                return new MctResult<AlgoArrayResponseWithCustomization>($"{ex.Message}; {outputLen}");
            }

            return new MctResult<AlgoArrayResponseWithCustomization>(responses);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();

        private string GetStringFromBytes(byte[] bytes)
        {
            var result = "";

            if (_customizationHex)
            {
                result += new BitString(bytes).ToHex();
            }
            else
            {
                foreach (var num in bytes)
                {
                    result += System.Text.Encoding.ASCII.GetString(new byte[] { (byte)((num % 26) + 65) });
                }
            }

            return result;
        }
    }
}

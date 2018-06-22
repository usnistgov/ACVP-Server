using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Crypto.CMAC
{
    public class CmacTdes : ICmac
    {
        private const int BlockSize = 64;
        private readonly IModeBlockCipher<SymmetricCipherResult> _tdes;
        private readonly BitString Rb64 = new BitString("000000000000001A");  //TODO this should be 1B, not 1A

        public int OutputLength => BlockSize;

        public CmacTdes(IModeBlockCipher<SymmetricCipherResult> tdes)
        {
            _tdes = tdes;
        }

        public MacResult Generate(BitString key, BitString message, int macLength = 0)
        {

            //http://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-38b.pdf
            //6.1 SUBKEY GENERATION

            //Prerequisites:
            //    block cipher CIPH with block size b;
            //    key K.

            //Output:
            //subkeys K1, K2.

            //Suggested Notation:
            //    SUBK(K).

            //Steps:
            //    1.Let L = CIPHK(0^b).
            var param = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, key, new BitString(BlockSize));
            var L = _tdes.ProcessPayload(param).Result;
            BitString K1, K2;
            //    2.If MSB1(L) = 0, then K1 = L << 1;
            if (!L.GetMostSignificantBits(1).Bits[0])
            {
                K1 = L.LSBShift(-1);
            }
            //Else K1 = (L << 1) XOR Rb; see Sec. 5.3 for the definition of Rb.
            else
            {
                K1 = L.LSBShift(-1).XOR(Rb64);
            }
            
            //    3.If MSB1(K1) = 0, then K2 = K1 << 1;
            if (!K1.GetMostSignificantBits(1).Bits[0])
            {
                K2 = K1.LSBShift(-1);
            }
            //    Else K2 = (K1 << 1) XOR Rb.
            else
            {
                K2 = K1.LSBShift(-1).XOR(Rb64);
            }
            //    4.Return K1, K2.


            //6.2 MAC Generation

            //Prerequisites:
            //    block cipher CIPH with block size b;
            //    key K;
            //    MAC length parameter Tlen

            //Input:
            //    message M of bit length Mlen.

            //Output:
            //    MAC T of bit length Tlen.

            //Suggested Notation:
            //    CMAC(K, M, Tlen) or, if Tlen is understood from the context, CMAC(K, M).

            //Steps:
            //    1. Apply the subkey generation process in Sec. 6.1 to K to produce K1 and K2.
            //    2. If Mlen = 0, let n = 1; else, let n = ceiling(Mlen / b).
            var n = message.BitLength == 0 ? 1 : System.Math.Ceiling(message.BitLength / (double)BlockSize);
            //    3. Let M1, M2, ... , Mn - 1, Mn* denote the unique sequence of bit strings 
            //       such that M = M1 || M2 || ... || Mn - 1 || Mn*,
            //       where M1, M2,..., Mn-1 are complete blocks.2

            //    4. If Mn* is a complete block, let Mn = K1 XOR Mn*; 
            

            var numOfBlocks = (int)System.Math.Ceiling(message.BitLength / (double)BlockSize);
            var s1 = message.BitLength > BlockSize ? message.MSBSubstring(0, (numOfBlocks - 1) * BlockSize) : new BitString(0);

            var lastBlock = message.BitLength != 0 ? message.MSBSubstring(s1.BitLength, message.BitLength - s1.BitLength) : new BitString(0);
            
            if (message.BitLength % BlockSize == 0 && message.BitLength != 0)
            {
                lastBlock = lastBlock.XOR(K1);
            }
            //       else, let Mn = K2 XOR (Mn* || 10^j), where j = nb - Mlen - 1.
            else
            {
                var padding = new BitString(BlockSize - lastBlock.BitLength);
                padding.Set(padding.BitLength - 1, true);
                lastBlock = K2.XOR(lastBlock.ConcatenateBits(padding));
            }
            message = s1.ConcatenateBits(lastBlock).GetDeepCopy();
            //if this was an empty message, it would have been padded with another block
            if (message.BitLength % BlockSize != 0)
            {
                throw new Exception("Message isn't composed of same sized blocks.");
            }
            numOfBlocks = message.BitLength / BlockSize;
            BitString prevC = new BitString(64);
            BitString currC = new BitString(64);
            for (var i = 0; i < numOfBlocks; i++)
            {
                var block = message.MSBSubstring(i * BlockSize, BlockSize);
                var param2 = new ModeBlockCipherParameters(BlockCipherDirections.Encrypt, key, prevC.XOR(block));
                currC = _tdes.ProcessPayload(param2).Result;
                prevC = currC;
            }

            //    5. Let C0 = 0^b.
            //    6. For i = 1 to n, let Ci = CIPHK(Ci - 1 XOR Mi).
            //    7. Let T = MSBTlen(Cn).
            //    8. Return T.
            try
            {
                BitString mac;
                if (macLength != 0)
                {
                    mac = currC.GetMostSignificantBits(macLength);
                }
                else
                {
                    mac = currC.GetDeepCopy();
                }

                return new MacResult(mac);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public MacResult Verify(BitString keyBits, BitString message, BitString macToVerify)
        {
            try
            {
                var mac = Generate(keyBits, message, macToVerify.BitLength);

                if (!mac.Success)
                {
                    return new MacResult(mac.ErrorMessage);
                }

                if (mac.Mac.Equals(macToVerify))
                {
                    return new MacResult(mac.Mac);
                }

                return new MacResult("CMAC did not match.");
            }
            catch (Exception ex)
            {
                ThisLogger.Debug($"keyLen:{keyBits.BitLength}; dataLen:{message.BitLength}");
                ThisLogger.Error(ex);
                return new MacResult(ex.Message);
            }
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
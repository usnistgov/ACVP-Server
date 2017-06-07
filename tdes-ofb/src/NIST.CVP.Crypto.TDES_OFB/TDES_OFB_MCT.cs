using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_OFB
{
    public class TDES_OFB_MCT : ITDES_OFB_MCT
    {
        private readonly ITDES_OFB _algo;
        private readonly IMonteCarloKeyMaker _keyMaker;

        private const int NUMBER_OF_CASES = 400;
        private const int NUMBER_OF_ITERATIONS = 10000;
        private const int NUMBER_OF_OUTPUTS_TO_SAVE = 3;

        protected virtual int NumberOfCases { get { return NUMBER_OF_CASES; } }

        public TDES_OFB_MCT(ITDES_OFB algo, IMonteCarloKeyMaker keyMaker)
        {
            _algo = algo;
            _keyMaker = keyMaker;
        }

        public MCTResult<AlgoArrayResponse> MCTEncrypt(BitString keyBits, BitString data, BitString iv)
        {


            /*
            TMOVS:
            Initialize KEY1.0, KEY2.0, KEY3.0, IV, TEXT-0
            Send KEY1.0, KEY2.0, KEY3.0, IV, TEXT.0


            IUT:
            FOR i = 0 TO 399
            {
                (Part of Triple DES processing):
                    If (i==0) 
                    {
                        I.0 = IV
                    }

                Record i, KEY1.i, KEY2.i, KEY3.i, TEXT.0
                INITTEXT = TEXT.0
                FOR j = 0 TO 9999
                {
                    Perform Triple DES:
                        I.j is read into TDEA and is encrypted by DEA.1 using KEY1.i, 
                        resulting in TEMP1

                        TEMP1 is decrypted by DEA.2 using KEY2.i, 
                        resulting in TEMP2

                        TEMP2 is encrypted by DEA.3 using KEY3.i, 
                        resulting in O.j

                        RESULT.j = O.j XOR TEXT.j

                    TEXT.j+1 = I.j

                    (Part of Triple DES processing):
                        I.j+1 = O.j
                }
                Record I-0, RESULT.j

                Send i, KEY1.i, KEY2.i, KEY3.i, I-0, TEXT-0, RESULT.j


                KEY1.i+1 = KEY1.i XOR RESULT.j

                IF (KEY1.i and KEY2.i are independent and KEY3.i= KEY1.i) 
                or 
                (KEY1.i, KEY2.i, and KEY3.i are independent)
                {
                    KEY2.i+1 = KEY2.i XOR RESULT.j-1
                }
                ELSE
                {
                    KEY2.i+1 = KEY2.i XOR RESULT.j
                }

                IF (KEY1.i = KEY2.i = KEY3.i) 
                or 
                (KEY1.i and KEY2.i are independent and KEY3.i = KEY1.i)
                {
                    KEY3.i+1 = KEY3.i XOR RESULT.j
                }
                ELSE
                {
                    KEY3.i+1 = KEY3.i XOR RESULT.j - 2
                }

                TEXT.0 = INITTEXT XOR I.j
                I.0 = O.j
            }

            TMOVS:
            Check IUT's output for correctness.
             */



            throw new NotImplementedException();



        }

        public MCTResult<AlgoArrayResponse> MCTDecrypt(BitString keyBits, BitString data, BitString iv)
        {
            throw new NotImplementedException();
        }
    }
}

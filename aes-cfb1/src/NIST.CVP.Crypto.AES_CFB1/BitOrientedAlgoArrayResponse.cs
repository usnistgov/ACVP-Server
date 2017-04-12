using System.Collections.Generic;
using NIST.CVP.Crypto.AES;

namespace NIST.CVP.Crypto.AES_CFB1
{
    public class BitOrientedAlgoArrayResponse : AlgoArrayResponse
    {
        public new BitOrientedBitString PlainText { get; set; }
        public new BitOrientedBitString CipherText { get; set; }

        public static BitOrientedAlgoArrayResponse GetDerivedFromBase(AlgoArrayResponse original)
        {
            BitOrientedAlgoArrayResponse resp = new BitOrientedAlgoArrayResponse()
            {
                IV = original.IV,
                Key = original.Key,
                PlainText = BitOrientedBitString.GetDerivedFromBase(original.PlainText),
                CipherText = BitOrientedBitString.GetDerivedFromBase(original.CipherText)
            };

            return resp;
        }

        public static List<BitOrientedAlgoArrayResponse> GetDerivedFromBase(
            IEnumerable<AlgoArrayResponse> original)
        {
            List<BitOrientedAlgoArrayResponse> list = new List<BitOrientedAlgoArrayResponse>();

            foreach (var item in original)
            {
                list.Add(GetDerivedFromBase(item));
            }

            return list;
        }
    }
}

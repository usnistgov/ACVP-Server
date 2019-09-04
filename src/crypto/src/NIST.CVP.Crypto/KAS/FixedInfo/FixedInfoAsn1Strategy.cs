using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KAS.FixedInfo;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.FixedInfo
{
    public class FixedInfoAsn1Strategy : IFixedInfoStrategy
    {
        public BitString GetFixedInfo(Dictionary<string, BitString> fixedInfoParts)
        {
//            AsnEncodedDataCollection asnCollection = new AsnEncodedDataCollection();
//
//            foreach (var pair in fixedInfoParts)
//            {
//                asnCollection.Add(new AsnEncodedData(pair.Key, pair.Value.ToBytes()));
//            }
//            
//            

            

            throw new NotImplementedException();
        }
    }
}
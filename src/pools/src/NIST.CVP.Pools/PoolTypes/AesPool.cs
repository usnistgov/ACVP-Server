using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolTypes
{
    public class AesPool : Pool<AesParameters, AesResult>
    {
        public AesPool(AesParameters waterType, string filename, IList<JsonConverter> jsonConverters) : base(waterType, filename, jsonConverters) { }

        //public override bool PoolIsOfType(Parameters param)
        //{
        //    if (param is AesParameters potentialType)
        //    {
        //        return potentialType.DataLength == WaterType.DataLength &&
        //               potentialType.Direction == WaterType.Direction &&
        //               potentialType.KeyLength == WaterType.KeyLength &&
        //               potentialType.Mode == WaterType.Mode;
        //    }

        //    return false;
        //}
    }
}

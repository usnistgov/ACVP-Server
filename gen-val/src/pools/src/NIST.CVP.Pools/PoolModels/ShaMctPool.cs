﻿using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class ShaMctPool : PoolBase<ShaParameters, MctResult<HashResult>>
    {
        public ShaMctPool(PoolConstructionParameters<ShaParameters> param)
            : base(param) { }

        public override async Task RequestWater()
        {
            AddWater(await Oracle.GetShaMctCaseAsync(WaterType));
        }
    }
}
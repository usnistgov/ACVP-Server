using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.SafePrimes.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer
{
    public class TestGroup : ITestGroup<TestGroup, TestCase>
    {
        [JsonIgnore] public bool IsSample { get; }
        public int TestGroupId { get; set; }
        public string TestType { get; set; }
        public List<TestCase> Tests { get; set; } = new List<TestCase>();

        public SafePrime SafePrimeGroup { get; set; }

        public int DomainParameterL { get; private set; }
        public int DomainParameterN { get; private set; }

        private FfcDomainParameters _domainParameters;
        public FfcDomainParameters DomainParameters
        {
            get => _domainParameters;
            set
            {
                _domainParameters = value;

                DomainParameterL = _domainParameters.P.ExactBitString().PadToModulusMsb(32).BitLength;
                DomainParameterN = _domainParameters.Q.ExactBitString().PadToModulusMsb(32).BitLength;
            }
        }
    }
}

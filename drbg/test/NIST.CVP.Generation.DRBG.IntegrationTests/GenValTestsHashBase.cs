using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.DRBG.IntegrationTests
{
    public abstract class GenValTestsHashBase : GenValTestsDrbgBase
    {
        public abstract override string Algorithm { get; }
        public abstract override string Mode { get; }
        public abstract override int DataLength { get; }

        protected override string GetTestFileMinimalTestCases(string targetFolder)
        {
            return GetTestFileFewTestCases(targetFolder);
        }

        protected override string GetTestFileFewTestCases(string targetFolder)
        {
            MathDomain nonceLen = new MathDomain();
            nonceLen.AddSegment(new ValueDomainSegment(DataLength));

            MathDomain additionalInputLen = new MathDomain();
            additionalInputLen.AddSegment(new RangeDomainSegment(new Random800_90(), DataLength, DataLength + 256, 64));

            MathDomain persoStringLen = new MathDomain();
            persoStringLen.AddSegment(new ValueDomainSegment(DataLength));

            MathDomain entropyInputLen = new MathDomain();
            entropyInputLen.AddSegment(new ValueDomainSegment(DataLength));

            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                NonceLen = nonceLen,
                AdditionalInputLen = additionalInputLen,
                PersoStringLen = persoStringLen,
                EntropyInputLen = entropyInputLen,
                ReturnedBitsLen = DataLength * 4,
                ReseedImplemented = false,
                PredResistanceEnabled = false
            };

            return CreateRegistration(targetFolder, p);
        }

        protected override string GetTestFileLotsOfTestCases(string targetFolder)
        {
            MathDomain nonceLen = new MathDomain();
            nonceLen.AddSegment(new ValueDomainSegment(DataLength));

            MathDomain additionalInputLen = new MathDomain();
            additionalInputLen.AddSegment(new RangeDomainSegment(new Random800_90(), DataLength, DataLength + 64, 64));
            additionalInputLen.AddSegment(new ValueDomainSegment(256));

            MathDomain persoStringLen = new MathDomain();
            persoStringLen.AddSegment(new ValueDomainSegment(DataLength));
            persoStringLen.AddSegment(new RangeDomainSegment(new Random800_90(), 256, 512, 128));

            MathDomain entropyInputLen = new MathDomain();
            entropyInputLen.AddSegment(new ValueDomainSegment(DataLength));

            Parameters p = new Parameters
            {
                Algorithm = Algorithm,
                Mode = Mode,
                NonceLen = nonceLen,
                AdditionalInputLen = additionalInputLen,
                PersoStringLen = persoStringLen,
                EntropyInputLen = entropyInputLen,
                ReturnedBitsLen = DataLength * 4,
                ReseedImplemented = true,
                PredResistanceEnabled = true
            };

            return CreateRegistration(targetFolder, p);
        }
    }
}

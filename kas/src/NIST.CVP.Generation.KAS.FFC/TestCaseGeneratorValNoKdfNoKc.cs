using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.FFC.Helpers;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseGeneratorValNoKdfNoKc : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IKasBuilder<FfcParameterSet, FfcScheme, FfcSharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _kasBuilder;
        private readonly ISchemeBuilder<FfcParameterSet, FfcScheme, FfcSharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _schemeBuilder;
        private readonly IDsaFfcFactory _dsaFactory;
        private readonly TestCaseDispositionOption _intendedDisposition;

        public TestCaseGeneratorValNoKdfNoKc(
            IKasBuilder<FfcParameterSet, FfcScheme, FfcSharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> kasBuilder, 
            ISchemeBuilder<FfcParameterSet, FfcScheme, FfcSharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> schemeBuilder, 
            IDsaFfcFactory dsaFactory, 
            TestCaseDispositionOption intendedDisposition
        )
        {
            _kasBuilder = kasBuilder;
            _schemeBuilder = schemeBuilder;
            _dsaFactory = dsaFactory;
            _intendedDisposition = intendedDisposition;

            // This shouldn't happen, but just in case, NoKdfNoKc doesn't use DKM, MacData, or OI
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedDkm
                || _intendedDisposition == TestCaseDispositionOption.FailChangedMacData
                || _intendedDisposition == TestCaseDispositionOption.FailChangedOi)
            {
                _intendedDisposition = TestCaseDispositionOption.Success;
            }
        }

        public int NumberOfTestCasesToGenerate => 25;
        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            TestCase tc = new TestCase {TestCaseDisposition = _intendedDisposition};

            return Generate(group, tc);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            var dsa = _dsaFactory.GetInstance(group.HashAlg);

            var uParty = _kasBuilder
                .WithPartyId(
                    group.KasRole == KeyAgreementRole.InitiatorPartyU
                        ? SpecificationMapping.IutId
                        : SpecificationMapping.ServerId
                )
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithParameterSet(group.ParmSet)
                .WithScheme(group.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithDsaFactory(_dsaFactory)
                        .WithHashFunction(group.HashAlg)
                )
                .BuildNoKdfNoKc()
                .Build();

            var vParty = _kasBuilder
                .WithPartyId(
                    group.KasRole == KeyAgreementRole.ResponderPartyV
                        ? SpecificationMapping.IutId
                        : SpecificationMapping.ServerId
                )
                .WithKeyAgreementRole(KeyAgreementRole.ResponderPartyV)
                .WithParameterSet(group.ParmSet)
                .WithScheme(group.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithDsaFactory(_dsaFactory)
                        .WithHashFunction(group.HashAlg)
                )
                .BuildNoKdfNoKc()
                .Build();

            FfcDomainParameters dp = new FfcDomainParameters(
                group.P,
                group.Q,
                group.G
            );

            uParty.SetDomainParameters(dp);
            vParty.SetDomainParameters(dp);

            var uPartyPublic = uParty.ReturnPublicInfoThisParty();
            var vPartyPublic = vParty.ReturnPublicInfoThisParty();

            var serverKas = group.KasRole == KeyAgreementRole.InitiatorPartyU ? vParty : uParty;
            var iutKas = group.KasRole == KeyAgreementRole.InitiatorPartyU ? uParty : vParty;

            // Mangle the keys prior to running compute result, to create a "successful" result on bad keys.
            // IUT should pick up on bad private/public key information.
            TestCaseDispositionHelper.MangleKeys(
                testCase,
                _intendedDisposition,
                serverKas,
                iutKas
            );

            // Use the IUT kas for compute result
            KasResult iutResult = null;
            if (serverKas == uParty)
            {
                iutResult = vParty.ComputeResult(uPartyPublic);
            }
            else
            {
                iutResult = uParty.ComputeResult(vPartyPublic);
            }

            // Set the test case up w/ the information from the kas instances
            TestCaseDispositionHelper.SetTestCaseInformationFromKasResults(group, testCase, serverKas, iutKas, iutResult);
            
            // introduce errors into other data
            if (_intendedDisposition == TestCaseDispositionOption.FailChangedZ)
            {
                testCase.FailureTest = true;

                // Change the Z
                testCase.Z[0] += 2;

                // Rehash Z
                testCase.Tag = dsa.Sha.HashMessage(testCase.Z).Digest;
            }

            if (_intendedDisposition == TestCaseDispositionOption.FailChangedTag)
            {
                testCase.FailureTest = true;
                testCase.HashZ[0] += 2;
            }

            // check for successful conditions w/ constraints.
            if (_intendedDisposition == TestCaseDispositionOption.SuccessLeadingZeroNibbleZ)
            {
                // No zero nibble in MSB
                if (testCase.Z[0] >= 0x10)
                {
                    // call generate again, until getting to a zero nibble MSB for Z
                    Generate(group, testCase);
                }
            }

            return new TestCaseGenerateResponse(testCase);
        }
    }
}
using System.Collections.Generic;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.FFC.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseValidatorAftKdfNoKc : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _testGroup;
        private readonly IKasBuilder _kasBuilder;
        private readonly IMacParametersBuilder _macParametersBuilder;
        private readonly ISchemeBuilder _schemeBuilder;
        private readonly IEntropyProviderFactory _entropyProviderFactory;
        private (FfcScheme scheme, KeyAgreementRole thisPartyKasRole, bool generatesStaticKeyPair, bool generatesEphemeralKeyPair) _iutKeyRequirements;
        private (FfcScheme scheme, KeyAgreementRole thisPartyKasRole, bool generatesStaticKeyPair, bool generatesEphemeralKeyPair) _serverKeyRequirements;

        public TestCaseValidatorAftKdfNoKc(TestCase expectedResult, TestGroup testGroup, IKasBuilder kasBuilder, IMacParametersBuilder macParametersBuilder, ISchemeBuilder schemeBuilder, IEntropyProviderFactory entropyProviderFactory)
        {
            _expectedResult = expectedResult;
            _testGroup = testGroup;
            _kasBuilder = kasBuilder;
            _macParametersBuilder = macParametersBuilder;
            _schemeBuilder = schemeBuilder;
            _entropyProviderFactory = entropyProviderFactory;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();

            _iutKeyRequirements =
                SpecificationMapping.GetKeyGenerationOptionsForSchemeAndRole(_testGroup.Scheme, _testGroup.KasRole);
            _serverKeyRequirements =
                SpecificationMapping.GetKeyGenerationOptionsForSchemeAndRole(_testGroup.Scheme,
                    _testGroup.KasRole == KeyAgreementRole.InitiatorPartyU
                        ? KeyAgreementRole.ResponderPartyV
                        : KeyAgreementRole.InitiatorPartyU);

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join("; ", errors) };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (_iutKeyRequirements.generatesStaticKeyPair)
            {
                if (suppliedResult.StaticPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.StaticPublicKeyIut)} but was not supplied");
                }
            }

            if (_iutKeyRequirements.generatesEphemeralKeyPair)
            {
                if (suppliedResult.EphemeralPublicKeyIut == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.EphemeralPublicKeyIut)} but was not supplied");
                }
            }

            // AES-CCM nonce required only when IUT is both initiator, and macType is AES-CCM
            if (_testGroup.KasRole == KeyAgreementRole.InitiatorPartyU
                && _testGroup.MacType == KeyAgreementMacType.AesCcm)
            {
                if (suppliedResult.NonceAesCcm == null || suppliedResult.NonceAesCcm.BitLength == 0)
                {
                    errors.Add($"Expected {nameof(suppliedResult.NonceAesCcm)} but was not supplied");
                }
            }

            if (suppliedResult.IdIutLen == 0)
            {
                errors.Add($"Expected {nameof(suppliedResult.IdIutLen)} must be supplied and non zero");
            }

            if (suppliedResult.IdIut == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.Tag)} but was not supplied");
            }

            if (suppliedResult.OiLen == 0)
            {
                errors.Add($"Expected {nameof(suppliedResult.OiLen)} must be supplied and non zero");
            }

            if (suppliedResult.OtherInfo == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.OtherInfo)} but was not supplied");
            }

            if (suppliedResult.Dkm == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.Dkm)} but was not supplied");
            }

            if (suppliedResult.Tag == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.Tag)} but was not supplied");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            KasResult serverResult = PerformKas(suppliedResult);

            if (!serverResult.Tag.Equals(suppliedResult.Dkm))
            {
                errors.Add($"{nameof(suppliedResult.Dkm)} does not match");
            }

            if (!serverResult.Tag.Equals(suppliedResult.Tag))
            {
                errors.Add($"{nameof(suppliedResult.Tag)} does not match");
            }
        }

        // TODO extract algo from class
        private KasResult PerformKas(TestCase suppliedResult)
        {
            FfcDomainParameters domainParameters = new FfcDomainParameters(_testGroup.P, _testGroup.Q, _testGroup.G);
            FfcSharedInformation iutPublicInfo = new FfcSharedInformation(
                domainParameters,
                suppliedResult.IdIut,
                suppliedResult.StaticPublicKeyServer,
                suppliedResult.EphemeralPublicKeyServer,
                null,
                null,
                _expectedResult.NonceNoKc
            );

            var macParameters = _macParametersBuilder
                .WithKeyAgreementMacType(_testGroup.MacType)
                .WithMacLength(_testGroup.MacLen)
                .WithNonce(suppliedResult.NonceAesCcm)
                .Build();

            KeyAgreementRole serverRole = _testGroup.KasRole == KeyAgreementRole.InitiatorPartyU
                ? KeyAgreementRole.ResponderPartyV
                : KeyAgreementRole.InitiatorPartyU;

            // inject specific entropy for nonceNoKc when the server is the initiator
            // when the server is not the initiator, nonceNoKc is provided via the other party's public info
            if (serverRole == KeyAgreementRole.InitiatorPartyU)
            {
                var entropyProvider = _entropyProviderFactory
                    .GetEntropyProvider(EntropyProviderTypes.Testable);
                entropyProvider.AddEntropy(_expectedResult.NonceNoKc);
                    
                _schemeBuilder.WithEntropyProvider(entropyProvider);
            }

            var serverKas = _kasBuilder
                .WithKeyAgreementRole(
                    _serverKeyRequirements.thisPartyKasRole
                )
                .WithParameterSet(_testGroup.ParmSet)
                .WithScheme(_testGroup.Scheme)
                .WithSchemeBuilder(
                    _schemeBuilder
                        .WithOtherInfoFactory(new FakeOtherInfoFactory(suppliedResult.OtherInfo))
                )
                .WithKeyAgreementRole(serverRole)
                .WithPartyId(_testGroup.IdServer)
                .BuildKdfNoKc()
                .WithKeyLength(_testGroup.KeyLen)
                .WithMacParameters(macParameters)
                .WithOtherInfoPattern(_testGroup.OiPattern)
                .Build();

            serverKas.SetDomainParameters(domainParameters);
            serverKas.ReturnPublicInfoThisParty();

            if (_serverKeyRequirements.generatesStaticKeyPair)
            {
                serverKas.Scheme.StaticKeyPair.PrivateKeyX = _expectedResult.StaticPrivateKeyServer;
                serverKas.Scheme.StaticKeyPair.PublicKeyY = _expectedResult.StaticPublicKeyServer;
            }

            if (_serverKeyRequirements.generatesEphemeralKeyPair)
            {
                serverKas.Scheme.EphemeralKeyPair.PrivateKeyX = _expectedResult.EphemeralPrivateKeyServer;
                serverKas.Scheme.EphemeralKeyPair.PublicKeyY = _expectedResult.EphemeralPublicKeyServer;
            }

            var serverResult = serverKas.ComputeResult(iutPublicInfo);
            return serverResult;
        }
    }
}
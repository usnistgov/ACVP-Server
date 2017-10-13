using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC
{
    public class TestCaseValidatorAftNoKdfNoKc : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        private readonly TestGroup _testGroup;
        private readonly IKasBuilder _kasBuilder;

        private (FfcScheme scheme, KeyAgreementRole thisPartyKasRole, bool generatesStaticKeyPair, bool
            generatesEphemeralKeyPair) _iutKeyRequirements;
        private (FfcScheme scheme, KeyAgreementRole thisPartyKasRole, bool generatesStaticKeyPair, bool
            generatesEphemeralKeyPair) _serverKeyRequirements;

        public TestCaseValidatorAftNoKdfNoKc(TestCase expectedResult, TestGroup testGroup, IKasBuilder kasBuilder)
        {
            _expectedResult = expectedResult;
            _testGroup = testGroup;
            _kasBuilder = kasBuilder;
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

            if (suppliedResult.HashZ == null)
            {
                errors.Add($"Expected {nameof(suppliedResult.HashZ)} but was not supplied");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            KasResult serverResult = PerformKas(suppliedResult);

            if (!serverResult.Tag.Equals(suppliedResult.HashZ))
            {
                errors.Add($"{nameof(suppliedResult.HashZ)} does not match");
            }
        }

        // TODO extract algo from class
        private KasResult PerformKas(TestCase suppliedResult)
        {
            FfcDomainParameters domainParameters = new FfcDomainParameters(_testGroup.P, _testGroup.Q, _testGroup.G);
            FfcSharedInformation iutPublicInfo = new FfcSharedInformation(
                domainParameters,
                null,
                suppliedResult.StaticPublicKeyServer,
                suppliedResult.EphemeralPublicKeyServer,
                null,
                null,
                null
            );

            var serverKas = _kasBuilder
                .WithKeyAgreementRole(
                    _serverKeyRequirements.thisPartyKasRole
                )
                .WithParameterSet(_testGroup.ParmSet)
                .WithScheme(_testGroup.Scheme)
                .BuildNoKdfNoKc()
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
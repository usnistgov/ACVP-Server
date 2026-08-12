using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.v1_0.SigVer;

/// <summary>
/// A negative pre-hash case cannot test which pre-hash function the verifier used, since it is expected
/// to fail whichever function it names. Only a valid case can. These tests show that a pre-hash group
/// now carries one valid case per registered hash function, and that pairing valid cases with the
/// coverage queue gives every function a valid case.
/// </summary>
[TestFixture, UnitTest]
public class PreHashCoverageTests
{
    private static readonly HashFunctions[] AllHashFunctions =
    {
        HashFunctions.Sha2_d224, HashFunctions.Sha2_d256, HashFunctions.Sha2_d384, HashFunctions.Sha2_d512,
        HashFunctions.Sha2_d512t224, HashFunctions.Sha2_d512t256,
        HashFunctions.Sha3_d224, HashFunctions.Sha3_d256, HashFunctions.Sha3_d384, HashFunctions.Sha3_d512,
        HashFunctions.Shake_d128, HashFunctions.Shake_d256
    };

    [Test]
    public void PreHashProviderHasOneValidCasePerHashFunction()
    {
        var provider = new SignatureExpectationProvider(AllHashFunctions.Length);

        var count = provider.ExpectationCount;
        var reasons = new List<MLDSASignatureDisposition>();
        for (var i = 0; i < count; i++)
        {
            reasons.Add(provider.GetRandomReason());
        }

        var valid = reasons.Count(r => r == MLDSASignatureDisposition.None);
        Assert.That(valid, Is.EqualTo(AllHashFunctions.Length), "one valid case per hash function");
    }

    [Test]
    public void ValidCasesPairedWithCoverageQueueCoverEveryHashFunction()
    {
        // Reproduces the generator's pairing: the provider gives one None per hash function, and each
        // None draws from a shuffled queue of all functions, so every function gets a valid case.
        var provider = new SignatureExpectationProvider(AllHashFunctions.Length);
        var coverage = new ShuffleQueue<HashFunctions>(AllHashFunctions.ToList());

        var count = provider.ExpectationCount;
        var covered = new HashSet<HashFunctions>();
        for (var i = 0; i < count; i++)
        {
            if (provider.GetRandomReason() == MLDSASignatureDisposition.None)
            {
                covered.Add(coverage.Pop());
            }
        }

        Assert.That(covered, Is.EquivalentTo(AllHashFunctions), "every hash function has a valid case");
    }

    [Test]
    public void NonPreHashProviderIsUnchanged()
    {
        var provider = new SignatureExpectationProvider();

        var count = provider.ExpectationCount;
        var reasons = new List<MLDSASignatureDisposition>();
        for (var i = 0; i < count; i++)
        {
            reasons.Add(provider.GetRandomReason());
        }

        Assert.That(reasons.Count, Is.EqualTo(15), "default group is still 15 cases");
        Assert.That(reasons.Count(r => r == MLDSASignatureDisposition.None), Is.EqualTo(3), "default group has 3 valid cases");
    }
}

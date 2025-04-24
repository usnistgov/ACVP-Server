using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NLog;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestCaseGeneratorDecrypt : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 60;

    ShuffleQueue<int> plaintextLengths, ADLengths, truncationLengths;

    public TestCaseGeneratorDecrypt(IOracle oracle)
    {
        _oracle = oracle;
    }

    public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
    {
        List<int> plengths = new List<int>();
        List<int> adlengths = new List<int>();
        List<int> trunclengths = new List<int>();

        plengths.AddRange(group.PlaintextLength.GetDomainMinMaxAsEnumerable());
        adlengths.AddRange(group.ADLength.GetDomainMinMaxAsEnumerable());
        trunclengths.AddRange(group.TruncationLength.GetDomainMinMaxAsEnumerable());
        for (int i = 0; i < 8; i++)
        {
            plengths.AddRange(group.PlaintextLength.GetRandomValues(x => x % 8 == i, 5));
            adlengths.AddRange(group.ADLength.GetRandomValues(x => x % 8 == i, 5));
        }
        //Testing breakpoints and surrounding values for chunk sizes
        for (int i = 3; i < 8; i++)
        {
            plengths.AddRange(group.PlaintextLength.GetSequentialValues((i << i) - 1, (i << i) + 1, 3));
            adlengths.AddRange(group.ADLength.GetSequentialValues((i << i) - 1, (i << i) + 1, 3));
        }
        plaintextLengths = new ShuffleQueue<int>(plengths);
        ADLengths = new ShuffleQueue<int>(adlengths);
        truncationLengths = new ShuffleQueue<int>(trunclengths);

        return new GenerateResponse();
    }

    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new AsconAEAD128Parameters
        {
            PayloadBitLength = plaintextLengths.Pop(),
            ADBitLength = ADLengths.Pop(),
            NonceMasking = group.NonceMasking,
            TruncationLength = truncationLengths.Pop(),
        };

        try
        {
            var result = await _oracle.GetAsconDecryptCaseAsync(param);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Ciphertext = result.Ciphertext,
                Tag = result.Tag,
                Key = result.Key,
                AD = result.AD,
                Nonce = result.Nonce,
                Plaintext = result.Plaintext,
                SecondKey = result.SecondKey,
                PayloadBitLength = param.PayloadBitLength,
                ADBitLength = param.ADBitLength,
                TagLength = param.TruncationLength
            });
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error performing decryption: {ex.Message}");
        }
    }

    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}

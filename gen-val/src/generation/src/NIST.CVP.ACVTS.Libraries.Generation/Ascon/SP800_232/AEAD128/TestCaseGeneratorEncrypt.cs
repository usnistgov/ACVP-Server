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

public class TestCaseGeneratorEncrypt : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 60;

    ShuffleQueue<int> _plaintextLengths, _adLengths, _truncationLengths;

    public TestCaseGeneratorEncrypt(IOracle oracle)
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
        
        //T esting breakpoints and surrounding values for chunk sizes
        for (int i = 3; i < 8; i++)
        {
            plengths.AddRange(group.PlaintextLength.GetSequentialValuesInIncrement((1 << i) - 1, 3));
            adlengths.AddRange(group.ADLength.GetSequentialValuesInIncrement((1 << i) - 1, 3));
        }
        
        _plaintextLengths = new ShuffleQueue<int>(plengths);
        _adLengths = new ShuffleQueue<int>(adlengths);
        _truncationLengths = new ShuffleQueue<int>(trunclengths);

        return new GenerateResponse();
    }

    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new AsconAEAD128Parameters
        {
            PayloadBitLength = _plaintextLengths.Pop(),
            ADBitLength = _adLengths.Pop(),
            NonceMasking = group.NonceMasking,
            TruncationLength = _truncationLengths.Pop(),
        };

        try
        {
            var result = await _oracle.GetAsconEncryptCaseAsync(param);

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
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error performing encryption: {ex.Message}");
        }
    }

    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}

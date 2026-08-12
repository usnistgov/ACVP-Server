using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;

public class TestCaseGeneratorAft : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
    private ShuffleQueue<int> _messageLengths;
    private ShuffleQueue<int> _idx;

    public int NumberOfTestCasesToGenerate => 4;

    public TestCaseGeneratorAft(IOracle oracle)
    {
        _oracle = oracle;
    }

    public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
    {
        var messageLengths = new List<int>();
        messageLengths.AddRange(group.MessageLength.GetDomainMinMaxAsEnumerable());
        messageLengths.AddRange(group.MessageLength.GetRandomValues(_ => true, NumberOfTestCasesToGenerate - 2));

        _messageLengths = new ShuffleQueue<int>(messageLengths);
        _idx = new ShuffleQueue<int>(Enumerable.Range(0, 32).ToList());

        return new GenerateResponse();
    }

    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        try
        {
            var param = new XmssSignatureParameters
            {
                Disposition = group.TestCaseExpectationProvider.GetRandomReason(),
                XmssMode = group.XmssMode,
                MessageLength = _messageLengths.Pop(),
                Idx = _idx.Pop(),
                XmssKeyPair = group.KeyPair
            };

            var result = await _oracle.GetXmssVerifyResultAsync(param);

            var testCase = new TestCase
            {
                Message = result.VerifiedValue.Message,
                MessageLength = param.MessageLength,
                Reason = param.Disposition,
                TestPassed = result.Result,
                Signature = result.VerifiedValue.Signature
            };

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating case: {ex.Message}");
        }
    }

    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}

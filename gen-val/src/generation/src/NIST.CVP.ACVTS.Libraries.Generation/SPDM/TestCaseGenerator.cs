using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NLog;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.SPDM;

public class TestCaseGenerator : ITestCaseGeneratorWithPrep<TestGroup, TestCase>
{
    private readonly IOracle _oracle;

    public int NumberOfTestCasesToGenerate => 20;

    ShuffleQueue<int> _keyLengths;
    ShuffleQueue<int> _thLengths;

    public TestCaseGenerator(IOracle oracle)
    {
        _oracle = oracle;
    }

    public GenerateResponse PrepareGenerator(TestGroup group, bool isSample)
    {
        List<int> klengths = [];
        List<int> thlengths = [];

        klengths.AddRange(group.KeyLength.GetDomainMinMaxAsEnumerable());
        klengths.AddRange(group.KeyLength.GetRandomValues(x => x % 8 == 0, NumberOfTestCasesToGenerate - 2));
        _keyLengths = new ShuffleQueue<int>(klengths);

        thlengths.AddRange(group.THLength.GetDomainMinMaxAsEnumerable());
        thlengths.AddRange(group.THLength.GetRandomValues(x => x % 8 == 0, NumberOfTestCasesToGenerate - 2));
        _thLengths = new ShuffleQueue<int>(thlengths);

        return new GenerateResponse();
    }

    public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = -1)
    {
        var param = new SPDMParameters
        {
            KeyLength = _keyLengths.Pop(),
            THLength = _thLengths.Pop(),
            Mode = group.HashFunction,
            Version = group.Version,
            PSK = group.UsesPSK,
        };

        try
        {
            var result = await _oracle.GetSpdmCaseAsync(param);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase
            {
                Key = result.Key,
                RequestHandshakeSecret = result.RequestDirectionHandshake,
                ResponseHandshakeSecret = result.ResponseDirectionHandshake,
                RequestDataSecret = result.RequestDirectionData,
                ResponseDataSecret = result.ResponseDirectionData,
                ExportMasterSecret = result.ExportMaster,
                TH1 = result.TH1,
                TH2 = result.TH2
            });
        }
        catch (Exception ex)
        {
            ThisLogger.Error(ex);
            return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error performing SPDM key schedule: {ex.Message}");
        }
    }

    private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
}

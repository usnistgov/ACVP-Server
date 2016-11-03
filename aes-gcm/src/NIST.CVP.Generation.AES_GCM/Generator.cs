using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Generation.AES_GCM.Resolvers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    public class Generator
    {
        public const int NUMBER_OF_CASES = 15; //@@@Make configurable
        private readonly ITestVectorFactory _testVectorFactory;
        private readonly IRandom800_90 _random800_90;
        private readonly IParameterParser _parameterParser;
        private readonly IParameterValidator _parameterValidator;
        private readonly IAES_GCM _aesGcm;
        public readonly List<JsonOutputDetail> JsonOutputs = new List<JsonOutputDetail>
        {
            new JsonOutputDetail { OutputFileName = "answer.json", Resolver = new AnswerResolver()},
            new JsonOutputDetail { OutputFileName = "prompt.json", Resolver = new PromptResolver()},
            new JsonOutputDetail { OutputFileName = "testResults.json", Resolver = new ResultResolver()},
        };

        public Generator(ITestVectorFactory testVectorFactory, IRandom800_90 random800_90, IParameterParser parameterParser, IParameterValidator parameterValidator)
        {
            _testVectorFactory = testVectorFactory;
            _random800_90 = random800_90;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
        }

        public GenerateResponse Generate(string requestFilePath)
        {
            var parameterResponse = _parameterParser.Parse(requestFilePath);
            if (!parameterResponse.Success)
            {
                return new GenerateResponse(parameterResponse.ErrorMessage);
            }
            var parameters = parameterResponse.ParsedObject;
            var validateResponse = _parameterValidator.Validate(parameters);
            if (!validateResponse.Success)
            {
                return new GenerateResponse(validateResponse.ErrorMessage);
            }
            var testVector = _testVectorFactory.BuildTestVectorSet(parameters);
            int testId = 1;
            foreach (var direction in parameters.Mode)
            {
                var generator = GetCaseGenerator(direction, parameters.ivGen);
                foreach (var group in testVector.TestGroups.Select(g => (TestGroup)g))
                {
                    for (int caseNo = 0; caseNo < NUMBER_OF_CASES; ++caseNo)
                    {
                        // @@@ TODO should some of these values be generated within the generator itself?
                        // some of the values generated here are not needed for decrypt, 
                        // and additional values are needed for decrypt.
                        // Would also allow for the simplification/consistency of the interface ITestCaseGenerator if *all*
                        // random values are generated within the generator
                        var key = _random800_90.GetRandomBitString(group.KeyLength);
                        var plainText = _random800_90.GetRandomBitString(group.PTLength);
                        var aad = _random800_90.GetRandomBitString(group.AADLength);
                        var testCaseResponse = generator.Generate(group, key, plainText, aad);
                        if (!testCaseResponse.Success)
                        {
                            return new GenerateResponse(testCaseResponse.ErrorMessage);
                        }
                        var testCase = (TestCase)testCaseResponse.TestCase;
                        testCase.TestCaseId = testId;
                        group.Tests.Add(testCase);
                        testId++;
                    }
                }   
            }
            var outputDirPath = Path.GetDirectoryName(requestFilePath);
            foreach (var jsonOutput in JsonOutputs)
            {
                var saveResult = SaveProjectionToFile(outputDirPath, testVector, jsonOutput);
                if (!string.IsNullOrEmpty(saveResult))
                {
                    return  new GenerateResponse(saveResult);
                }
            }

            return new GenerateResponse();
        }
        private string SaveProjectionToFile(string requestFilePath, ITestVectorSet testVectorSet, JsonOutputDetail jsonOutput)
        {
            //serialize to file
            var json = JsonConvert.SerializeObject(testVectorSet, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = jsonOutput.Resolver,
                    Converters = new List<JsonConverter> { new BitstringConverter() },
                    NullValueHandling = NullValueHandling.Ignore
                });
            return SaveToFile(Path.GetDirectoryName(requestFilePath), jsonOutput.OutputFileName, json);
            
        }

        private string SaveToFile(string fileRoot, string fileName, string json)
        {
            string path = Path.Combine(fileRoot, fileName);
            try
            {
                File.WriteAllText(path, json);
                return null;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return $"Could not create {path}";
            }
        }

        public ITestCaseGenerator GetCaseGenerator(string direction, string ivGen)
        {
            if (direction == "encrypt")
            {
                if (ivGen == "internal")
                {
                    return new TestCaseGeneratorInternalEncrypt(_random800_90, _aesGcm);
                }
                if (ivGen == "external")
                {
                    return new TestCaseGeneratorExternalEncrypt(_random800_90, _aesGcm);
                }
            }

            if (direction == "decrypt")
            {
                throw new NotImplementedException("Haven't added decryption yet");
                if (ivGen == "internal" || ivGen == "external")
                {
                    // for decryption vectors, IV must be known in order to generate a cipher for decryption.
                    // internal/external IV not relevant for decryption.
                    return new TestCaseGeneratorInternalDecrypt(_random800_90, _aesGcm);
                }
            }
            return new TestCaseGeneratorNull();
        }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}

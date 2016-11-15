using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.AES_GCM
{
    public class Validator
    {
        private readonly IDynamicParser _dynamicParser;
        private readonly IResultValidator _resultValidator;
        private readonly ITestCaseGeneratorFactory _testCaseGeneratorFactory;
       

        public Validator(IDynamicParser dynamicParser, IResultValidator resultValidator, ITestCaseGeneratorFactory testCaseGeneratorFactory)
        {
            _dynamicParser = dynamicParser;
            _resultValidator = resultValidator;
            _testCaseGeneratorFactory = testCaseGeneratorFactory;
        }

        public ValidateResponse Validate(string resultPath, string answerPath, string promptPath)
        {
            var answerParseResponse = _dynamicParser.Parse(answerPath);
            if (!answerParseResponse.Success)
            {
                return  new ValidateResponse(answerParseResponse.ErrorMessage);
            }
            var promptParseResponse = _dynamicParser.Parse(promptPath);
            if (!promptParseResponse.Success)
            {
                return new ValidateResponse(promptParseResponse.ErrorMessage);
            }

             var testResultParseResponse = _dynamicParser.Parse(resultPath);
            if (!testResultParseResponse.Success)
            {
                return new ValidateResponse(testResultParseResponse.ErrorMessage);
            }

            var testVectorSet = new TestVectorSet(answerParseResponse.ParsedObject, promptParseResponse.ParsedObject);
            var suppliedResults = GetTestCaseResults(testResultParseResponse.ParsedObject);
            var testCases = BuildValidatorList(testVectorSet, suppliedResults);
            var response = _resultValidator.ValidateResults(testCases, suppliedResults);
            
            var validationJson = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore });
            var saveResult = SaveToFile(Path.GetDirectoryName(resultPath), "validation.json", validationJson);
            if (!string.IsNullOrEmpty(saveResult))
            {
                return new ValidateResponse(saveResult);
            }

            return  new ValidateResponse();
        }

        private List<TestCase> GetTestCaseResults(dynamic results)
        {
            var list = new List<TestCase>();


            return list;
        }

      

        private List<ITestCaseValidator> BuildValidatorList(TestVectorSet testVectorSet, List<TestCase>  suppliedResults)
        {

            var list = new List<ITestCaseValidator>();
           
            foreach (var group in testVectorSet.TestGroups.Select(g => (TestGroup)g))
            {
                var generator = _testCaseGeneratorFactory.GetCaseGenerator(group.Function, group.IVGeneration);
                foreach (var test in group.Tests.Select(t => (TestCase)t))
                {
                    var workingTest = test;
                    if (test.Deferred)
                    {
                        //if we're waiting for additional input on the response...
                        var matchingResult = suppliedResults.FirstOrDefault(r => r.TestCaseId == test.TestCaseId);
                        var protoTest = new TestCase
                        {
                            AAD =  test.AAD,
                            Key = test.Key,
                            PlainText = test.PlainText,
                            CipherText = test.CipherText,
                            Tag = test.Tag,
                            IV =  matchingResult.IV
                        };
                        var genResult = generator.Generate(group, protoTest);
                        if (!genResult.Success)
                        {
                            throw new Exception($"Could not generate results. for testCase = {test.TestCaseId}");
                        }
                    }
                    if (group.Function == "encrypt")
                    {
                        list.Add(new TestCaseValidatorEncrypt(workingTest));
                    }
                    else
                    {
                        list.Add(new TestCaseValidatorDecrypt(workingTest));
                    }
                   
                }
            }

            return list;
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

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}

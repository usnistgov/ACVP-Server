﻿using System;
using System.Collections.Generic;
using System.IO;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Parsers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class Generator<TParameters, TTestVectorSet, TTestGroup, TTestCase> : IGenerator
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private readonly ITestVectorFactory<TParameters, TTestVectorSet, TTestGroup, TTestCase> _testVectorFactory;
        private readonly IParameterParser<TParameters> _parameterParser;
        private readonly IParameterValidator<TParameters> _parameterValidator;
        private readonly ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> _testCaseGeneratorFactoryFactory;
        private readonly IVectorSetSerializer<TTestVectorSet, TTestGroup, TTestCase> _vectorSetSerializer;

        public readonly List<JsonOutputDetail> JsonOutputs = 
            new List<JsonOutputDetail>
        {
            new JsonOutputDetail { FileName = "internalProjection.json", Projection = Projection.Server },
            new JsonOutputDetail { FileName = "prompt.json", Projection = Projection.Prompt },
            new JsonOutputDetail { FileName = "expectedResults.json", Projection = Projection.Result },
        };

        public Generator(
            ITestVectorFactory<TParameters, TTestVectorSet, TTestGroup, TTestCase> testVectorFactory, 
            IParameterParser<TParameters> parameterParser, 
            IParameterValidator<TParameters> parameterValidator, 
            ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> iTestCaseGeneratorFactoryFactory,
            IVectorSetSerializer<TTestVectorSet, TTestGroup, TTestCase> vectorSetSerializer
        )
        {
            _testVectorFactory = testVectorFactory;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
            _testCaseGeneratorFactoryFactory = iTestCaseGeneratorFactoryFactory;
            _vectorSetSerializer = vectorSetSerializer;
        }

        public virtual GenerateResponse Generate(GenerateRequest generateRequest)
        {
            try
            {
                var parameterResponse = _parameterParser.Parse(generateRequest.RegistrationJson);
                if (!parameterResponse.Success)
                {
                    return new GenerateResponse(parameterResponse.ErrorMessage, StatusCode.ParameterError);
                }
                var parameters = parameterResponse.ParsedObject;
                var validateResponse = _parameterValidator.Validate(parameters);
                if (!validateResponse.Success)
                {
                    return new GenerateResponse(validateResponse.ErrorMessage, StatusCode.ParameterValidationError);
                }
                var testVector = _testVectorFactory.BuildTestVectorSet(parameters);
                var testCasesResult = _testCaseGeneratorFactoryFactory.BuildTestCases(testVector);
                if (!testCasesResult.Success)
                {
                    return testCasesResult;
                }

                return CreateResponse(testVector);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new GenerateResponse("General exception. Contact service provider.", StatusCode.Exception);
            }
        }

        private GenerateResponse CreateResponse(TTestVectorSet testVector)
        {
            return new GenerateResponse(
                _vectorSetSerializer.Serialize(testVector, Projection.Server),
                _vectorSetSerializer.Serialize(testVector, Projection.Prompt),
                _vectorSetSerializer.Serialize(testVector, Projection.Result));
        }

        protected Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
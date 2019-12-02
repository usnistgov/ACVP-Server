using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.GenValApp.Helpers;
using NIST.CVP.Generation.GenValApp.Models;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.GenValApp.Tests
{
    [TestFixture, UnitTest]
    public class ErrorLoggerTests
    {
        [Test]
        [TestCase(StatusCode.Exception)]
        [TestCase(StatusCode.CommandLineError)]
        [TestCase(StatusCode.FileReadError)]
        [TestCase(StatusCode.FileSaveError)]
        [TestCase(StatusCode.GeneratorError)]
        [TestCase(StatusCode.ModeError)]
        [TestCase(StatusCode.ParameterError)]
        [TestCase(StatusCode.ParameterValidationError)]
        [TestCase(StatusCode.TestCaseGeneratorError)]
        [TestCase(StatusCode.TestCaseValidatorError)]
        [TestCase(StatusCode.ValidatorError)]
        public void ShouldCreateJsonFileWithError(StatusCode code)
        {
            var guid = Guid.NewGuid().ToString();
            var testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\LoggingTests\");
            var fullPath = Path.Combine(testPath, guid);
            Directory.CreateDirectory(fullPath);
            ErrorLogger.LogError(code, "test", "test-test", fullPath);

            var errorPath = Path.Combine(fullPath, "error.json");
            Assert.IsTrue(File.Exists(errorPath), "File must exist");

            var errorContent = File.ReadAllText(errorPath);
            var error = JsonConvert.DeserializeObject<Error>(errorContent);

            Assert.AreEqual(code, error.StatusCode, "Status codes must match");
        }
    }
}

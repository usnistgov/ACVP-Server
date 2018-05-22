using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB128.Tests.Parsers
{
    [TestFixture, UnitTest]
    public class ParameterParserTests
    {
        private const string _WORKING_DIRECTORY = @"C:\temp";
        private string _filename = $"parameterParsing{Guid.NewGuid().ToString()}";
        private string _file_extension = ".json";

        private string FullFile { get { return $"{_WORKING_DIRECTORY}\\{_filename}{_file_extension}"; } }

        [OneTimeSetUp]
        public void Setup()
        {
            if (!Directory.Exists(_WORKING_DIRECTORY))
            {
                Directory.CreateDirectory(_WORKING_DIRECTORY);
            }

            Parameters p = new Parameters()
            {
                Algorithm = "AES-CFB128",
                KeyLen = new[] { 128, 192, 256 },
                Direction = new[] { "encrypt" },
                IsSample = true
            };

            var json = JsonConvert.SerializeObject(p);

            File.WriteAllText(FullFile, json);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (File.Exists(FullFile))
            {
                File.Delete(FullFile);
            }
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            var subject = GetSubject();
            var result = subject.Parse($"C:\\{Guid.NewGuid()}\\request.json");
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = GetSubject();
            var result = subject.Parse(path);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldParseProperModeValuesIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            var parameters = result.ParsedObject;
            Assert.IsNotNull(parameters.Direction);
            Assert.AreEqual(1, parameters.Direction.Length);
            Assert.IsNotNull(parameters.Direction.First(v => v == "encrypt"));

        }

        [Test]
        public void ShouldParseProperAlgorithmValueIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            var parameters = result.ParsedObject;
            Assert.AreEqual("AES-CFB128", parameters.Algorithm);

        }

        [Test]
        public void ShouldParseProperKeyLengthValuesIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            ValidateArray(result.ParsedObject.KeyLen, new[] { 128, 192, 256 });
        }

        private void ValidateArray(int[] array, int[] expectedArray)
        {
            Assert.IsNotNull(array);
            Assert.AreEqual(expectedArray.Length, array.Length);
            foreach (var value in expectedArray)
            {
                Assert.IsNotNull(array.First(v => v == value));
            }

        }

        [Test]
        public void ShouldParseProperIsSampleValueIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            var parameters = result.ParsedObject;
            Assert.AreEqual(true, parameters.IsSample);
        }

        private ParameterParser<Parameters> GetSubject()
        {
            return new ParameterParser<Parameters>();
        }
    }
}

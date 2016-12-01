using Newtonsoft.Json;
using NIST.CVP.Generation.AES_GCM.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_GCM.Tests.Parsers
{
    [TestFixture]
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
                aadLen = new[] { 128, 136, 256, 264 },
                Algorithm = "AES-GCM",
                ivGen = "internal",
                ivGenMode = "8.2.1",
                ivLen = new[] { 96 },
                KeyLen = new[] { 128, 192, 256 },
                Mode = new[] { "encrypt" },
                PtLen = new[] { 0, 128, 136, 256, 264 },
                TagLen = new[] { 96, 128 },
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
            Assert.IsNotNull(parameters.Mode);
            Assert.AreEqual(1, parameters.Mode.Length);
            Assert.IsNotNull(parameters.Mode.First(v => v == "encrypt"));

        }

        [Test]
        public void ShouldParseProperAlgorithmValueIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            var parameters = result.ParsedObject;
            Assert.AreEqual("AES-GCM", parameters.Algorithm);

        }

        [Test]
        public void ShouldParseProperIvGenValueIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            var parameters = result.ParsedObject;
            Assert.AreEqual("internal", parameters.ivGen);
        }

        [Test]
        public void ShouldParseProperIvGenModeValueIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            var parameters = result.ParsedObject;
            Assert.AreEqual("8.2.1", parameters.ivGenMode);
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

        [Test]
        public void ShouldParseProperTagLengthValuesIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            ValidateArray(result.ParsedObject.TagLen, new[] { 96, 128 });
        }

        [Test]
        public void ShouldParseProperptLengthValuesIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            ValidateArray(result.ParsedObject.PtLen, new[] { 0, 128, 136, 256, 264 });
        }

        [Test]
        public void ShouldParseProperivLengthValuesIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            ValidateArray(result.ParsedObject.ivLen, new[] { 96 });
        }

        [Test]
        public void ShouldParseProperaadLengthValuesIntoParameters()
        {
            var subject = GetSubject();
            var result = subject.Parse(FullFile);

            Assume.That(result != null);
            Assume.That(result.ParsedObject != null);
            ValidateArray(result.ParsedObject.aadLen, new[] { 128, 136, 256, 264 });
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

        private ParameterParser GetSubject()
        {
            return new ParameterParser();
        }
    }
}

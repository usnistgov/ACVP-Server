using Newtonsoft.Json;
using NIST.CVP.Generation.AES_GCM;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AES_GCM.IntegrationTests
{
    [TestFixture]
    public class GeneratorTests
    {

        string _unitTestPath = Path.GetFullPath(@"..\..\TestFiles\");
        private string _targetFolder;

        [OneTimeSetUp]
        public void Setup()
        {
            _targetFolder = Path.Combine(_unitTestPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_targetFolder);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_targetFolder, true);
        }

        [Test]
        public void ShouldReturn1OnNoArgumentsSupplied()
        {
            var result = AES_GCM.Program.Main(new string[] { });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldReturn1OnInvalidFileName()
        {
            var result = AES_GCM.Program.Main(new string[] { $"{Guid.NewGuid()}.json" });

            Assert.AreEqual(1, result);
        }

        [Test]
        public void ShouldCreateTestVectors()
        {
            var fileName = CreateTestFile();

            AES_GCM.Program.Main(new string[] { fileName });

            Assert.IsTrue(File.Exists($"{_targetFolder}\\answer.json"), "answer");
            Assert.IsTrue(File.Exists($"{_targetFolder}\\prompt.json"), "prompt");
            Assert.IsTrue(File.Exists($"{_targetFolder}\\testResults.json"), "testResults");
        }

        private string CreateTestFile()
        {
            Parameters p = new Parameters()
            {
                Algorithm = "AES-GCM",
                //// @@@ TODO avoid decrypt for now until test vectors/projections implemented
                //Mode = new string[] { "encrypt" },
                Mode = ParameterValidator.VALID_DIRECTIONS,
                KeyLen = ParameterValidator.VALID_KEY_SIZES,
                PtLen = new int[] { 128 },
                ivLen = new int[] { 96 },
                ivGen = ParameterValidator.VALID_IV_GEN[0],
                ivGenMode = ParameterValidator.VALID_IV_GEN_MODE[0],
                aadLen = new int[] { 128 },
                TagLen = ParameterValidator.VALID_TAG_LENGTHS
            };

            var json = JsonConvert.SerializeObject(p);

            string fileName = $"{_targetFolder}\\registration.json";

            File.WriteAllText(fileName, json);

            return fileName;
        }
    }
}

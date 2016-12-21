using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture]
    public class AlgoFileFinderBaseTests
    {

        // TODO: Do actual file pathing for everything
        // TODO: Make sure tests reflect purpose of the class

        private string _testPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\algoFileFinderBaseTests");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        [TestCase(@"some/file/path/")]
        [TestCase(null)]
        [TestCase("")]
        public void ShouldCorrectlyHandleAllFilePrefixes(string filePrefix)
        {
            var subject = GetSubject(filePrefix);
            Assume.That(subject.Name == "FakeAlgo");
            Assert.IsTrue(subject.FilePrefix == filePrefix);
        }

        [Test]
        public void UseFilePrefixShouldBeTrueForNonNullOrNonEmptyFilePrefix()
        {
            var subject = GetSubject(@"some/file/path/");
            Assert.IsTrue(subject.UseFilePrefix);
        }

        [Test]
        public void ShouldNotCopyFilesIfSourceDirectoryDoesNotExist()
        {
            var subject = GetSubject(@"some/file/path/");
            var result = subject.CopyFoundFilesToTargetDirectory(@"file/path/that/does/not/exist/", "fileFolderName",
                "targetDirectory");
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ExtractFilesFromZipShouldBeTrueWhenValid()
        {
            var subject = GetSubject(null);
            var result = subject.ExtractFilesFromZip("filepath", "extractionpath");
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(@"source/does/not/exist/", "extractionpath")]
        [TestCase("sourcepath", @"destination/does/not/exist/")]
        [TestCase(@"source/does/not/exist/", @"destination/does/not/exist")]
        public void ExtractFilesFromZipShouldBeFalseWhenInvalid(string sourcePath, string destinationPath)
        {
            var subject = GetSubject(null);
            var result = subject.ExtractFilesFromZip(sourcePath, destinationPath);
            Assert.IsFalse(result);
        }
        
        private FakeAlgoFileFinderBase GetSubject(string filePrefix)
        {
            string path = Path.Combine(_testPath, filePrefix);
            return new FakeAlgoFileFinderBase("FakeAlgo", path);
        }
    }
}

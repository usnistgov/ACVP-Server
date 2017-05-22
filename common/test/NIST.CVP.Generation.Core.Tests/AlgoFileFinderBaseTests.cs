using System;
using System.IO;
using NIST.CVP.Generation.Core.Tests.Fakes;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class AlgoFileFinderBaseTests
    {

        string _unitTestPath;
        private string _targetFolder;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\FakeAlgoFinderFiles");
            _targetFolder = Path.Combine(_unitTestPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_targetFolder);
        }

        [Test]
        [TestCase("resp", 3, "FA")]
        [TestCase("fax", 2, "FA")]
        [TestCase("req", 1, "FA")]
        [TestCase("sample", 4, "FA")]
        [TestCase("resp", 6, "")]
        [TestCase("fax", 4, "")]
        [TestCase("req", 3, "")]
        [TestCase("sample", 8, "")]
        public void ShouldCopyProperFilesFromSuppliedSubFolder(string fileFolderName, int expected, string filePrefix)
        {
            var subject = new FakeAlgoFileFinderBase(filePrefix);
            string path = Path.Combine(_unitTestPath, "FakeAlgo");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 3, "FA")]
        [TestCase("fax", 2, "FA")]
        [TestCase("req", 1, "FA")]
        [TestCase("sample", 4, "FA")]
        [TestCase("resp", 6, "")]
        [TestCase("fax", 4, "")]
        [TestCase("req", 3, "")]
        [TestCase("sample", 8, "")]
        public void ShouldCopyProperFilesFromNestedSuppliedSubFolder(string fileFolderName, int expected, string filePrefix)
        {
            var subject = new FakeAlgoFileFinderBase(filePrefix);
            string path = Path.Combine(_unitTestPath, "SomeImplementation", "FakeAlgo");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 3, "FA")]
        [TestCase("fax", 2, "FA")]
        [TestCase("req", 1, "FA")]
        [TestCase("sample", 4, "FA")]
        [TestCase("resp", 6, "")]
        [TestCase("fax", 4, "")]
        [TestCase("req", 3, "")]
        [TestCase("sample", 8, "")]
        public void ShouldCopyFilesFromZipFileSuppliedSubFolder(string fileFolderName, int expected, string filePrefix)
        {
            var subject = new FakeAlgoFileFinderBase(filePrefix);
            string path = Path.Combine(_unitTestPath, "SomeLab");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonExistentSubFolder()
        {
            var subject = new FakeAlgoFileFinderBase("FA");
            string path = Path.Combine(_unitTestPath, "FakeAlgo");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "FolderDoesNotExist", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonPrefixFolder()
        {
            var subject = new FakeAlgoFileFinderBase("FA");
            string path = Path.Combine(_unitTestPath, "FA_FakeMode");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(0, result);
        }

        [Test]
        public void ShouldNotCopyWhenSourceDoesNotExist()
        {
            var subject = new FakeAlgoFileFinderBase("");
            string path = Path.Combine(_unitTestPath, "FolderDoesNotExist");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_targetFolder, true);
        }
    }
}

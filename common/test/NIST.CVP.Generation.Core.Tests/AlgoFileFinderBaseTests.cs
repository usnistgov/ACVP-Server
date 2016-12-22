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
        [TestCase("resp", 3)]
        [TestCase("fax", 2)]
        [TestCase("req", 1)]
        [TestCase("sample", 4)]
        public void ShouldCopyProperFilesFromSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new FakeAlgoFileFinderBase();
            string path = Path.Combine(_unitTestPath, "FakeAlgo");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 3)]
        [TestCase("fax", 2)]
        [TestCase("req", 1)]
        [TestCase("sample", 4)]
        public void ShouldCopyProperFilesFromNestedSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new FakeAlgoFileFinderBase();
            string path = Path.Combine(_unitTestPath, "SomeImplementation", "FakeAlgo");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 3)]
        [TestCase("fax", 2)]
        [TestCase("req", 1)]
        [TestCase("sample", 4)]
        public void ShouldCopyFilesFromZipFileSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new FakeAlgoFileFinderBase();
            string path = Path.Combine(_unitTestPath, "SomeLab");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonExistentSubFolder()
        {
            var subject = new FakeAlgoFileFinderBase();
            string path = Path.Combine(_unitTestPath, "FakeAlgo");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "FolderDoesNotExist", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonPrefixFolder()
        {
            var subject = new FakeAlgoFileFinderBase();
            string path = Path.Combine(_unitTestPath, "FA_FakeMode");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(0, result);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            Directory.Delete(_targetFolder, true);
        }
    }
}

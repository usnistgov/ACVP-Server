using System;
using System.IO;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_ECB.GenVal.Tests
{
    [TestFixture]
    public class AlgoFileFinderTests
    {
        string _unitTestPath;
        private string _targetFolder;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\AlgoFinderFiles");
            _targetFolder = Path.Combine(_unitTestPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_targetFolder);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Directory.Delete(_targetFolder, true);
        }

        [Test]
        [TestCase("resp", 3)]
        [TestCase("fax", 2)]
        [TestCase("req", 1)]
        [TestCase("sample", 4)]
        public void ShouldCopyProperFilesFromAESSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 3)]
        [TestCase("fax", 2)]
        [TestCase("req", 1)]
        [TestCase("sample", 4)]
        public void ShouldCopyProperFilesFromNestedAESSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath,"SomeImplementation", "AES");
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
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeLab");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromAESNonExistentSubFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp1", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonAESFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES_GCM");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(0, result);
        }
    }
}

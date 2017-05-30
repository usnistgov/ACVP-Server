using System;
using System.IO;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture, UnitTest]
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
        [TestCase("resp", 2)]
        [TestCase("fax", 9)]
        [TestCase("req", 5)]
        [TestCase("sample", 3)]
        public void ShouldCopyProperFilesFromSHASuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SHA");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 2)]
        [TestCase("fax", 9)]
        [TestCase("req", 5)]
        [TestCase("sample", 3)]
        public void ShouldCopyProperFilesFromNestedSHASuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeImplementation", "SHA");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }


        [Test]
        [TestCase("resp", 2)]
        [TestCase("fax", 9)]
        [TestCase("req", 5)]
        [TestCase("sample", 3)]
        public void ShouldCopyFilesFromZipFileSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeLab");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromSHANonExistentSubFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SHA");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp1", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonSHAFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(0, result);
        }
    }
}

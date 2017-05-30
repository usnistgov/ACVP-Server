using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
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
        [TestCase("resp", 4)]
        [TestCase("fax", 8)]
        [TestCase("req", 4)]
        [TestCase("sample", 6)]
        public void ShouldCopyProperFilesFromSHA3SuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SHA3");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 4)]
        [TestCase("fax", 8)]
        [TestCase("req", 4)]
        [TestCase("sample", 6)]
        public void ShouldCopyProperFilesFromNestedSHA3SuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeImplementation", "SHA3");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }


        [Test]
        [TestCase("resp", 4)]
        [TestCase("fax", 8)]
        [TestCase("req", 4)]
        [TestCase("sample", 6)]
        public void ShouldCopyFilesFromZipFileSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeLab");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromSHA3NonExistentSubFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SHA3");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp1", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonSHA3Folder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(0, result);
        }
    }
}

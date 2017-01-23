using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Common;
using System.IO;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class AlgoFileFinderTests
    {
        private string _unitTestPath;
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
        [TestCase("fax", 3)]
        [TestCase("req", 3)]
        [TestCase("sample", 3)]
        public void ShouldCopyFilesFromSHA1SuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SHA1");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 3)]
        [TestCase("fax", 3)]
        [TestCase("req", 3)]
        [TestCase("sample", 3)]
        public void ShouldCopyFilesFromNestedSHA1SuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeImplementation", "SHA1");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 3)]
        [TestCase("fax", 3)]
        [TestCase("req", 3)]
        [TestCase("sample", 3)]
        public void ShouldCopyFilesFromZipFileForSHA1SuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeLab");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromSHA1NonExistentSubFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SHA1");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp1", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonSHA1Folder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SHA");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(0, result);
        }
    }
}

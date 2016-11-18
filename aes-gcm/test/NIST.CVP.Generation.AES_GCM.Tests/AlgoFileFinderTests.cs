using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    [TestFixture]
    public class AlgoFileFinderTests
    {
        string _unitTestPath = Path.GetFullPath(@"..\..\TestFiles\AlgoFinderFiles");
        private string _targetFolder;

        [SetUp]
        public void Setup()
        {
            _targetFolder = Path.Combine(_unitTestPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_targetFolder);
        }

        [Test]
        [TestCase("resp", 6)]
        [TestCase("fax", 8)]
        [TestCase("req", 8)]
        [TestCase("sample", 8)]
        public void ShouldCopyFilesFromAES_GCMSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES_GCM");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        [TestCase("resp", 6)]
        [TestCase("fax", 8)]
        [TestCase("req", 8)]
        [TestCase("sample", 8)]
        public void ShouldCopyFilesFromNestedAES_GCMSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath,"SomeImplementation", "AES_GCM");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }


        [Test]
        [TestCase("resp", 6)]
        [TestCase("fax", 6)]
        [TestCase("req", 6)]
        [TestCase("sample", 6)]
        public void ShouldCopyFilesFromZipFile_GCMSuppliedSubFolder(string fileFolderName, int expected)
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "SomeLab");
            var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromAES_GCMNonExistentSubFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES_GCM");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp1", _targetFolder);
            Assert.AreEqual(-1, result);
        }

        [Test]
        public void ShouldNotCopyFilesFromNonAES_GCMFolder()
        {
            var subject = new AlgoFileFinder();
            string path = Path.Combine(_unitTestPath, "AES");
            var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
            Assert.AreEqual(0, result);
        }

        [TearDown]
        public void Teardown()
        {
           
            Directory.Delete(_targetFolder, true);
        }
    }
}

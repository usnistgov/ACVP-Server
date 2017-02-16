using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core;
using NUnit.Framework;

namespace NIST.CVP.Generation.TDES_CBC.Tests
{
    [TestFixture]
    public class AlgoFileFinderTests
    {
        // @todo Get tests files from Dean. 
        //string _unitTestPath;
        //private string _targetFolder;

        //[OneTimeSetUp]
        //public void OneTimeSetUp()
        //{
        //    _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\AlgoFinderFiles");
        //    _targetFolder = Path.Combine(_unitTestPath, Guid.NewGuid().ToString());
        //    Directory.CreateDirectory(_targetFolder);
        //}

        //[OneTimeTearDown]
        //public void OneTimeTearDown()
        //{
        //    Directory.Delete(_targetFolder, true);
        //}

        //[Test]
        //[TestCase("resp", 3)]
        //[TestCase("fax", 2)]
        //[TestCase("req", 1)]
        //[TestCase("sample", 4)]
        //public void ShouldCopyProperFilesFromTDESSuppliedSubFolder(string fileFolderName, int expected)
        //{
        //    var subject = new AlgoFileFinder();
        //    string path = Path.Combine(_unitTestPath, "TDES");
        //    var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
        //    Assert.AreEqual(expected, result);
        //}

        //[Test]
        //[TestCase("resp", 3)]
        //[TestCase("fax", 2)]
        //[TestCase("req", 1)]
        //[TestCase("sample", 4)]
        //public void ShouldCopyProperFilesFromNestedTDESSuppliedSubFolder(string fileFolderName, int expected)
        //{
        //    var subject = new AlgoFileFinder();
        //    string path = Path.Combine(_unitTestPath, "SomeImplementation", "TDES");
        //    var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
        //    Assert.AreEqual(expected, result);
        //}


        //[Test]
        //[TestCase("resp", 3)]
        //[TestCase("fax", 2)]
        //[TestCase("req", 1)]
        //[TestCase("sample", 4)]
        //public void ShouldCopyFilesFromZipFileSuppliedSubFolder(string fileFolderName, int expected)
        //{
        //    var subject = new AlgoFileFinder();
        //    string path = Path.Combine(_unitTestPath, "SomeLab");
        //    var result = subject.CopyFoundFilesToTargetDirectory(path, fileFolderName, _targetFolder);
        //    Assert.AreEqual(expected, result);
        //}

        //[Test]
        //public void ShouldNotCopyFilesFromTDESNonExistentSubFolder()
        //{
        //    var subject = new AlgoFileFinder();
        //    string path = Path.Combine(_unitTestPath, "TDES");
        //    var result = subject.CopyFoundFilesToTargetDirectory(path, "resp1", _targetFolder);
        //    Assert.AreEqual(-1, result);
        //}

        //[Test]
        //public void ShouldNotCopyFilesFromNonTDESFolder()
        //{
        //    var subject = new AlgoFileFinder();
        //    string path = Path.Combine(_unitTestPath, "AES");
        //    var result = subject.CopyFoundFilesToTargetDirectory(path, "resp", _targetFolder);
        //    Assert.AreEqual(0, result);
        //}
    }
}

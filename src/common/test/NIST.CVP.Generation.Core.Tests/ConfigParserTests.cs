﻿using System;
using System.IO;
using NIST.CVP.Tests.Core;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{

    [TestFixture, UnitTest]
    public class ConfigParserTests
    {
        private string _unitTestPath;

        [OneTimeSetUp]
        public void Setup()
        {
            _unitTestPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\..\TestFiles\ConfigFiles\");
        }

        [Test]
        public void ShouldReturnErrorForNonExistentFile()
        {
            var subject = GetSubject("badFile.json");
            Assert.IsFalse(subject.Success);
        }

        [Test]
        public void ShouldReturnErrorForNonExistentPath()
        {
            string path = $@"C:\{Guid.NewGuid()}\exampleConfig.json";
            var subject = new ConfigParser(path);
            Assert.IsFalse(subject.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldReturnErrorForNullOrEmptyPath(string path)
        {
            var subject = new ConfigParser(path);
            Assert.IsFalse(subject.Success);
        }

        [Test]
        public void ShouldReturnErrorForBadFile()
        {
            var subject = GetSubject("notJsonConfig.json");
            Assert.IsFalse(subject.Success);
        }

        [Test]
        public void ShouldParseValidFile()
        {
            var subject = GetSubject("exampleConfig.json");
            Assert.IsTrue(subject.Success);
        }

        [Test]
        [TestCase("testField", 10)]                 // Tests a regular field
        [TestCase("secondField", 20)]               // ''
        [TestCase("arrayField:0", 30)]              // Tests a field that holds an array
        [TestCase("arrayField:1", 40)]              // ''
        [TestCase("objectField:insideField", 50)]    // Tests an object that has a field inside
        public void ShouldReadCorrectValuesFromFields(string field, int value)
        {
            var subject = GetSubject("exampleConfig.json");
            Assume.That(subject.Success);
            Assert.AreEqual(value, int.Parse(subject.Configuration[field]));
        }

        private ConfigParser GetSubject(string fileName)
        {
            var path = Path.Combine(_unitTestPath, fileName);
            return new ConfigParser(path);
        }
    }
}
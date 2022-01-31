using System;
using System.IO;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Tests.Core;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class FileServiceTests
    {
        private string _testPath;
        private readonly IFileService _subject = new FileService();
        private const string FileContents = "file contents";

        [OneTimeSetUp]
        public void Setup()
        {
            _testPath = Utilities.GetConsistentTestingStartPath(GetType(), @"..\TestFiles\unitTest\fileService\");
            Directory.CreateDirectory(_testPath);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Directory.Delete(_testPath, true);
        }

        [Test]
        public void ShouldWriteFile()
        {
            var fileName = $"{Guid.NewGuid()}.txt";

            try
            {
                _subject.WriteFile(Path.Combine(_testPath, fileName), FileContents, true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            Assert.Pass();
        }

        [Test]
        public void ShouldOverwriteFileWhenTrue()
        {

            var fileName = $"{Guid.NewGuid()}.txt";
            _subject.WriteFile(Path.Combine(_testPath, fileName), FileContents, true);

            try
            {
                _subject.WriteFile(Path.Combine(_testPath, fileName), FileContents, true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            Assert.Pass();
        }

        [Test]
        public void ShouldThrowWhenOverwriteFalse()
        {
            var fileName = $"{Guid.NewGuid()}.txt";
            _subject.WriteFile(Path.Combine(_testPath, fileName), FileContents, true);

            Assert.Throws<ArgumentException>(() =>
                _subject.WriteFile(Path.Combine(_testPath, fileName), FileContents, false));
        }

        [Test]
        public void ShouldThrowWhenFileDoesNotExist()
        {
            var fileName = $"{Guid.NewGuid()}.txt";
            Assert.Throws<FileNotFoundException>(() => _subject.ReadFile(Path.Combine(_testPath, fileName)));
        }

        [Test]
        public void ShouldReadFileWhenExists()
        {
            var fileName = $"{Guid.NewGuid()}.txt";
            _subject.WriteFile(Path.Combine(_testPath, fileName), FileContents, true);

            var contents = _subject.ReadFile(Path.Combine(_testPath, fileName));

            Assert.AreEqual(FileContents, contents);
        }

        [Test]
        public async Task ShouldWriteFileAsync()
        {
            var fileName = $"{Guid.NewGuid()}.txt";

            try
            {
                await _subject.WriteFileAsync(Path.Combine(_testPath, fileName), FileContents, true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            Assert.Pass();
        }

        [Test]
        public async Task ShouldOverwriteFileWhenTrueAsync()
        {

            var fileName = $"{Guid.NewGuid()}.txt";
            await _subject.WriteFileAsync(Path.Combine(_testPath, fileName), FileContents, true);

            try
            {
                await _subject.WriteFileAsync(Path.Combine(_testPath, fileName), FileContents, true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }

            Assert.Pass();
        }

        [Test]
        public async Task ShouldThrowWhenOverwriteFalseAsync()
        {
            var fileName = $"{Guid.NewGuid()}.txt";
            await _subject.WriteFileAsync(Path.Combine(_testPath, fileName), FileContents, true);

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _subject.WriteFileAsync(Path.Combine(_testPath, fileName), FileContents, false));
        }

        [Test]
        public void ShouldThrowWhenFileDoesNotExistAsync()
        {
            var fileName = $"{Guid.NewGuid()}.txt";

            Assert.ThrowsAsync<FileNotFoundException>(async () => await _subject.ReadFileAsync(Path.Combine(_testPath, fileName)));
        }

        [Test]
        public async Task ShouldReadFileWhenExistsAsync()
        {
            var fileName = $"{Guid.NewGuid()}.txt";
            await _subject.WriteFileAsync(Path.Combine(_testPath, fileName), FileContents, true);

            var contents = await _subject.ReadFileAsync(Path.Combine(_testPath, fileName));

            Assert.AreEqual(FileContents, contents);
        }
    }
}

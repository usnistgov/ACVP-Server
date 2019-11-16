using System;
using System.IO;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public class FileService : IFileService
    {
        public string ReadFile(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                throw new FileNotFoundException(fileLocation);
            }
            
            return File.ReadAllText(fileLocation);
        }

        public void WriteFile(string destination, string contents, bool overwriteFileIfExists)
        {
            if (File.Exists(destination) && !overwriteFileIfExists)
            {
                throw new ArgumentException($"Attempted to overwrite file at \"{destination}\", but the file already exists and {nameof(overwriteFileIfExists)} was false.");
            }
            
            File.WriteAllText(destination, contents);
        }

        public async Task<string> ReadFileAsync(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                throw new FileNotFoundException(fileLocation);
            }

            return await File.ReadAllTextAsync(fileLocation);
        }

        public async Task WriteFileAsync(string destination, string contents, bool overwriteFileIfExists)
        {
            if (File.Exists(destination) && !overwriteFileIfExists)
            {
                throw new ArgumentException($"Attempted to overwrite file at \"{destination}\", but the file already exists and {nameof(overwriteFileIfExists)} was false.");
            }

            await File.WriteAllTextAsync(destination, contents);
        }
    }
}
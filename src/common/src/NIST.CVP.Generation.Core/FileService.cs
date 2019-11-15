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

        public Task<string> ReadFileAsync(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                throw new FileNotFoundException(fileLocation);
            }

            return File.ReadAllTextAsync(fileLocation);
        }

        public Task WriteFileAsync(string destination, string contents, bool overwriteFileIfExists)
        {
            if (File.Exists(destination) && !overwriteFileIfExists)
            {
                throw new ArgumentException($"Attempted to overwrite file at \"{destination}\", but the file already exists and {nameof(overwriteFileIfExists)} was false.");
            }

            return File.WriteAllTextAsync(destination, contents);
        }
    }
}
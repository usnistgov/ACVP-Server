using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    /// <summary>
    /// Interface provides methods for reading and writing files.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Read a file from a <see cref="fileLocation"/> and return the contents.
        /// </summary>
        /// <param name="fileLocation">The file location.</param>
        /// <returns>contents of the file.</returns>
        string ReadFile(string fileLocation);
        /// <summary>
        /// Write <see cref="contents"/> to a file at <see cref="destination"/>.
        /// </summary>
        /// <param name="destination">The location to write the file.</param>
        /// <param name="contents">The contents to write</param>
        /// <param name="overwriteFileIfExists">Should the file be overwritten if it already exists at the location?</param>
        void WriteFile(string destination, string contents, bool overwriteFileIfExists);
        
        /// <summary>
        /// Read a file from a <see cref="fileLocation"/> and return the contents.
        /// </summary>
        /// <param name="fileLocation">The file location.</param>
        /// <returns>Task wrapping the contents of the file.</returns>
        Task<string> ReadFileAsync(string fileLocation);
        /// <summary>
        /// Write <see cref="contents"/> to a file at <see cref="destination"/>.
        /// </summary>
        /// <param name="destination">The location to write the file.</param>
        /// <param name="contents">The contents to write</param>
        /// <param name="overwriteFileIfExists">Should the file be overwritten if it already exists at the location?</param>
        /// <returns>Task representing the write operation.</returns>
        Task WriteFileAsync(string destination, string contents, bool overwriteFileIfExists);
    }
}
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public abstract class AlgoFileFinderBase
    {
        public abstract string Name { get; }

        private Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }

        public int CopyFoundFilesToTargetDirectory(string sourceDirectory, string fileFolderName, string targetDirectory)
        {
            if (!Directory.Exists(sourceDirectory))
            {
                return -1;
            }
            
            var root = new DirectoryInfo(sourceDirectory);
            //walk folders and files
            return WalkDirectory(root, targetDirectory, fileFolderName);
        }

        private int WalkDirectory(DirectoryInfo folder, string targetDirectory, string fileFolderName)
        {
            //if aes_gcm grab files
            if (folder.Name == Name)
            {
                return CopyFilesToTarget(folder, targetDirectory, fileFolderName);
                
            }
            int filesCopied = 0;
            //walk folders and files
            foreach (var childFolder in folder.GetDirectories())
            {
                filesCopied += WalkDirectory(childFolder, targetDirectory, fileFolderName);
            }

            //if zip, extract and walk that
            foreach (var file in folder.GetFiles("*.zip"))
            {
                var extractionPath = Path.Combine(targetDirectory, $"EX_{Guid.NewGuid()}");
                ExtractFilesFromZip(file.FullName, extractionPath);
                var extractionFolder = new DirectoryInfo(extractionPath);
                filesCopied += WalkDirectory(extractionFolder, targetDirectory, fileFolderName);

                //clean up afterwards
                try
                {
                    Directory.Delete(extractionPath, true);
                }
                catch (Exception ex)
                {
                    ThisLogger.Error(ex);
                }
            }
            return filesCopied;
        }

        public bool ExtractFilesFromZip(string zipFilePath, string extractionPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipFilePath, extractionPath);
                return true;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return false;
            }
        }

        private int CopyFilesToTarget(DirectoryInfo folder, string targetDirectory, string fileFolderName)
        {
            int filesCopied = 0;
            //if match file suffix, give a good unique name and copy to target directory
            var requestedChildFolder = folder.GetDirectories(fileFolderName).FirstOrDefault();
            if (requestedChildFolder == null)
            {
                return -1;
            }

            foreach (var file in requestedChildFolder.GetFiles())
            {
                string targetFileName = file.Name.Replace(".", $"_{Guid.NewGuid()}.");
                string targetFilePath = Path.Combine(targetDirectory, targetFileName);
                File.Copy(file.FullName, targetFilePath );
                filesCopied++;
            }

            return filesCopied;
        }
    }
}
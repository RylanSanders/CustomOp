using CustomOp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CustomOp.Operations
{
    internal class CopyFileOperation : Operation
    {
        public CopyFileOperation(XElement config) : base(config)
        {
        }

        public override void execute(OpData data)
        {
            base.execute(data);
            bool isDir = Directory.Exists(data.getString("SourceFile"));
            if (isDir)
            {
                CopyDirectory(data.getString("SourceFile"), data.getString("DestinationFile"));
            }
            else
            {
                File.Copy(data.getString("SourceFile"), data.getString("DestinationFile"));
            }

        }

        //ChatGPT wrote this
        public void CopyDirectory(string sourceDirectory, string destinationDirectory)
        {
            DirectoryInfo sourceDir = new DirectoryInfo(sourceDirectory);
            DirectoryInfo destDir = new DirectoryInfo(destinationDirectory);

            // If the source directory does not exist, throw an exception or handle the error
            if (!sourceDir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory '{sourceDirectory}' does not exist.");
            }

            // If the destination directory does not exist, create it
            if (!destDir.Exists)
            {
                destDir.Create();
            }

            // Copy the files and subdirectories
            foreach (FileInfo file in sourceDir.GetFiles())
            {
                string destinationFilePath = Path.Combine(destinationDirectory, file.Name);
                file.CopyTo(destinationFilePath, false);
            }

            foreach (DirectoryInfo subDir in sourceDir.GetDirectories())
            {
                string subDirDestinationPath = Path.Combine(destinationDirectory, subDir.Name);
                CopyDirectory(subDir.FullName, subDirDestinationPath);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Service
{
    public class FileManager
    {
        public void createNewFile(string filePath, string fileContent)
        {
            using (FileStream file = File.Create(filePath))
            {
                byte[] writableContent = new UTF8Encoding(true).GetBytes(fileContent);
                file.Write(writableContent);
            }
        }

        public void createDirectory(string DirectoryPath)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        public string getProjectRootDirectory() 
        {
            string workingDirectory = Environment.CurrentDirectory;
            string? projectRootDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;

            if (projectRootDirectory == null)
            {
                throw new Exception();
            }

            return projectRootDirectory;
        }
    }
}

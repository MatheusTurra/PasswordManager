using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public class FileService : IFileService
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
            string appDataDirectory = FileSystem.Current.AppDataDirectory;
            if (appDataDirectory == null)
            {
                throw new Exception();
            }

            return appDataDirectory;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Services.Interfaces;

namespace App.Services
{
    public class FileService : IFileService
    {
        public void CreateNewFile(string filePath, string fileContent)
        {
            using (FileStream file = File.Create(filePath))
            {
                byte[] writableContent = new UTF8Encoding(true).GetBytes(fileContent);
                file.Write(writableContent);
            }
        }

        public void CreateDirectory(string DirectoryPath)
        {
            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }
        }

        public string GetProjectRootDirectory()
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

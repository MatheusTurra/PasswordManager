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
    }
}

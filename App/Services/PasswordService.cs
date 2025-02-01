using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using App.Models;
using App.Services.Interfaces;

namespace App.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IFileService fileService;
        public PasswordService(IFileService fileService) 
        {
            this.fileService = fileService;
        }

        public void CreatePasswordFile(Password password)
        {
            string passwordsFolder = CreatePasswordsFolder();
            fileService.CreateDirectory(passwordsFolder);

            string fileName = RemoveSpecialCharacters(password.name);
            string passwordFilePath = Path.Combine(passwordsFolder, fileName + ".pwd");

            string jsonPassword = JsonSerializer.Serialize(password);

            fileService.CreateNewFile(passwordFilePath, jsonPassword);
        }

        public string CreatePasswordsFolder()
        {
            string projectRootDirectory = fileService.GetProjectRootDirectory();
            return projectRootDirectory + "/Assets/Passwords";
        }


        public string RemoveSpecialCharacters(string? str)
        {
            string result = "";

            if (str != null)
            {
                result = Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled).ToLower();
            }

            return result;
        }
    }
}

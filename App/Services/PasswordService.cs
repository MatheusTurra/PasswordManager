using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using App.Models;
using App.Services.Interfaces;

namespace App.Services
{
    /*
     * TODO: REMOVER CHAMADAS ESTATICAS PARA O SISTEMA DE ARQUIVOS PARA FAZER COM QUE OS TESTES NAO FACAM CHAMADAS EXTERNAS A APLICACAO
     *       NO CASO DA CLASSE ESTATICA DIRECTORY SERA NECESSARIO EXTRAIR TODOS OS METODOS QUE O SISTEMA CHAMA EM UMA INTERFACE SEPARADA.
     */
    public class PasswordService : IPasswordService
    {
        private readonly IFileService fileService;
        public PasswordService(IFileService fileService) 
        {
            this.fileService = fileService;
        }

        public void CreatePasswordFile(Password password)
        {
            if (password.password != password.repeatPassword)
            {
                throw new Exception("As senhas são diferentes");
            }

            string passwordsFolder = GetPasswordsFolder();

            if (!Directory.Exists(passwordsFolder))
            {
                fileService.CreateDirectory(passwordsFolder);
            }

            string fileName = RemoveSpecialCharacters(password.name);
            string passwordFilePath = Path.Combine(passwordsFolder, fileName + ".pwd");

            string jsonPassword = JsonSerializer.Serialize(password);

            fileService.CreateNewFile(passwordFilePath, jsonPassword);
        }

        public string GetPasswordsFolder()
        {
            string projectRootDirectory = fileService.GetProjectRootDirectory();
            return Path.Combine(projectRootDirectory, "Passwords");
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

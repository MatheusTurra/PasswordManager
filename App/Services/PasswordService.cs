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
    //TODO: REMOVER CHAMADAS ESTATICAS PARA O SISTEMA DE ARQUIVOS PARA FAZER COM QUE OS TESTES NAO FACAM CHAMADAS EXTERNAS A APLICACAO
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

        public void EncryptFile(Password password)
        {
            string result = String.Empty;
            string fileName = RemoveSpecialCharacters(password.name);
            string jsonPassword = JsonSerializer.Serialize(password);

            byte[] encryptedData;

            string privateKey = password.name ?? "PrivateKey";


            using (var passwordFileStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonPassword ?? "")))
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = GenerateKeyFromPassword(privateKey);
                    aes.GenerateIV();

                    ICryptoTransform encryptor = aes.CreateEncryptor();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        
                        stream.Write(aes.IV, 0, aes.IV.Length); //GRAVA O VETOR DE INICIALIZACAO NO INCIO DO ARQUIVO P/ DESCRIPTOGRAFAR DEPOIS

                        using (CryptoStream cryptoStr = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(cryptoStr))
                            {
                                writer.Write(jsonPassword);
                            }

                            encryptedData = stream.ToArray();
                        }
                    }
                }

               result = Convert.ToBase64String(encryptedData);
            }

            string file = decryptPasswordFile(result, privateKey);

        }


        public string decryptPasswordFile(string encryptedFile, string privateKey)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                byte[] cipher = Convert.FromBase64String(encryptedFile);

                byte[] initializationVector = new byte[aesAlgorithm.BlockSize / 8];
                Array.Copy(cipher, 0, initializationVector, 0, initializationVector.Length);
                aesAlgorithm.IV = initializationVector;

                aesAlgorithm.Key = GenerateKeyFromPassword(privateKey);

                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

                //IGNORA O VETOR DE INICIALIZACAO COLOCADO NO COMECO DO ARQUIVO CRIPTOGRAFADO
                byte[] cipherWithoutInitializationVector = new byte[cipher.Length - initializationVector.Length];
                Array.Copy(cipher, initializationVector.Length, cipherWithoutInitializationVector, 0, cipherWithoutInitializationVector.Length);
                using (MemoryStream stream = new MemoryStream(cipherWithoutInitializationVector))
                {
                    using (CryptoStream cryptStr = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cryptStr))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private static byte[] GenerateKeyFromPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Converte a senha para bytes e gera o hash SHA-256
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                return sha256.ComputeHash(passwordBytes);
            }
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

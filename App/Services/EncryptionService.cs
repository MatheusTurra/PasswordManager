using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using App.Models;
using App.Services.Interfaces;

namespace App.Services
{
    public class EncryptionService : IEncryptionService
    {
        //TODO: A CHAVE PRIVADA P/ CRIPTOGRAFAR O ARQUIVO DE SENHAS DEVE SER A SENHA DE USUARIO
        public string EncryptPasswordFile(string passwordContent)
        {
            string result = String.Empty;

            byte[] encryptedData;

            string privateKey = "TemporaryPrivateKey";
            //string privateKey = passwordContent.name ?? "PrivateKey";


            using (var passwordFileStream = new MemoryStream(Encoding.UTF8.GetBytes(passwordContent ?? "")))
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = generateKeyFromPassword(privateKey);
                    aes.GenerateIV();

                    ICryptoTransform encryptor = aes.CreateEncryptor();
                    using (MemoryStream stream = new MemoryStream())
                    {

                        stream.Write(aes.IV, 0, aes.IV.Length);

                        using (CryptoStream cryptoStr = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter writer = new StreamWriter(cryptoStr))
                            {
                                writer.Write(passwordContent);
                            }

                            encryptedData = stream.ToArray();
                        }
                    }
                }

                result = Convert.ToBase64String(encryptedData);
            }

            return result;
        }

        private static byte[] generateKeyFromPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                return sha256.ComputeHash(passwordBytes);
            }
        }

        public string DecryptPasswordFile(string encryptedPassword)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                byte[] cipher = Convert.FromBase64String(encryptedPassword);

                byte[] initializationVector = new byte[aesAlgorithm.BlockSize / 8];
                Array.Copy(cipher, 0, initializationVector, 0, initializationVector.Length);
                aesAlgorithm.IV = initializationVector;

                aesAlgorithm.Key = generateKeyFromPassword("TemporaryPrivateKey");

                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using App.Models;
using App.Services;

namespace Tests.Unit.Services
{
    [TestClass]
    public class EncryptionServiceTests
    {
        [TestMethod]
        public void EncryptionService_PasswordContentShouldChangeAfterEncryption()
        {
            //ARRANGE
            string passwordContent = getPasswordAsJson();
            
            EncryptionService service = new EncryptionService();
            
            //ACT
            string encryptedPassword = service.EncryptPasswordFile(passwordContent);

            //ASSERT
            Assert.IsNotNull(encryptedPassword);
            Assert.AreNotEqual(passwordContent, encryptedPassword);
        }

        [TestMethod]
        public void EncryptionService_ShouldDecryptPasswordWithAllData()
        {
            //ARRANGE
            string passwordContent = getPasswordAsJson();

            EncryptionService service = new EncryptionService();

            //ACT
            string encryptedPassword = service.EncryptPasswordFile(passwordContent);
            string decryptedPassword = service.DecryptPasswordFile(encryptedPassword);

            var originalPassword = JsonSerializer.Deserialize<Password>(passwordContent);
            var decryptedPasswordObject = JsonSerializer.Deserialize<Password>(decryptedPassword);

            Assert.AreEqual(originalPassword?.user, decryptedPasswordObject?.user);
            Assert.AreEqual(originalPassword?.name, decryptedPasswordObject?.name);
            Assert.AreEqual(originalPassword?.password, decryptedPasswordObject?.password);
            Assert.AreEqual(originalPassword?.repeatPassword, decryptedPasswordObject?.repeatPassword);
        }

        [TestMethod]
        public void EncryptPasswordFile_ShouldGenerartADifferentIVEachTime()
        {
            //ARRANGE
            string passwordContent = getPasswordAsJson();

            EncryptionService service = new EncryptionService();

            // ACT
            string encryptedDataBase64_1 = service.EncryptPasswordFile(passwordContent);
            string encryptedDataBase64_2 = service.EncryptPasswordFile(passwordContent);
            string encryptedDataBase64_3 = service.EncryptPasswordFile(passwordContent);

            byte[] encryptedData1 = Convert.FromBase64String(encryptedDataBase64_1);
            byte[] encryptedData2 = Convert.FromBase64String(encryptedDataBase64_2);
            byte[] encryptedData3 = Convert.FromBase64String(encryptedDataBase64_3);

            byte[] iv1 = new byte[16];
            byte[] iv2 = new byte[16];
            byte[] iv3 = new byte[16];

            Array.Copy(encryptedData1, 0, iv1, 0, 16);
            Array.Copy(encryptedData2, 0, iv2, 0, 16);
            Array.Copy(encryptedData3, 0, iv3, 0, 16);

            // ASSERT
            Assert.IsFalse(iv1.SequenceEqual(iv2));
            Assert.IsFalse(iv1.SequenceEqual(iv3));
            Assert.IsFalse(iv2.SequenceEqual(iv3));
        }

        private string getPasswordAsJson()
        {
            Password password = new Password()
            {
                name = "EncryptionTest",
                password = "test123",
                repeatPassword = "test123",
                user = "unitTest"
            };

            return JsonSerializer.Serialize(password);
        }
    }
}

﻿using App.Models;
using App.Services;
using App.Services.Interfaces;
using Moq;
using Newtonsoft.Json;

namespace Tests.Unit.Services
{
    [TestClass]
    public class PasswordServiceTests
    {
        //TODO: CLASSES ESTATICAS FILESERVICE E DIRECTORY ESTAO CRIANDO PASTAS/ARQUIVOS AO SEREM EXECUTADAS NOS TESTES DE UNIDADE
        [TestMethod]
        public void CreatesPasswordDirectory()
        {
            //ARRANGE
            string projectRootDirectory = "C://ProjectRoot/";
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(service => service.GetProjectRootDirectory()).Returns(projectRootDirectory);
            PasswordService passwordService = getPasswordServiceMock(fileService);
            Password password = getFakePassword();

            //ACT
            passwordService.CreatePasswordFile(password);

            //ASSERT 
            string expectedDirectoryPath = projectRootDirectory + "Passwords";
            fileService.Verify(service => service.CreateDirectory(expectedDirectoryPath), Times.Once());
        }

        [TestMethod]
        public void CreatePasswordFile_FileIsCreatedWithoutSpecialCharacters()
        {
            //ARRANGE
            var fileService = getFileServiceMock();

            string createNewFilePathParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePathParameter = filePath);

            PasswordService passwordService = getPasswordServiceMock(fileService);
            Password password = getFakePassword();
            password.name = "unitTéstSpecíãlCharactêrs!";

            //ACT
            passwordService.CreatePasswordFile(password);

            //ASSERT
            string fileName = Path.GetFileNameWithoutExtension(createNewFilePathParameter);
            bool nameContainsOnlyAlphanumeric = fileName.All(name => Char.IsLetterOrDigit(name));
            Assert.IsTrue(nameContainsOnlyAlphanumeric);
        }

        [TestMethod]
        public void CreatePasswordFile_FileIsCreatedWithCorrectExtension()
        {
            //ARRANGE
            var fileService = getFileServiceMock();

            string createNewFilePathParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePathParameter = filePath);

            //ACT
            PasswordService passwordService = getPasswordServiceMock(fileService);
            Password password = getFakePassword();
            password.name = "unitTestFileExtension";

            passwordService.CreatePasswordFile(password);

            //ASSERT
            string fileName = Path.GetExtension(createNewFilePathParameter);
            Assert.AreEqual<string>(fileName, ".pwd");
        }

        [TestMethod]
        public void CreatePasswordFile_JsonFileCreatedWithFormData()
        {
            //ARRANGE
            var fileService = getFileServiceMock();

            string createNewFilePasswordParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePasswordParameter = fileContent);

            var encryptionService = new EncryptionService();

            PasswordService passwordService = new PasswordService(fileService.Object, encryptionService);

            Password password = getFakePassword();

            //ACT
            passwordService.CreatePasswordFile(password);

            var fileContent = encryptionService.DecryptPasswordFile(createNewFilePasswordParameter);

            var jsonPassword = JsonConvert.DeserializeObject<Password>(fileContent);

            //ASSERT
            Assert.AreEqual<string>(password.name, jsonPassword?.name);
            Assert.AreEqual<string>(password.user, jsonPassword?.user);
            Assert.AreEqual<string>(password.password, jsonPassword?.password);
            Assert.AreEqual<string>(password.repeatPassword, jsonPassword?.repeatPassword);

        }

        //TODO: VERIFICAR A MENSAGEM DA EXCEPTION
        [TestMethod]
        public void CreatePasswordFile_ThrowsExceptionIfPasswordsIsDifferent()
        {
            //ARRANGE
            var fileService = getFileServiceMock();

            Password password = getFakePassword();
            password.repeatPassword = "differentPassword";

            //ACT & ASSERT
            PasswordService passwordService = getPasswordServiceMock(fileService);
            Assert.ThrowsException<Exception>(() => passwordService.CreatePasswordFile(password));
        }

        private Mock<IFileService> getFileServiceMock()
        {
            var fileService = new Mock<IFileService>();
            fileService.Setup(fs => fs.GetProjectRootDirectory()).Returns("C:/UnitTest");
            return fileService;
        }
        
        private static PasswordService getPasswordServiceMock(Mock<IFileService> fileService)
        {
            Mock<IEncryptionService> encryption = new Mock<IEncryptionService>();
            return new PasswordService(fileService.Object, encryption.Object);
        }

        private Password getFakePassword()
        {
            return new Password() {
                name = "TestPassword",
                user = "user@test.com",
                password = "12345",
                repeatPassword = "12345"
            };
        }
    }
}

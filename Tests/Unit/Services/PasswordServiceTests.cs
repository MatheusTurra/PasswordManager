using App.Models;
using App.Services;
using App.Services.Interfaces;
using Moq;
using Newtonsoft.Json;

namespace Tests.Unit.Services
{
    [TestClass]
    public class PasswordServiceTests
    {
        [TestMethod]
        public void     CreatesPasswordDirectory()
        {
            //ARRANGE
            string projectRootDirectory = "C://ProjectRoot/";
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(service => service.GetProjectRootDirectory()).Returns(projectRootDirectory);

            PasswordService passwordService = new PasswordService(fileService.Object);
            Password password = getFakePassword();
            
            //ACT
            passwordService.CreatePasswordFile(password);

            //ASSERT
            string expectedDirectoryPath = projectRootDirectory + "/Assets/Passwords";
            fileService.Verify(service => service.CreateDirectory(expectedDirectoryPath), Times.Once());
        }

        [TestMethod]
        public void CreatePasswordFile_FileIsCreatedWithoutSpecialCharacters()
        {
            //ARRANGE
            var fileService = new Mock<IFileService>();

            string createNewFilePathParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePathParameter = filePath);

            PasswordService passwordService = new PasswordService(fileService.Object);
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
            var fileService = new Mock<IFileService>();

            string createNewFilePathParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePathParameter = filePath);

            //ACT
            PasswordService passwordService = new PasswordService(fileService.Object);
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
            var fileService = new Mock<IFileService>();

            string createNewFileJsonPasswordParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFileJsonPasswordParameter = fileContent);

            PasswordService passwordService = new PasswordService(fileService.Object);
            
            Password password = getFakePassword();

            //ACT
            passwordService.CreatePasswordFile(password);

            var jsonPassword = JsonConvert.DeserializeObject<Password>(createNewFileJsonPasswordParameter);
            Assert.AreEqual<string>(password.name, jsonPassword?.name);
            Assert.AreEqual<string>(password.user, jsonPassword?.user);
            Assert.AreEqual<string>(password.password, jsonPassword?.password);
            Assert.AreEqual<string>(password.repeatPassword, jsonPassword?.repeatPassword);

        }

        [TestMethod]
        public void CreatePasswordFile_ThrowsExceptionIfPasswordsIsDifferent()
        {
            //ARRANGE
            var fileService = new Mock<IFileService>();

            Password password = getFakePassword();
            password.repeatPassword = "differentPassword";

            //ACT & ASSERT
            PasswordService passwordService = new PasswordService(fileService.Object);
            Assert.ThrowsException<Exception>(() => passwordService.CreatePasswordFile(password));
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

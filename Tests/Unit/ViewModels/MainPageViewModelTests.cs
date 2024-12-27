using App.Services;
using App.ViewModels;
using App.Models;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tests.Unit.ViewModels
{
    [TestClass]
    public class MainPageViewModelTests
    {


        [TestMethod]
        public void SavePassword_CreatesPasswordDirectory()
        {
            //ARRANGE
            string projectRootDirectory = "C://ProjectRoot/";
            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(service => service.GetProjectRootDirectory()).Returns(projectRootDirectory);

            var mainPage = new MainPageViewModel(fileService.Object);

            //ACT
            mainPage.CreatePasswordFile();

            //ASSERT
            string expectedDirectoryPath = projectRootDirectory + "/Assets/Passwords";
            fileService.Verify(service => service.CreateDirectory(expectedDirectoryPath), Times.Once());
        }

        [TestMethod]
        public void SavePassword_FileIsCreatedWithoutSpecialCharacters()
        {
            //ARRANGE
            var fileService = new Mock<IFileService>();

            string createNewFilePathParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePathParameter = filePath);

            var mainPage = new MainPageViewModel(fileService.Object);
            mainPage.Name = "unitTéstSpecíãlCharactêrs!";

            //ACT
            mainPage.CreatePasswordFile();

            //ASSERT
            string fileName = Path.GetFileNameWithoutExtension(createNewFilePathParameter);
            bool nameContainsOnlyAlphanumeric = fileName.All(name => Char.IsLetterOrDigit(name));
            Assert.IsTrue(nameContainsOnlyAlphanumeric);
        }

        [TestMethod]
        public void SavePassword_FileIsCreatedWithCorrectExtension()
        {
            //ARRANGE
            var fileService = new Mock<IFileService>();
            
            string createNewFilePathParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePathParameter = filePath);

            //ACT
            var mainPage = new MainPageViewModel(fileService.Object);
            mainPage.Name = "unitTestFileExtension";

            mainPage.CreatePasswordFile();

            //ASSERT
            string fileName = Path.GetExtension(createNewFilePathParameter);
            Assert.AreEqual<string>(fileName, ".pwd");
        }


        [TestMethod]
        public void SavePassword_JsonFileCreatedWithFormData()
        {
            //ARRANGE
            var fileService = new Mock<IFileService>();
            
            string createNewFileJsonPasswordParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFileJsonPasswordParameter = fileContent);

            var mainPage = new MainPageViewModel(fileService.Object);
            mainPage.Name = "SiteNameUnitTest";
            mainPage.User = "userUnitTest";
            mainPage.Password = "passwordUnitTest";
            mainPage.RepeatPassword = "passwordUnitTest";


            //ACT
            mainPage.CreatePasswordFile();

            var jsonPassword = JsonConvert.DeserializeObject<Password>(createNewFileJsonPasswordParameter);
            Assert.AreEqual<string>(mainPage.Name, jsonPassword?.name);
            Assert.AreEqual<string>(mainPage.User, jsonPassword?.user);
            Assert.AreEqual<string>(mainPage.Password, jsonPassword?.password);
            Assert.AreEqual<string>(mainPage.RepeatPassword, jsonPassword?.repeatPassword);

        }
    }
}

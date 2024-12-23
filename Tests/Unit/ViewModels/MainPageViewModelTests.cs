using App.Services;
using App.ViewModels;
using Moq;

namespace Tests.Unit.ViewModels
{
    [TestClass]
    public class MainPageViewModelTests
    {


        //TODO: CRIAR NOVOS MOCKS AO EXECUTAR CADA METODO (PESQUISAR DECORATOR MSTESTS)
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
        public void SavePassword_PasswordFileIsCreatedWithoutSpecialCharacters()
        {
            //ARRANGE
            var fileService = new Mock<IFileService>();

            string createNewFilePathParameter = "";
            fileService.Setup(fs => fs.CreateNewFile(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((filePath, fileContent) => createNewFilePathParameter = filePath);

            var mainPage = new MainPageViewModel(fileService.Object);
            mainPage.Name = "unitTést!";

            //ACT
            mainPage.CreatePasswordFile();

            //ASSERT
            string fileName = Path.GetFileNameWithoutExtension(createNewFilePathParameter);
            bool nameContainsOnlyAlphanumeric = fileName.All(name => Char.IsLetterOrDigit(name));
            Assert.IsTrue(nameContainsOnlyAlphanumeric);
        }

    }
}

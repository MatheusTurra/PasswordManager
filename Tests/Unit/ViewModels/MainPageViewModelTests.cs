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
        public void Test_SavePassword_CreatesPasswordDirectory()
        {
            //ARRANGE
            string projectRootDirectory = "C://ProjectRoot/";

            Mock<IFileService> fileService = new Mock<IFileService>();
            fileService.Setup(service => service.getProjectRootDirectory()).Returns(projectRootDirectory);

            //REFATORACAO: CRIAR METODO FACTORY PARA INSTANCIAR O METODO SOB TESTE
            var mainPage = new MainPageViewModel(fileService.Object);

            //ACT
            mainPage.CreatePasswordFile();


            //ASSERT
            string expectedDirectoryPath = projectRootDirectory + "/Assets/Passwords";
            fileService.Verify(service => service.createDirectory(expectedDirectoryPath), Times.Once());
        }
    }
}

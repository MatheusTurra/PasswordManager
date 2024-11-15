using App.ViewModels;

namespace Tests.Unit.ViewModels
{
    [TestClass]
    public class MainPageViewModelTests
    {
        [TestMethod]
        public void isMainPageViewModelAccessible()
        {
            var mainPage = new MainPageViewModel();

            string test = mainPage.unitTest();
            Assert.AreEqual(test, "TODO: Unit tests");
        }
    }
}

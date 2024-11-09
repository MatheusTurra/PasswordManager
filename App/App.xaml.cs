
namespace App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window fixedSizeWindow = new Window(new AppShell());
            fixedSizeWindow.MaximumWidth = 700;
            fixedSizeWindow.MaximumHeight = 700;
            return fixedSizeWindow;
        }
    }
}

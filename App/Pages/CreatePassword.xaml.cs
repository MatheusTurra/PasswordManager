using App.ViewModels;

namespace App.Pages
{
    public partial class CreatePassword : ContentPage
    {
        public CreatePassword(CreatePasswordViewModel mainPageViewModel)
        {
            InitializeComponent();
            BindingContext = mainPageViewModel;
        }
    }
}

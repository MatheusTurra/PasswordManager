using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using App.Services.Interfaces;

namespace App.ViewModels
{
    public partial class MainPageViewModel : ObservableValidator
    {
        [Required(ErrorMessage = "Digite o nome do programa ou site")]
        [MinLength(2, ErrorMessage = "O nome do site ou programa deve conter mais de dois caracteres")]
        [ObservableProperty]
        private string? _name = "";

        [Required(ErrorMessage = "Digite o nome de usuário do programa ou site")]
        [MinLength(3, ErrorMessage = "O nome de usuário do site ou programa deve conter mais de três caracteres")]
        [ObservableProperty]
        private string? _user = "";

        [Required(ErrorMessage = "Digite o nome da senha")]
        [MinLength(5, ErrorMessage = "A senha deve conter mais de cinco caracteres")]
        [ObservableProperty]
        private string? _password = "";

        [Required(ErrorMessage = "Digite novamente o nome da senha")]
        [MinLength(5, ErrorMessage = "A senha deve conter mais de cinco caracteres")]
        [ObservableProperty]
        private string? _repeatPassword = "";

        public static string CurrentYear { get => DateTime.Now.Year.ToString(); }

        private readonly IPasswordService passwordService;
        public MainPageViewModel(IPasswordService passwordService)
        { 
            this.passwordService = passwordService;
        }
        
        [RelayCommand]
        private void savePassword()
        {
            ValidateAllProperties();
            
            Password password = new Password()
            {
                name = Name,
                user = User,
                password = Password,
                repeatPassword = RepeatPassword
            };

            passwordService.CreatePasswordFile(password);
        }
    }
}

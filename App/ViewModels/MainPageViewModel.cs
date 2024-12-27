using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Services;
using App.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

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

        private readonly IFileService _fileService;
        public MainPageViewModel(IFileService fileService)
        { 
            _fileService = fileService;
        }
        
        [RelayCommand]
        private void savePassword()
        {
            ValidateAllProperties();
            
            CreatePasswordFile();
        }

        //TODO: VALIDATE IF THE TWO PASSWORDS ARE EQUAL
        public void CreatePasswordFile()
        {
            string projectRootDirectory = _fileService.GetProjectRootDirectory();
            string passwordsFolder = projectRootDirectory + "/Assets/Passwords";

            _fileService.CreateDirectory(passwordsFolder);

            string fileName = RemoveSpecialCharacters(Name);
            string passwordFilePath = Path.Combine(passwordsFolder, fileName + ".pwd");

            string jsonPassword = CreatePasswordAsJson();

            _fileService.CreateNewFile(passwordFilePath, jsonPassword);
        }

        private Password CreatePassword()
        {
            return new Password()
            {
                name = Name,
                user = User,
                password = Password,
                repeatPassword = RepeatPassword
            };
        }

        private string CreatePasswordAsJson()
        {
            return JsonSerializer.Serialize(CreatePassword());
        }

        private string RemoveSpecialCharacters(string? str)
        {
            string result = "";

            if (str != null)
            {
                result = Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled).ToLower();
            }

            return result;
        }
    }
}

﻿using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Services;
using App.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace App.ViewModels
{
    public partial class MainPageViewModel: ObservableValidator
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

        public string CurrentYear { get => DateTime.Now.Year.ToString(); }

        [RelayCommand]
        private void savePassword()
        {
            ValidateAllProperties();

            string projectRootDirectory = getFileService().getProjectRootDirectory();
            string passwordsFolder = projectRootDirectory + "/Assets/Passwords";

            getFileService().createDirectory(passwordsFolder);

            string fileName = RemoveSpecialCharacters(Name);
            string passwordFilePath = System.IO.Path.Combine(passwordsFolder, fileName + ".pwd");

            string jsonPassword = createPasswordAsJson();

            getFileService().createNewFile(passwordFilePath, jsonPassword);

            Debug.WriteLine(passwordsFolder);
        }
        
        private Password createPassword()
        {
            return new Password()
            {
                name = Name,
                user = User,
                password = Password,
                repeatPassword = RepeatPassword
            };
        }

        private string createPasswordAsJson()
        {
            return JsonSerializer.Serialize(createPassword());
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

        protected FileService getFileService()
        {
            return new FileService();
        }

        public string unitTest()
        {
            return "TODO: Unit tests";
        }
    }
}

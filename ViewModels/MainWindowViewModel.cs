using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PasswordManager.ViewModels;

public partial class MainWindowViewModel : ObservableValidator
{
    [ObservableProperty]
    [Required(ErrorMessage = "Digite o nome do programa ou site")]
    [MinLength(2, ErrorMessage = "O nome do site ou programa deve conter mais de dois caracteres")]
    private string? _name = "";

    [Required(ErrorMessage = "Digite o nome da senha")]
    [MinLength(5, ErrorMessage = "A senha deve conter mais de cinco caracteres")]
    [ObservableProperty]
    private string? _password = "";

    public string CurrentYear { get => DateTime.Now.Year.ToString(); }


    [RelayCommand]
    private void savePassword()
    {
        ValidateAllProperties();
        getPasswordViewModel().createPassword(Name, Password);
    }

    private PasswordViewModel getPasswordViewModel()
    {
        return new PasswordViewModel();
    }
}

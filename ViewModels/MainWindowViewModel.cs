using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PasswordManager.ViewModels;

public partial class MainWindowViewModel : ObservableValidator
{   
    [ObservableProperty]
    [Required(ErrorMessage = "Mensagem validacao do nome")]
    [MinLength(2, ErrorMessage = "Validacao tamanho minimo do nome do programa")]
    private string? _name = "";

    [Required(ErrorMessage = "Mensagem validacao senha")]
    [MinLength(5, ErrorMessage = "Validacao tamanho minimo da senha")]
    [ObservableProperty]
    private string? _password = "";

    [RelayCommand]
    private void savePassword()
    {
        ValidateAllProperties();

         //IEnumerable<ValidationResult> validationErrors = GetErrors();
        
        foreach (ValidationResult validation in GetErrors())
        {
            IEnumerable<string> erroList = validation.MemberNames;
            
        }

        if (HasErrors)
        {
            return;
        }

        getPasswordViewModel().createPassword(Name, Password);
    }

    private PasswordViewModel getPasswordViewModel()
    {
        return new PasswordViewModel();
    }
}

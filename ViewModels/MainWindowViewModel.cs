using Avalonia.Controls.Shapes;
using Avalonia.Rendering;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace PasswordManager.ViewModels;

public partial class MainWindowViewModel : ObservableValidator
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


        string workingDirectory = Environment.CurrentDirectory;
        string? projectRootDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;

        if (projectRootDirectory != null)
        {
            string passwordsFolder = projectRootDirectory + "/Assets/Passwords";

            if (!Directory.Exists(passwordsFolder))
            {
                Directory.CreateDirectory(passwordsFolder);
            }


            string passwordFilePath = System.IO.Path.Combine(passwordsFolder, "file.txt");
            if (File.Exists(passwordFilePath))
            {
                //TODO: LER ARQUIVO E VERIFICAR SE A SENHA JA EXISTE
                //TODO: SALVAR ARQUIVO COM O NOME DO SITE DA SENHA
                Password password = createPassword();
                writePasswordToFile(passwordFilePath, password);
            } 
            else
            {
                savePasswordInNewFile(passwordFilePath);
            }

            Debug.WriteLine(passwordsFolder);
        }
    }

    private void writePasswordToFile(string filePath, Password password)
    {
        using (StreamWriter file = new StreamWriter(filePath, true))
        {
            string jsonPassword = JsonSerializer.Serialize(createPassword());
            file.WriteLine(jsonPassword);
        }
    }

    private void savePasswordInNewFile(string filePath)
    {
        using (FileStream file = File.Create(filePath))
        {
            string passwordJson = createPasswordAsJson();
            byte[] writablePassword = new UTF8Encoding(true).GetBytes("[" + passwordJson + "]");
            file.Write(writablePassword);
        }
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
}

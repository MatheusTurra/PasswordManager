using PasswordManager.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PasswordManager.ViewModels
{
    public partial class PasswordViewModel : ViewModelBase
    {
        public Password createPassword(string? newName, string? newPassword)
        {
            return new Password()
            {
                name = newName,
                password = newPassword
            };
        }
    }
}

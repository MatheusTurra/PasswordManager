using App.Models;

namespace App.Services.Interfaces
{
    internal interface IEncryptionService
    {
        string DecryptPasswordFile(string encryptedPassword);
        string EncryptPasswordFile(string passwordContent);
    }
}
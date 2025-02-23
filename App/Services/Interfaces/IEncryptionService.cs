using App.Models;

namespace App.Services.Interfaces
{
    public interface IEncryptionService
    {
        string DecryptPasswordFile(string encryptedPassword);
        string EncryptPasswordFile(string passwordContent);
    }
}
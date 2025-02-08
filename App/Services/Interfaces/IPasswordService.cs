using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Services.Interfaces
{
    public interface IPasswordService
    {
        void CreatePasswordFile(Password password);
        string GetPasswordsFolder();
        string RemoveSpecialCharacters(string? str);
    }
}

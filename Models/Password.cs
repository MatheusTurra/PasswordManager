using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    public class Password
    {
        public string? name { get; set; }

        public string? user { get; set; }

        public string? password { get; set; }

        public string? repeatPassword { get; set; }
    }
}

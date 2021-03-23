using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eigenmaaltijden.wwwroot.classes
{
    public class UserData
    {

        public string Name;
        public string Email;

        public UserData(string Name, string Email)
        {
            this.Name = Name;
            this.Email = Email;
        }
    }
}

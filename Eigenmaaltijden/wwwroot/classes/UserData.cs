using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eigenmaaltijden.wwwroot.classes
{
    public class UserData
    {

        public uint UserID;
        public string Name;
        public string Email;
        public string Street;
        public int Number;
        public string Addon;
        public string City;
        public string Country;
        public string PostCode;

        public UserData(UInt32 UserID, string Name, string Email, string Street, UInt16 Number, string Addon, string City, string Country, string PostCode)
        {
            this.UserID = UserID;
            this.Name = Name;
            this.Email = Email;
            this.Street = Street;
            this.Number = Number;
            this.Addon = Addon;
            this.Country = Country;
            this.PostCode = PostCode;
        }
    }
}

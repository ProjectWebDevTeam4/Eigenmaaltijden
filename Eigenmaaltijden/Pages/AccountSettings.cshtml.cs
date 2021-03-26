using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Eigenmaaltijden.wwwroot.includes;
using Microsoft.AspNetCore.Http;
using Dapper;

namespace Eigenmaaltijden.Pages
{
    public class AccountSettingsModel : PageModel
    {
        public Database db = Database.get();

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }
        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage = "";

        public void OnGet()
        {

        }

        public IActionResult OnPostAccountSettings()
        {
            using var connection = db.Connect();

            //check if email isn't taken
            if (Email != db.GetLoggedInUser().Email)
            {
                if (connection.QuerySingle<int>("SELECT COUNT(*) FROM verkoper WHERE `Email`=@Email", new { Email }) == 0)
                {
                    string OrginalEmail = db.GetLoggedInUser().Email;
                    connection.Execute("UPDATE `verkoper` SET `Email`=@Email WHERE `Email`=@OrginalEmail", new { Email, OrginalEmail });
                }
                else
                {
                    //Email already taken
                    ErrorMessage = "Email is already taken.";
                }
            }

            if (NewPassword != ConfirmPassword && !string.IsNullOrWhiteSpace(NewPassword)) // Is Password equal fo comfirm?
            {
                ErrorMessage = "Passwords don't match.";
                return Page();
            }

            if (NewPassword.Length <= 7 && !string.IsNullOrWhiteSpace(NewPassword)) // Is Password Valid?
            {
                ErrorMessage = "Password should be atleast 8 characters long.";
                return Page();
            }

            if (NewPassword == CurrentPassword)
            {
                ErrorMessage = "Password cannot be the same as old password.";
                return Page();
            }

            if (NewPassword != "") { 
                if (connection.QuerySingle<int>("SELECT COUNT(*) FROM verkoper WHERE `Email`=@Email AND `Password`=@CurrentPassword", new { Email, CurrentPassword }) == 0)
                {
                    connection.Execute("UPDATE `verkoper` SET `Password`=@NewPassword WHERE `Email`=@Email AND `Password`=@CurrentPassword", new { NewPassword, Email, CurrentPassword });
                }
                else
                {
                    //Password isn't correct

                }

            }



            //if password is not null ? update password if current password is correct!

            /*
            if (connection.QuerySingle<int>("SELECT COUNT(*) FROM verkoper WHERE `Email`=@Email", new { Email }) == 0)
            {
                connection.Execute("INSERT INTO verkoper (Email, Password) VALUES (@Email, @Password)", new { Email, Password });
                // need id for user profile
                int UserID = connection.QuerySingle<int>("SELECT UserID FROM verkoper WHERE Email=@Email AND Password=@Password", new { Email, Password });
                string ProfilePhotoPath = "img/users/default.png"; // Sets profile picture to default (placeholder) picture.
                connection.Execute("INSERT INTO verkoper_profiel (UserID, Name, ProfilePhotoPath) VALUES (@UserID, @Name, @ProfilePhotoPath)", new { UserID, Name, ProfilePhotoPath });
                return OnPostLogin();
            }
            else
            {
                ErrorMessage = "Email or Password does not exist.";
                return Page();
            }*/

            return Page();
        }
    }
}

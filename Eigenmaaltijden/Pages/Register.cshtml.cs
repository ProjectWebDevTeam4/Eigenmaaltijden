using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eigenmaaltijden.wwwroot.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Dapper;

namespace Eigenmaaltijden.Pages
{
    public class RegisterModel : PageModel
    {

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string PhoneNumber { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage = "";

        [BindProperty]
        public bool AcceptedTOS { get; set; }

        Database db = new wwwroot.includes.Database();

        public void OnGet()
        {

        }

        /// <summary>
        /// Checks if user exists and if user exists, logs in that user.
        /// </summary>
        /// <returns>returns current page when user cant login, returns index page when user is sucessfully logged in.</returns>
        public IActionResult OnPostLogin()
        {
            if (Password != null && Email != null)
            {
                using var connection = db.Connect();

                int id = connection.QuerySingle<int>("SELECT `UserID` FROM `verkoper` WHERE `Email`=@Email AND `Password`=@Password", new { Email, Password });

                if (id != 0)
                {
                    var rand = new Random();
                    int cd = rand.Next(1000000, 1000000000);
                    connection.Execute("UPDATE `verkoper` SET `SessionID`=@cd WHERE `Email`=@Email AND `UserID`=@id AND `Password`=@Password", new { cd, Email, id, Password });
                    HttpContext.Session.SetString("uid", "" + id);
                    HttpContext.Session.SetString("sessionid", "" + cd);
                }
            }
            return RedirectToPage("Index");
        }

        /// <summary>
        /// Registers the user when all fields are valid.
        /// </summary>
        /// <returns>returns index when succesful, returns current page when failed</returns>
        public IActionResult OnPostRegister()
        {
            using var connection = db.Connect();

            if (!AcceptedTOS)
            {
                ErrorMessage = "Please accept the Terms Of Service.";
                return Page();
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords don't match.";
                return Page();
            }

            if (Password.Length <= 7)
            {
                ErrorMessage = "Password should be atleast 8 characters long.";
                return Page();
            }

            if (connection.QuerySingle<int>("SELECT COUNT(*) FROM verkoper WHERE `Email`=@Email", new { Email } ) == 0)
            {
                connection.Execute("INSERT INTO verkoper (Email, Password) VALUES (@Email, @Password)", new { Email, Password });
                // need id for user profile
                int UserID = connection.QuerySingle<int>("SELECT UserID FROM verkoper WHERE Email=@Email AND Password=@Password", new { Email, Password});
                string ProfilePhotoPath = "img/users/default.png"; // Sets profile picture to default (placeholder) picture.
                connection.Execute("INSERT INTO verkoper_profiel (UserID, Name, ProfilePhotoPath) VALUES (@UserID, @Name, @ProfilePhotoPath)", new { UserID, Name, ProfilePhotoPath });
                return OnPostLogin();
            }
            else
            {
                ErrorMessage = "Email or Password does not exist.";
                return Page();
            }
        }
    }
}

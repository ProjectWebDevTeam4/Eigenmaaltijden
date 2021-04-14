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

        [BindProperty]
        public string Street { get; set; }

        [BindProperty]
        public int HouseNumber { get; set; }

        [BindProperty]
        public string Addon { get; set; }

        [BindProperty]
        public string City { get; set; }

        [BindProperty]
        public string Country { get; set; }

        [BindProperty]
        public string PostCode { get; set; }


        [BindProperty]
        public bool AcceptedTOS { get; set; }

        public string ErrorMessage = "";


        Database db = Database.get();


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

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Password cannot be only spaces.";
                return Page();
            }

            if (connection.QuerySingle<int>("SELECT COUNT(*) FROM verkoper WHERE `Email`=@Email", new { Email } ) == 0)
            {
                connection.Execute("INSERT INTO verkoper (Email, Password, SessionID) VALUES (@Email, @Password, 0)", new { Email, Password });
                // need id for user profile
                int UserID = connection.QuerySingle<int>("SELECT UserID FROM verkoper WHERE Email=@Email AND Password=@Password", new { Email, Password});
                string ProfilePhotoPath = "img/users/default.png"; // Sets profile picture to default (placeholder) picture.

                connection.Execute("INSERT INTO verkoper_profiel (UserID, Name, ProfilePhotoPath, Description, EmailHidden) VALUES (@UserID, @Name, @ProfilePhotoPath, '', 0)", new { UserID, Name, ProfilePhotoPath });

                connection.Execute(@"INSERT INTO verkoper_adres (UserID, Street, Number, Addon, City, Country, PostCode) 
                                    VALUES (@UserID, @Street, @HouseNumber, @Addon, @City, @Country, @PostCode)", 
                                    new { UserID, Street, HouseNumber, Addon, City, Country, PostCode});

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

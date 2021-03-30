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
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmNewPassword { get; set; }

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

        public string ErrorMessage = "";
        public bool isLoggedIn { get; set; }

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

            uint UserID = db.GetLoggedInUser().UserID;
            connection.Execute("UPDATE `verkoper_adres` SET `Street`=@Street, `Number`=@HouseNumber, `Addon`=@Addon, `City`=@City, `Country`=@Country, `PostCode`=@PostCode WHERE UserID=@UserID", new { Street, HouseNumber, Addon, City, Country, PostCode, UserID});

            if (!string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrWhiteSpace(ConfirmNewPassword))
            {
                if (NewPassword != ConfirmNewPassword || string.IsNullOrWhiteSpace(NewPassword)) // Is Password equal fo comfirm?
                {
                    ErrorMessage = "Passwords don't match.";
                    return Page();
                }

                if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length <= 7) // Is Password Valid?
                {
                    ErrorMessage = "Password should be atleast 8 characters long.";
                    return Page();
                }

                if (NewPassword == CurrentPassword)
                {
                    ErrorMessage = "Password cannot be the same as old password.";
                    return Page();
                }

                if (NewPassword != "")
                {
                    if (connection.QuerySingle<int>("SELECT COUNT(*) FROM verkoper WHERE `Email`=@Email AND `Password`=@CurrentPassword", new { Email, CurrentPassword }) == 0)
                    {
                        connection.Execute("UPDATE `verkoper` SET `Password`=@NewPassword WHERE `Email`=@Email AND `Password`=@CurrentPassword", new { NewPassword, Email, CurrentPassword });
                    }
                    else
                    {
                        //Password isn't correct

                    }

                }
            }
            db.loginCheck(HttpContext.Session.GetString("sessionid"), HttpContext.Session.GetString("uid"));

            return RedirectToPage("AccountSettings");
        }

        public IActionResult OnGet()
        {
            isLoggedIn = db.loginCheck(HttpContext.Session.GetString("sessionid"),
                HttpContext.Session.GetString("uid"));
            if (!isLoggedIn)
            {
                return RedirectToPage("/Login");
            }

            return null;
        }
    }
}

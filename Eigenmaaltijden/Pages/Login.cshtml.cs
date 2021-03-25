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
    public class LoginModel : PageModel
    {


        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage = "";

        Database db = new wwwroot.includes.Database();

        public void OnGet()
        {

        }

        /*public void RegisterUser(string Username, string Password)
        {
            using var connection = Connect();
            connection.Query("INSERT INTO Users (Username, Password) VALUES (@Username, @Password);",
                new { Username, Password });
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostLogin()
        {
            if (Password != null && Email != null)
            {
                //var dPs = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Password));
                var dPs = Password;
                using var connection = db.Connect();

                int id = connection.QuerySingle<int>("SELECT `UserID` FROM `verkoper` WHERE `Email`=@Email AND `Password`=@dPs", new { Email, dPs });

                if (id != 0)
                {
                    var rand = new Random();
                    int cd = rand.Next(1000000, 1000000000);
                    connection.Execute("UPDATE `verkoper` SET `SessionID`=@cd WHERE `Email`=@Email AND `UserID`=@id AND `Password`=@dPs", new { cd, Email, id, dPs });
                    HttpContext.Session.SetString("uid", "" + id);
                    HttpContext.Session.SetString("sessionid", "" + cd);
                }
            }
            return RedirectToPage("Index");
        }



    }
}

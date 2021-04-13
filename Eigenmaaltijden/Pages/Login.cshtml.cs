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

        Database db = Database.get();


        public void OnGet()
        {

        }

        /// <summary>
        /// Asks database to login user.
        /// </summary>
        /// <returns>page redirection depending on results.</returns>
        public IActionResult OnPostLogin()
        {
            if (Password != null && Email != null)
            {
                //var dPs = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Password));
                var dPs = Password;
                using var connection = db.Connect();

                int[] id = connection.Query<int>("SELECT `UserID` FROM `verkoper` WHERE `Email`=@Email AND `Password`=@dPs", new { Email, dPs }).ToArray();
                int intid = (id.Length == 0 ? 0 : id[0]);

                if (intid != 0)
                {
                    var rand = new Random();
                    int cd = rand.Next(1000000, 1000000000);
                    connection.Execute("UPDATE `verkoper` SET `SessionID`=@cd WHERE `Email`=@Email AND `UserID`=@id AND `Password`=@dPs", new { cd, Email, id, dPs });
                    HttpContext.Session.SetString("uid", "" + intid);
                    HttpContext.Session.SetString("sessionid", "" + cd);
                }
            }
            return RedirectToPage("Index");


        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Eigenmaaltijden.wwwroot.includes;

namespace EigenMaaltijd.Pages
{
    public class AddMeal : PageModel
    {
        private readonly ILogger<AddMeal> _logger;

        Database db = new Database();
        Manager _manager = new Manager();
        public bool isLoggedIn { get; set; }

        public AddMeal(ILogger<AddMeal> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Checks if user is logged in.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            isLoggedIn = db.loginCheck(HttpContext.Session.GetString("sessionid"), HttpContext.Session.GetString("uid"));
            if (!isLoggedIn)
                return RedirectToPage("/Login");
            return null;
        }

        public IActionResult OnPost() {
            Console.WriteLine(Request.Form["name"]);
            if (!this._manager.ValidateMealName(Request.Form["name"]))
                return null;
            this._manager.SaveToDatabase(this._manager.Parse(Request.Form), int.Parse(HttpContext.Session.GetString("uid")));
            return null;
        }
    }
}

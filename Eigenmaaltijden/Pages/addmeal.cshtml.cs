using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Eigenmaaltijden.wwwroot.includes;
using Microsoft.AspNetCore.Http;


namespace EigenMaaltijd.Pages
{
    public class AddMeal : PageModel
    {
        private readonly ILogger<AddMeal> _logger;

        Database db = new Database();
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
    }
}

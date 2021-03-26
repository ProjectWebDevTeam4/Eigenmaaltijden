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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public Database db = Database.get();
        public bool isLoggedIn { get; set; }


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            isLoggedIn = db.loginCheck(HttpContext.Session.GetString("sessionid"), HttpContext.Session.GetString("uid"));
        }

        public string getName()
        {
            return db.GetLoggedInUser().Name;
        }

        public IActionResult OnGetLogout()
        {
            db.LogoutUser();
            HttpContext.Session.Clear();

            return RedirectToPage("Index");
        }
    }
}

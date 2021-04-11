using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
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

        public class Maaltijd {
            public string url {get; set;}
            public string Name {get; set;}
            public string Image {get; set;}
            
        }
        public List<Maaltijd> maaltijd = new List<Maaltijd>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            isLoggedIn = db.loginCheck(HttpContext.Session.GetString("sessionid"), HttpContext.Session.GetString("uid"));
            GetMaaltijden();
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

        public void GetMaaltijden()
        {
            using var connection = db.Connect();
            
            var maaltijden = connection.Query("SELECT m.MealID, m.Name, m.PhotoPath FROM maaltijden m INNER JOIN maaltijd_info i ON m.MealID=i.MealID WHERE i.Availability=2 ORDER BY RAND() LIMIT 5");
            
            if (maaltijden.AsList().Count > 0)
            {
                foreach (var maal in maaltijden)
                {
                    var list = new Maaltijd();

                    list.url = $"/meal?meal={maal.MealID}";
                    list.Name = maal.Name;
                    list.Image = maal.PhotoPath;
                    
                    maaltijd.Add(list);
                }
            }
        }
    }
}

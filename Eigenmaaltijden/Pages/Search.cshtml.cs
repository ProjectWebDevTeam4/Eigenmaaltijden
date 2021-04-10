using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Eigenmaaltijden.wwwroot.includes;
using Eigenmaaltijden.wwwroot.classes;

namespace Eigenmaaltijden.Pages
{
    public class SearchModel : PageModel
    {
        public void OnGet()
        {
        }

        public List<Preview> mealsSearchResult { get; set; }

        Database db = Database.get();
        Manager _manager = new Manager();

        public void OnPostSearch(string searchValue)
        {
            mealsSearchResult = _manager.GetMealsByName(searchValue);
        }
            
    }
}

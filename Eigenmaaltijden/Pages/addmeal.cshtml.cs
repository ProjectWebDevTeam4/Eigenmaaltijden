using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Eigenmaaltijden.wwwroot.includes;
using Eigenmaaltijden.wwwroot.classes;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EigenMaaltijd.Pages
{
    public class AddMeal : PageModel
    {
        private readonly ILogger<AddMeal> _logger;

        Database db = new Database();
        Manager _manager = new Manager();
        public List<Preview> Previews;
        public MealForm meal;
        public SavedMeal save;
        public string url;
        public bool isLoggedIn { get; set; }
        public IFormFile uploadedImage { get; set; }
        private readonly IWebHostEnvironment _environment;

        public AddMeal(ILogger<AddMeal> logger, IWebHostEnvironment env) {
            _logger = logger;
            _environment = env;
        }

        private void setMealUpdate() {
            int mealid = 0;
            string query = Request.QueryString.ToString();
            switch (Request.QueryString.HasValue) {
                case true:
                    if (query.Length <= 1 || query.Split("=")[1].Length == 0) 
                        return;
                    mealid = int.Parse(query.Split("=")[1]);
                    break;
                case false:
                    return;
            }
            this.save = this._manager.GetMeal(mealid);
            return;
        }

        /// <summary>
        /// Checks if user is logged in.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet() {
            isLoggedIn = db.loginCheck(HttpContext.Session.GetString("sessionid"), HttpContext.Session.GetString("uid"));
            if (!isLoggedIn)
                return RedirectToPage("/Login");
            this.Previews = this._manager.GetMealPreviews(int.Parse(HttpContext.Session.GetString("uid")));
            this.setMealUpdate();
            return null;
        }

        public async Task OnPostAsync() {
            // if (!this._manager.ValidateMealName(Request.Form["name"]))
            //     return;
            Console.WriteLine(_environment.WebRootPath);
            var exportPath = Path.Combine(_environment.WebRootPath, "uploads", uploadedImage.FileName);
            using(Stream fileStream = new FileStream(exportPath, FileMode.Create))
                await uploadedImage.CopyToAsync(fileStream);
            this._manager.SaveToDatabase(this._manager.Parse(Request.Form, exportPath), int.Parse(HttpContext.Session.GetString("uid")));
            this.Previews = this._manager.GetMealPreviews(int.Parse(HttpContext.Session.GetString("uid")));
            return;
        }
    }
}

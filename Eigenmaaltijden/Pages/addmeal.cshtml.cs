using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Eigenmaaltijden.wwwroot.classes;
using Eigenmaaltijden.wwwroot.includes;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EigenMaaltijd.Pages
{
    public class AddMeal : PageModel
    {
        private readonly ILogger<AddMeal> _logger;

        Database db = Database.get();
        Manager _manager = new Manager();
        public List<Preview> Previews;
        public SavedMeal save;
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
            Console.WriteLine(this.save.Description);
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
            this.Previews = this._manager.GetMealPreviews(int.Parse(HttpContext.Session.GetString("uid")));
            string[] extensions = { "png", "jpg", "jpeg", "svg" };
            if (!extensions.Contains(uploadedImage.FileName.Split(".")[1]))
                return;
            var exportPath = Path.Combine(_environment.WebRootPath, "uploads", uploadedImage.FileName);
            using(Stream fileStream = new FileStream(exportPath, FileMode.Create))
                await uploadedImage.CopyToAsync(fileStream);
            this._manager.SaveToDatabase(this._manager.Parse(Request.Form, "/uploads/" + uploadedImage.FileName), int.Parse(HttpContext.Session.GetString("uid")));
            this.Previews = this._manager.GetMealPreviews(int.Parse(HttpContext.Session.GetString("uid")));
            return;
        }
    }
}

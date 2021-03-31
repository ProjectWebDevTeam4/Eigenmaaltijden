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

        public IFormFile uploadedImage { get; set; }
        public List<Preview> Previews;
        public SavedMeal save;
        
        private readonly IWebHostEnvironment _environment;
        private bool isLoggedIn { get; set; }

        public AddMeal(ILogger<AddMeal> logger, IWebHostEnvironment env) {
            _logger = logger;
            _environment = env;
        }

        private int GetMealID() {
            if (Request.Query["meal"].ToString().Length == 0) {
                return -1;
            }
            return int.Parse(Request.Query["meal"]);
        }

        private void initializeMealUpdate() {
            this.save = this._manager.GetMeal(_environment.WebRootPath, this.GetMealID());
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
            if (this.GetMealID() != -1) this.initializeMealUpdate();
            return null;
        }

        public async Task OnPostAsync() {
            string[] extensions = { "png", "jpg", "jpeg", "svg" };
            if (!(uploadedImage is null) && !extensions.Contains(uploadedImage.FileName.Split(".")[1])) {
                this.Previews = this._manager.GetMealPreviews(int.Parse(HttpContext.Session.GetString("uid"))); // Setting the Previews.
                return;
            }
            string fileName = (uploadedImage is null) ? "default.svg" : uploadedImage.FileName;
            if (this.GetMealID() != -1)
                this._manager.UpdateToDatabase(this._manager.Parse(Request.Form, "/uploads/" + fileName), this.GetMealID());
            else
                this._manager.SaveToDatabase(this._manager.Parse(Request.Form, "/uploads/" + fileName), int.Parse(HttpContext.Session.GetString("uid")));
            if (fileName != "default.svg") {
                var exportPath = Path.Combine(_environment.WebRootPath, "uploads", uploadedImage.FileName);
                using(Stream fileStream = new FileStream(exportPath, FileMode.Create))
                    await uploadedImage.CopyToAsync(fileStream);
            }
            this.Previews = this._manager.GetMealPreviews(int.Parse(HttpContext.Session.GetString("uid"))); // Setting the Previews.
            return;
        }
    }
}

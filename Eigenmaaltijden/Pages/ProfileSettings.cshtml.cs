using System;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Eigenmaaltijden.wwwroot.includes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;


namespace Eigenmaaltijden.Pages
{
    public class ProfileSettings : PageModel
    {
        [BindProperty]
        public string Name { get; set; }
        
        [BindProperty]
        public string PhoneNumber { get; set; }
        
        [BindProperty]
        public string Description { get; set; }
        
        [BindProperty]
        public string ProfilePhotoPath { get; set; }
        
        public IFormFile uploadedImage { get; set; }
        private readonly IWebHostEnvironment _environment;
        
        Database db = new wwwroot.includes.Database();

        public ProfileSettings(IWebHostEnvironment env) {
            _environment = env;
        }

        public void GetData()
        {
            using var connection = db.Connect();
            var profile = connection.QuerySingle("SELECT * FROM verkoper_profiel WHERE UserID = 1");

            Name = profile.Name;
            PhoneNumber = profile.PhoneNumber;
            Description = profile.Description;
            ProfilePhotoPath = profile.ProfilePhotoPath;
        }

        public void OnPostProfileSettings()
        {
            Name = Request.Form["username"];
            Description = Request.Form["about"];

            var connection = db.Connect();

            connection.Execute("UPDATE verkoper_profiel SET Name = @NAME, Description = @DESCRIPTION WHERE UserID = 1", new
            {
                NAME = Name,
                PHONE = PhoneNumber,
                DESCRIPTION = Description
            });
        }
        public async Task OnPostAsync() {
            var exportPath = Path.Combine(_environment.WebRootPath, "uploads", uploadedImage.FileName);
            using(Stream fileStream = new FileStream(exportPath, FileMode.Create))
                await uploadedImage.CopyToAsync(fileStream);
        }
    }
}
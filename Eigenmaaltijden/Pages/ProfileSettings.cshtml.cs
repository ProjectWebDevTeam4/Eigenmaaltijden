using System;
using Dapper;
using Eigenmaaltijden.wwwroot.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        
        Database db = new wwwroot.includes.Database();


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
            PhoneNumber = Request.Form["phonenumber"];
            Description = Request.Form["about"];

            var connection = db.Connect();

            connection.Execute("UPDATE verkoper_profiel SET Name = @NAME, PhoneNumber = @PHONE, Description = @DESCRIPTION WHERE UserID = 1", new
            {
                NAME = Name,
                PHONE = PhoneNumber,
                DESCRIPTION = Description
            });
        }
        public void OnGet()
        {
            
        }
    }
}
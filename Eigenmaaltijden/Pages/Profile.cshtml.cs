using System;
using System.Collections.Generic;
using Dapper;
using Eigenmaaltijden.wwwroot.includes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eigenmaaltijden.Pages
{
    public class Profile : PageModel
    {
        
        [BindProperty]
        public string Pfp { get; set; }
        
        [BindProperty]
        public string Name { get; set; }
        
        [BindProperty]
        public string City { get; set; }
        
        [BindProperty]
        public string Email { get; set; }
        
        [BindProperty]
        public string Phone { get; set; }
        
        [BindProperty]
        public string Intro { get; set; }
        
        public class Maaltijd {
            public int ID {get; set;}
            public string Name {get; set;}
            public string Image {get; set;}
            
        }
        public List<Maaltijd> maaltijd = new List<Maaltijd>();
        
        public List<Maaltijd> verwachttemaaltijd = new List<Maaltijd>();


        Database db = new wwwroot.includes.Database();

        public void GetData()
        {
            using var connection = db.Connect();
            var profile = connection.QuerySingle("SELECT verkoper.Email, verkoper_adres.City, verkoper_profiel.* FROM `verkoper_profiel` INNER JOIN verkoper ON verkoper.UserID = verkoper_profiel.UserID INNER JOIN verkoper_adres ON verkoper_adres.UserID = verkoper_profiel.UserID WHERE verkoper.UserID = 1");

            Pfp = profile.ProfilePhotoPath;
            Name = profile.Name;
            City = profile.City;
            Email = profile.Email;
            Phone = profile.PhoneNumber;
            Intro = profile.Description;

            var maaltijden = connection.Query("SELECT maaltijden.* FROM `maaltijden` INNER JOIN maaltijd_info ON maaltijd_info.MealID = maaltijden.MealID WHERE maaltijden.UserID = 1 AND maaltijd_info.PreparedOn <= CAST(NOW() AS date)");
            
            if (maaltijden.AsList().Count > 0)
            {
                foreach (var maal in maaltijden)
                {
                    var list = new Maaltijd();

                    list.ID = Convert.ToInt32(maal.MealID);
                    list.Name = maal.Name;
                    list.Image = maal.PhotoPath;
                    
                    maaltijd.Add(list);
                }
            }
            
            var verwachttemaaltijden = connection.Query("SELECT maaltijden.* FROM `maaltijden` INNER JOIN maaltijd_info ON maaltijd_info.MealID = maaltijden.MealID WHERE maaltijden.UserID = 1 AND maaltijd_info.PreparedOn > CAST(NOW() AS date)");

            
            if (maaltijden.AsList().Count > 0)
            {
                foreach (var maal in verwachttemaaltijden)
                {
                    var list = new Maaltijd();

                    list.ID = Convert.ToInt32(maal.MealID);
                    list.Name = maal.Name;
                    list.Image = maal.PhotoPath;
                    
                    verwachttemaaltijd.Add(list);
                }
            }
        }
        
        public void OnGet()
        {
            
        }
    }
}
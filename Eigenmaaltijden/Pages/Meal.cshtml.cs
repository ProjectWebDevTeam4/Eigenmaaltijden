using System;
using Dapper;
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

namespace Eigenmaaltijden.Pages {
    public class Meal : PageModel {

        private readonly ILogger<Meal> _logger;
        Database db = Database.get();
        Manager _manager = new Manager();

        public SaveCollection currentMeal;
        public List<String> seller;

        public Meal(ILogger<Meal> logger) {
            _logger = logger;
        }

        private int GetMealID() {
            if (Request.Query["meal"].ToString().Length == 0) {
                return -1;
            }
            return int.Parse(Request.Query["meal"]);
        }

        private List<string> GetSeller(int mealid) {
            using var connection = db.Connect();
            int userid = connection.QuerySingle<int>("SELECT UserID FROM maaltijden WHERE MealID=@id", new { id=mealid });
            SellerData sellerData = connection.QuerySingle<SellerData>("SELECT UserID, Name FROM verkoper_profiel WHERE UserID=@id", new { id=userid });
            string urlToRedirect = $"/profile?id={sellerData.UserID}";
            return new List<string>{sellerData.Name, urlToRedirect};
        }

        private SaveCollection GetCurrentmeal(int mealid) {
            using var connection = db.Connect();
            CurrentMeal currentMeal = connection.QuerySingle<CurrentMeal>("SELECT * FROM maaltijden m INNER JOIN maaltijd_info i ON m.MealID=i.MealID WHERE m.MealID=@id", new { id=mealid});
            var ingredients = connection.Query<string>("SELECT Ingredient FROM maaltijd_ingredienten WHERE MealID=@mealid", new { mealid });
            return new SaveCollection(
                currentMeal.Name, 
                currentMeal.Description, 
                currentMeal.PhotoPath,
                this._manager.DisplayIngredients(ingredients),
                (currentMeal.Fresh == 0) ? "Frozen" : "Fresh",
                currentMeal.Type, 
                currentMeal.PreparedOn.ToString("yyyy-MM-dd"), 
                currentMeal.AmountAvailable, 
                currentMeal.PortionWeight, 
                currentMeal.PortionPrice, 
                currentMeal.Availability
            );
        }

        public string TypeToString(int category) {
            switch (category) {
                case 0:
                    return "Veganistische Maaltijd";
                case 1:
                    return "Vegetarische Maaltijd";
                case 2:
                    return "Vlees Maaltijd";
            }
            return null;
        }

        public IActionResult OnGet() {
            if (this.GetMealID() == -1)
                return RedirectToPage("/Index");
            this.currentMeal = this.GetCurrentmeal(this.GetMealID());
            this.seller = this.GetSeller(this.GetMealID());
            return null;
        }
    }
}
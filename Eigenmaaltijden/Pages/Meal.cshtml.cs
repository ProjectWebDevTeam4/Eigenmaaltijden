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
using SendGrid;
using SendGrid.Helpers.Mail;

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

        public async Task Execute()
        {
            SaveCollection buyer = GetCurrentmeal(GetMealID());
            var client = new SendGridClient("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");
            var from = new EmailAddress("danisteunebrink@live.nl", "Dsteunebrink");
            var to = new EmailAddress("mathijs.hoving@student.nhlstenden.com", "Mhoving");
            var subject = "Er is een bestelling gemaakt!";
            var plainTextContent = "Beste Hobby Kok," + Environment.NewLine + Environment.NewLine +
                "Er is een bestelling geplaatst op de maaltijd " + buyer.Name + ". " + Environment.NewLine +
                "Deze maaltijd zal op" + buyer.Availability + " beschikbaar komen. " + Environment.NewLine +
                "Het e-mailadres van de besteller is danisteunebrink@live.nl deze heeft een portie besteld. " + Environment.NewLine + 
                "Met het e-mailadres van de besteller kan je aangeven in welk tijdslot deze zijn portie mag ophalen." + Environment.NewLine + Environment.NewLine +
                "Met vriendelijke groet," + Environment.NewLine +
                "Eigemaaltijd Team!";
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                htmlContent
            );

            var response = await client.SendEmailAsync(msg);

            from = new EmailAddress("danisteunebrink@live.nl", "Dsteunebrink");
            to = new EmailAddress("mathijs.hoving@student.nhlstenden.com", "Mhoving");
            subject = "Je bestelling is ontvangen!";
            plainTextContent = "Beste Mathijs," + Environment.NewLine + Environment.NewLine +
                "Je bestelling is ontvangen door de kok. Je hebt de maaltijd " + buyer.Name + " besteld. " + Environment.NewLine +
                "Deze zal zo snel mogelijk de bestelling klaar maken zodat je de kan ophalen. "  + Environment.NewLine  +
                "De prijs van je maaltijd zal " + buyer.Price + " zijn. " +
                "De kok zal aangeven waarneer je deze kan ophalen." + Environment.NewLine + Environment.NewLine +
                "Met vriendelijke groet," + Environment.NewLine +
                "Eigemaaltijd Team!";
            htmlContent = "";
            msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                htmlContent
            );

            response = await client.SendEmailAsync(msg);
        }

       public void OnPost() { 
            Execute().Wait();
            this.currentMeal = this.GetCurrentmeal(this.GetMealID());
            this.seller = this.GetSeller(this.GetMealID());
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
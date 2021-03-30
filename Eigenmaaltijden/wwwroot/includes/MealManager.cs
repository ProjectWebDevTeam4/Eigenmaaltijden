using System;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Eigenmaaltijden.wwwroot.classes;

namespace Eigenmaaltijden.wwwroot.includes {
    
    public class Manager {

        private MealForm _mealForm;
        
        public Manager() { }
        
        public MealForm Parse(IFormCollection postMethodData, string imagePath) {
            int frozen = 0; // Standard False
            int category = 0; // Standard Veganistisch
            int availability = 0;
            string[] keys = { "name", "desc", "ingredients", "frozen", "options", "date", "amount", "weight", "price", "saveoptions" };
            if (postMethodData[keys[3]] == "true")
                frozen = 1;
            switch(postMethodData[keys[4]]) {
                case "Veganisme Maaltijd":
                    category = 0;
                    break;
                case "Vegetarische Maaltijd":
                    category = 1;
                    break;
                case "Vlees Maaltijd":
                    category = 2;
                    break;
            }
            switch(postMethodData[keys[9]]) {
                case "Bewaren":
                    availability = 0;
                    break;
                case "Bewaren en Verwacht":
                    availability = 1;
                    break;
                case "Bewaren en Publiceren":
                    availability = 2;
                    break;
            }
            this._mealForm = new MealForm(postMethodData[keys[0]], postMethodData[keys[1]], imagePath, postMethodData[keys[2]], frozen, category, Convert.ToDateTime(postMethodData[keys[5]]).Date.ToString("yyyy-MM-dd"), int.Parse(postMethodData[keys[6]]), int.Parse(postMethodData[keys[7]]), float.Parse(postMethodData[keys[8]]), availability);
            return this._mealForm;
        }

        public List<Preview> GetMealPreviews(int uid) {
            var connection = this.Connect();
            List<Preview> mealsPreview = new List<Preview>();
            var listOfMeals = connection.Query<Meals>("SELECT * FROM maaltijden WHERE UserID=@uid", new { uid });
            foreach(var meal in listOfMeals)
                mealsPreview.Add(new Preview($"/addmeal?meal={meal.MealID}", meal.Name, meal.PhotoPath));
            return mealsPreview;
        }

        public SavedMeal GetMeal(int mealid) {
            var connection = this.Connect();
            var currentMeal = connection.QuerySingle<Meals>("SELECT * FROM maaltijden WHERE MealID=@mealid", new { mealid });
            var currentMealInfo = connection.QuerySingle<MealInfo>("SELECT * FROM maaltijd_info WHERE MealID=@mealid", new { mealid });
            string fresh = "checked";
            string category = "";
            string availability = "";
            if (currentMealInfo.Fresh == 1)
                fresh = "unchecked";
            switch(currentMealInfo.Type) {
                case 0:
                    category = "Veganisme Maaltijd";
                    break;
                case 1:
                    category = "Vegetarische Maaltijd";
                    break;
                case 2:
                    category = "Vlees Maaltijd";
                    break;
            }
            switch(currentMealInfo.Availability) {
                case 0:
                    availability = "Bewaren";
                    break;
                case 1:
                    availability = "Bewaren en Verwacht";
                    break;
                case 2:
                    availability = "Bewaren en Publiceren";
                    break;
            }
            SavedMeal save = new SavedMeal(currentMeal.Name, currentMeal.Description, currentMeal.PhotoPath, fresh, category, currentMealInfo.PreparedOn.ToString("yyyy-MM-dd"), currentMealInfo.AmountAvailable, currentMealInfo.PortionWeight, currentMealInfo.PortionPrice, availability);
            return save;
        }

        private IDbConnection Connect() {
            return new MySqlConnection(@"Server=localhost;Port=3306;Database=eigenmaaltijden;Uid=root;Pwd=;");
        }

        public bool ValidateMealName(string name) {
            var connection = this.Connect();
            try {
                string result = connection.QuerySingle<string>("SELECT Name FROM maaltijden WHERE Name=@name", new { name });
            } catch(InvalidOperationException err) {
                return false;
            }
            return true;
        }

        private int getMealID(int uid, string name) {
            var connection = this.Connect();
            int mealid = connection.QuerySingle<int>("SELECT MealID FROM maaltijden WHERE UserID=@uid AND Name=@name", new { uid, name });
            connection.Close();
            return mealid;
        }

        public void SaveToDatabase(MealForm meal, int uid) {
            var connection = this.Connect();
            connection.Execute("INSERT INTO maaltijden (UserID, Name, Description, PhotoPath) VALUES (@uid, @meal.Name, @meal.Description, @meal.ImagePath)", new { uid, meal.Name, meal.Description, meal.ImagePath });
            int mealid = this.getMealID(uid, meal.Name);
            string queryToMaaltijdenInfo = $"INSERT INTO maaltijd_info (MealID, AmountAvailable, Type, PortionPrice, PortionWeight, Fresh, PreparedOn, Availability) VALUES (@mealid, @meal.Amount, @meal.Category, @meal.Price, @meal.Weight, @meal.Frozen, @meal.Date, @meal.Availability)";
            string queryToIngredients = $"INSERT INTO maaltijd_ingredienten (MealID, Ingredient) VALUES (@mealid, @meal.Ingredients)";
            connection.Execute("INSERT INTO maaltijd_info (MealID, AmountAvailable, Type, PortionPrice, PortionWeight, Fresh, PreparedOn, Availability) VALUES (@mealid, @meal.Amount, @meal.Category, @meal.Price, @meal.Weight, @meal.Frozen, @meal.Date, @meal.Availability)", new { mealid, meal.Amount, meal.Category, meal.Price, meal.Weight, meal.Frozen, meal.Date, meal.Availability });
            connection.Execute($"INSERT INTO maaltijd_ingredienten (MealID, Ingredient) VALUES (@mealid, @meal.Ingredients)", new { mealid, meal.Ingredients });
        }
    }
}  
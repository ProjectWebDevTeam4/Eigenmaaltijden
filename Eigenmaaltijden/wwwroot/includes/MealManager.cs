using System;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Eigenmaaltijden.wwwroot.classes;

namespace Eigenmaaltijden.wwwroot.includes {
    
    public class Manager {

        private Database db = Database.get();
        
        public Manager() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public MealForm Parse(IFormCollection collection, string imagePath) {
            int fresh = 0; // Standard False
            return new MealForm(
                collection["name"], 
                collection["desc"], 
                imagePath,
                collection["ingredients"],
                fresh = (collection["fresh"] == "true") ? 1 : 0,
                int.Parse(collection["category"]), 
                Convert.ToDateTime(collection["date"]).Date.ToString("yyyy-MM-dd"),
                int.Parse(collection["amount"]), 
                int.Parse(collection["weight"]), 
                float.Parse(collection["price"]), 
                int.Parse(collection["saveoptions"])
            );
        }

        /// <summary>
        /// This method creates a list of previews for already saved meals.
        /// </summary>
        /// <param name="uid">The UserID of the current User</param>
        /// <returns>A list of Preview instances</returns>
        public List<Preview> GetMealPreviews(int uid) {
            using var connection = db.Connect();
            List<Preview> mealsPreview = new List<Preview>();
            var listOfMeals = connection.Query<Meals>("SELECT * FROM maaltijden WHERE UserID=@uid", new { uid });
            foreach(var meal in listOfMeals)
                mealsPreview.Add(new Preview($"/addmeal?meal={meal.MealID}", meal.Name, meal.PhotoPath));
            return mealsPreview;
        }

        // public List<SearchPreview> GetMealPreviews(string searchKeyWord) {

        // }

        /// <summary>
        /// Creates SavedMeal instance for updating already made meals.
        /// </summary>
        /// <param name="mealid">The ID of the meal request</param>
        /// <returns>A SavedMeal instance</returns>
        public SavedMeal GetMeal(string wwwroot, int mealid) {
            using var connection = db.Connect();
            string fresh = "";
            var currentMeal = connection.QuerySingle<Meals>("SELECT * FROM maaltijden WHERE MealID=@mealid", new { mealid });
            var currentMealInfo = connection.QuerySingle<MealInfo>("SELECT * FROM maaltijd_info WHERE MealID=@mealid", new { mealid });
            return new SavedMeal(
                currentMeal.Name, 
                currentMeal.Description, 
                wwwroot + currentMeal.PhotoPath,
                currentMeal.PhotoPath, 
                fresh = (currentMealInfo.Fresh == 0) ? "checked" : "unchecked",
                currentMealInfo.Type, 
                currentMealInfo.PreparedOn.ToString("yyyy-MM-dd"), 
                currentMealInfo.AmountAvailable, 
                currentMealInfo.PortionWeight, 
                currentMealInfo.PortionPrice, 
                currentMealInfo.Availability
            );
        }

        /// <summary>
        /// Work in progress
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool ValidateMealName(string name) {
            using var connection = db.Connect();
            try {
                string result = connection.QuerySingle<string>("SELECT Name FROM maaltijden WHERE Name=@name", new { name });
            } catch(InvalidOperationException err) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the MealID with from the given Meal Name
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private int getMealID(int uid, string name) {
            using var connection = db.Connect();
            int mealid = connection.QuerySingle<int>("SELECT MealID FROM maaltijden WHERE UserID=@uid AND Name=@name", new { uid, name });
            connection.Close();
            return mealid;
        }

        public void UpdateToDatabase(MealForm meal, int mealid) {
            using var connection = db.Connect();
            connection.Execute("UPDATE maaltijden SET Name=@name, Description=@description, PhotoPath=@photopath WHERE MealID=@id", new { 
                name = meal.Name,
                description = meal.Description,
                photopath = meal.ImagePath,
                id = mealid
            });
            connection.Execute("UPDATE maaltijd_info SET AmountAvailable=@amount, Type=@type, PortionPrice=@price, PortionWeight=@weight, Fresh=@fresh, PreparedOn=@date, Availability=@availability WHERE MealID=@id", new {
                amount = meal.Amount,
                type = meal.Category,
                price = meal.Price,
                weight = meal.Weight,
                fresh = meal.Fresh,
                date = meal.Date,
                availability = meal.Availability,
                id = mealid
            });
        }

        /// <summary>
        /// Inserts the formcollection data into the database
        /// </summary>
        /// <param name="meal">A struct with the collection data</param>
        /// <param name="uid">The UserID of the current User</param>
        public void SaveToDatabase(MealForm meal, int uid) {
            using var connection = db.Connect();
            connection.Execute("INSERT INTO maaltijden (UserID, Name, Description, PhotoPath) VALUES (@uid, @name, @description, @imagePath)", new { 
                uid, 
                name = meal.Name, 
                description = meal.Description, 
                imagePath = meal.ImagePath 
            });
            int mealid = this.getMealID(uid, meal.Name);
            connection.Execute("INSERT INTO maaltijd_info (MealID, AmountAvailable, Type, PortionPrice, PortionWeight, Fresh, PreparedOn, Availability) VALUES (@mealid, @amount, @category, @price, @weight, @frozen, @date, @availability)", new { 
                mealid, 
                amount = meal.Amount, 
                category = meal.Category, 
                price = meal.Price, 
                weight = meal.Weight, 
                frozen = meal.Fresh, 
                date = meal.Date, 
                availability = meal.Availability 
            });
            connection.Execute($"INSERT INTO maaltijd_ingredienten (MealID, Ingredient) VALUES (@mealid, @ingredients)", new { mealid, ingredients = meal.Ingredients });
        }
    }
}  
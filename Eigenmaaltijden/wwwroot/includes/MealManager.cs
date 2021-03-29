using Dapper;
using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace Eigenmaaltijden.wwwroot.includes {

    public struct Meal {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Ingredients { get; set; }
        public int Frozen { get; set; }
        public int Category { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
        public float Price { get; set; }

        public Meal(string name, string description, string imagePath, string ingredients, int frozen, int category, string date, int amount, int weight, float price) {
            this.Name = name; this.Description = description; this.ImagePath = imagePath; this.Ingredients = ingredients; this.Frozen = frozen; this.Category = category; this.Date = date; this.Amount = amount; this.Weight = weight; this.Price = price;
        }
    }
    
    public class Manager {

        private Meal _meal;
        
        public Manager() {

        }
        
        public Meal Parse(IFormCollection postMethodData) {
            int frozen = 0; // Standard False
            int category = 0; // Standard False
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
            this._meal = new Meal(postMethodData[keys[0]], postMethodData[keys[1]], "HelloItsMe", postMethodData[keys[2]], frozen, category, Convert.ToDateTime(postMethodData[keys[5]]).Date.ToString("yyyy-MM-dd"), int.Parse(postMethodData[keys[6]]), int.Parse(postMethodData[keys[7]]), float.Parse(postMethodData[keys[8]]));
            return this._meal;
        }

        private IDbConnection Connect() {
            return new MySqlConnection(@"Server=localhost;Port=3306;Database=eigenmaaltijden;Uid=root;Pwd=NEWPASSWORD;");
        }

        public bool ValidateMealName(string name) {
            var connection = this.Connect();
            try {
                string result = connection.QuerySingle<string>($"SELECT Name FROM maaltijden WHERE Name='{name}'");
            } catch(InvalidOperationException err) {
                return false;
            }
            return true;
        }

        private int getMealID(int uid, string name) {
            var connection = this.Connect();
            int mealid = connection.QuerySingle<int>($"SELECT MealID FROM maaltijden WHERE UserID='{uid}' AND Name='{name}'");
            connection.Close();
            return mealid;
        }

        public void SaveToDatabase(Meal meal, int uid) {
            var connection = this.Connect();
            string queryToMaaltijden = $"INSERT INTO maaltijden (UserID, Name, Description, PhotoPath) VALUES ('{uid}', '{meal.Name}', '{meal.Description}', '{meal.ImagePath}')";
            connection.Execute(queryToMaaltijden);
            string queryToMaaltijdenInfo = $"INSERT INTO maaltijd_info (MealID, AmountAvailable, Type, PortionPrice, PortionWeight, Fresh, AvailableUntil, PreparedOn) VALUES ('{this.getMealID(uid, meal.Name)}', '{meal.Amount}', '{meal.Category}', '{meal.Price}', '{meal.Weight}', '{meal.Frozen}', '{meal.Date}', '{meal.Date}')";
            string queryToIngredients = $"INSERT INTO maaltijd_ingredienten (MealID, Ingredient) VALUES ('{this.getMealID(uid, meal.Name)}', '{meal.Ingredients}')";
            connection.Execute(queryToMaaltijdenInfo);
            connection.Execute(queryToIngredients);
        }
    }
}  
using System;

namespace Eigenmaaltijden.wwwroot.classes {

    public struct MealForm {
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
        public int Availability { get; set; }

        public MealForm(string name, string description, string imagePath, string ingredients, int frozen, int category, string date, int amount, int weight, float price, int availability) {
            this.Name = name; this.Description = description; this.ImagePath = imagePath; this.Ingredients = ingredients; this.Frozen = frozen; this.Category = category; this.Date = date; this.Amount = amount; this.Weight = weight; this.Price = price; this.Availability = availability;
        }
    }

    public struct Preview {
        public string Url { get; set;  }
        public string Name { get; set; }
        public string PhotoPath { get; set; }

        public Preview(string url, string name, string photoPath) {
            this.Url = url; this.Name = name; this.PhotoPath = photoPath;
        }
    }

    public struct SavedMeal {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Frozen { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
        public float Price { get; set; }
        public string Availability { get; set; }

        public SavedMeal(string name, string desc, string imagePath, string frozen, string category, string date, int amount, int weight, float price, string availability) {
            this.Name = name; this.Description = desc; this.ImagePath = imagePath; this.Frozen = frozen; this.Category = category; this.Date = date; this.Amount = amount; this.Weight = weight; this.Price = price; this.Availability = availability;
        }

    }
    
    public class Meals {
        public int MealID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
    }

    public class MealInfo {
        public int MealID { get; set; }
        public int AmountAvailable { get; set; }
        public int Type { get; set; }
        public float PortionPrice { get; set; }
        public int PortionWeight { get; set; }
        public int Fresh { get; set; }
        public DateTime PreparedOn { get; set; }
        public int Availability { get; set; }

        public MealInfo() { }
    }
}
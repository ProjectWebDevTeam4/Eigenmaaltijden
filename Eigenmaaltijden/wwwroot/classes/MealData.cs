using System;

namespace Eigenmaaltijden.wwwroot.classes {

    public struct MealCollection {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Ingredients { get; set; }
        public int Fresh { get; set; }
        public int Category { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
        public float Price { get; set; }
        public int Availability { get; set; }

        public MealCollection(string name, string description, string imagePath, string ingredients, int fresh, int category, string date, int amount, int weight, float price, int availability) {
            this.Name = name; this.Description = description; this.ImagePath = imagePath; this.Ingredients = ingredients; this.Fresh = fresh; this.Category = category; this.Date = date; this.Amount = amount; this.Weight = weight; this.Price = price; this.Availability = availability;
        }
    }

    public struct Preview {
        public string Url { get; set; }
        public string Name { get; set; }
        public string PhotoPath { get; set; }

        public string Description { get; set; }

        public Preview(string url, string name, string photoPath) {
            this.Url = url; this.Name = name; this.PhotoPath = photoPath; this.Description = "";
        }

        public Preview(string url, string name, string photoPath, string Description)
        {
            this.Url = url; this.Name = name; this.PhotoPath = photoPath; this.Description = Description;
        }
    }

    public struct SaveCollection {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AbsoluteImagePath { get; set; }
        public string ImagePath { get; set; }
        public string Ingredients { get; set; }
        public string Frozen { get; set; }
        public int Category { get; set; }
        public string Date { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
        public float Price { get; set; }
        public int Availability { get; set; }

        public SaveCollection(string name, string desc, string absoluteImagePath, string imagePath, string ingredients, string frozen, int category, string date, int amount, int weight, float price, int availability) {
            this.Name = name; this.Description = desc; this.AbsoluteImagePath = absoluteImagePath; this.ImagePath = imagePath; this.Ingredients = ingredients; this.Frozen = frozen; this.Category = category; this.Date = date; this.Amount = amount; this.Weight = weight; this.Price = price; this.Availability = availability;
        }

    }
    
    public class CurrentMeal {
        public int MealID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public int AmountAvailable { get; set; }
        public int Type { get; set; }
        public float PortionPrice { get; set; }
        public int PortionWeight { get; set; }
        public int Fresh { get; set; }
        public DateTime PreparedOn { get; set; }
        public int Availability { get; set; }
    }
}
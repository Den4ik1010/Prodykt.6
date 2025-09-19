using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Bogus;

namespace ProductManagerApp
{
    // 1. Клас Product
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }

    // 2. Клас ProductManager
    public class ProductManager
    {
        public List<Product> Products { get; private set; } = new();

        // Метод генерації продуктів
        public void GenerateProducts(int count)
        {
            var faker = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0]);

            Products = faker.Generate(count);
        }

        // Метод збереження у файл
        public void SaveToFile(string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(Products, options);
            File.WriteAllText(filePath, json);
        }
    }

    // 3. Головний клас програми
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new ProductManager();
            manager.GenerateProducts(50); // Генеруємо 50 продуктів

            string filePath = "products.json";
            manager.SaveToFile(filePath);

            Console.WriteLine($"Збережено {manager.Products.Count} продуктів у файл {filePath}");
        }
    }
}

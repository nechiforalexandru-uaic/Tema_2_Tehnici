using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogSearch
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<IProduct> _products;

        public ProductRepository()
        {
            _products = GenerateMockProducts();
        }

        public IEnumerable<IProduct> GetAllProducts()
        {
            return _products;
        }

        public IProduct GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        private List<IProduct> GenerateMockProducts()
        {
            var products = new List<IProduct>();
            var random = new Random();
            var categories = new[] { "Electronice", "Îmbrăcăminte", "Cărți", "Jucării", "Sport", "Cosmetice" };
            var productNames = new[]
            {
                "Laptop", "Telefon", "Căști", "Tricou", "Pantaloni", "Carte C#",
                "Rochie", "Geacă", "Mouse", "Tastatură", "Monitor", "Tabletă",
                "Păpușă", "Mașinuță", "Minge", "Cremă", "Șampon", "Parfum"
            };

            for (int i = 1; i <= 1000; i++)
            {
                var category = categories[random.Next(categories.Length)];
                var name = $"{productNames[random.Next(productNames.Length)]} {i}";
                var price = Math.Round((decimal)(random.NextDouble() * 1000 + 10), 2);
                var stock = random.Next(0, 100);
                var isAvailable = stock > 0;

                products.Add(new Product
                {
                    Id = i,
                    Name = name,
                    Category = category,
                    Price = price,
                    StockQuantity = stock,
                    IsAvailable = isAvailable,
                    CreatedDate = DateTime.Now.AddDays(-random.Next(1, 365))
                });
            }

            return products;
        }
    }
}
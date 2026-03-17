using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ProductCatalogSearch
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Catalog Produse - Sistem de Căutare LINQ ===\n");

            IProductRepository repository = new ProductRepository();
            IProductSearchService searchService = new ProductSearchService(repository);
            var stopwatch = new Stopwatch();

            Console.WriteLine("1. CăUTARE SIMPLĂ:");
            Console.WriteLine("   Produse din categoria 'Electronice' cu preț între 100 și 500:");

            stopwatch.Start();
            var filter = new ProductFilter
            {
                Category = "Electronice",
                MinPrice = 100,
                MaxPrice = 500,
                IsAvailable = true
            };

            var results = searchService.Search(filter).Take(10);
            stopwatch.Stop();

            foreach (var product in results)
            {
                Console.WriteLine($"   {product}");
            }
            Console.WriteLine($"   Timp căutare: {stopwatch.ElapsedMilliseconds} ms\n");

            Console.WriteLine("2. CăUTARE CU SORTARE (cele mai ieftine produse):");

            stopwatch.Restart();
            var sortedResults = searchService.SearchAndSort(
                new ProductFilter { IsAvailable = true },
                ProductSortBy.Price,
                SortOrder.Ascending
            ).Take(10);
            stopwatch.Stop();

            foreach (var product in sortedResults)
            {
                Console.WriteLine($"   {product}");
            }
            Console.WriteLine($"   Timp sortare: {stopwatch.ElapsedMilliseconds} ms\n");

            Console.WriteLine("3. GRUPARE PE CATEGORII:");

            stopwatch.Restart();
            var groupedByCategory = searchService.GroupByCategory(new ProductFilter { IsAvailable = true });
            stopwatch.Stop();

            foreach (var group in groupedByCategory.Take(5))
            {
                Console.WriteLine($"   Categorie: {group.Key} - {group.Count()} produse");
                Console.WriteLine($"   Preț mediu: {group.Average(p => p.Price):C}");
                Console.WriteLine($"   Stoc total: {group.Sum(p => p.StockQuantity)}");
                Console.WriteLine();
            }

            Console.WriteLine("4. STATISTICI GENERALE:");

            stopwatch.Restart();
            var stats = searchService.GetStatistics(new ProductFilter { IsAvailable = true });
            stopwatch.Stop();

            foreach (var stat in stats)
            {
                if (stat.Value is IEnumerable<IProduct> products)
                {
                    Console.WriteLine($"   {stat.Key}:");
                    foreach (var p in products)
                    {
                        Console.WriteLine($"      {p.Name} - {p.Price:C}");
                    }
                }
                else
                {
                    Console.WriteLine($"   {stat.Key}: {stat.Value}");
                }
            }
            Console.WriteLine($"   Timp statistici: {stopwatch.ElapsedMilliseconds} ms\n");

            Console.WriteLine("5. TOP 5 CELE MAI SCUMPE PRODUSE:");

            var topExpensive = searchService.GetTopProducts(5, p => p.Price);
            foreach (var product in topExpensive)
            {
                Console.WriteLine($"   {product}");
            }

            Console.WriteLine("\n6. CăUTARE COMPLEXĂ:");
            Console.WriteLine("   Produse care conțin 'Laptop' în nume, disponibile, cu stoc > 10:");

            var complexFilter = new ProductFilter
            {
                SearchTerm = "Laptop",
                IsAvailable = true,
                MinStock = 10
            };

            var complexResults = searchService.SearchAndSort(complexFilter, ProductSortBy.StockQuantity, SortOrder.Descending);

            foreach (var product in complexResults.Take(5))
            {
                Console.WriteLine($"   {product}");
            }

            Console.WriteLine("\n7. TEST PERFORMANȚĂ CU 1000 DE PRODUSE:");

            stopwatch.Restart();
            var allProducts = searchService.Search(new ProductFilter()).ToList();
            var count = allProducts.Count;
            stopwatch.Stop();
            Console.WriteLine($"   Total produse încărcate: {count}");
            Console.WriteLine($"   Timp încărcare: {stopwatch.ElapsedMilliseconds} ms");

            Console.WriteLine("\n=== APLICAȚIE FINALIZATĂ ===");
            Console.ReadKey();
        }
    }
}
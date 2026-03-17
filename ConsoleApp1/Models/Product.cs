using System;

namespace ProductCatalogSearch
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Name} | Categorie: {Category} | Preț: {Price:C} | Stoc: {StockQuantity} | Disponibil: {(IsAvailable ? "Da" : "Nu")} | Data: {CreatedDate:dd.MM.yyyy}";
        }
    }
}
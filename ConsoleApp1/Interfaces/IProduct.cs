using System;

namespace ProductCatalogSearch
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }
        string Category { get; set; }
        decimal Price { get; set; }
        int StockQuantity { get; set; }
        bool IsAvailable { get; set; }
        DateTime CreatedDate { get; set; }
    }
}
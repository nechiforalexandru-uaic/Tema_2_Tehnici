using System;

namespace ProductCatalogSearch
{
    public class ProductFilter
    {
        public string Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public string SearchTerm { get; set; }
        public int? MinStock { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ProductCatalogSearch
{
    public class ProductSearchService : IProductSearchService
    {
        private readonly IProductRepository _productRepository;

        public ProductSearchService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<IProduct> Search(ProductFilter filter)
        {
            Func<IProduct, bool> predicate = BuildFilterPredicate(filter);
            return _productRepository.GetAllProducts()
                .AsParallel()
                .Where(predicate)
                .ToList();
        }

        public IEnumerable<IProduct> SearchAndSort(ProductFilter filter, ProductSortBy sortBy, SortOrder order = SortOrder.Ascending)
        {
            var query = Search(filter).AsQueryable();
            var sortedQuery = SortQuery(query, sortBy, order);
            return sortedQuery.ToList();
        }

        public IEnumerable<IGrouping<string, IProduct>> GroupByCategory(ProductFilter filter)
        {
            var query = from product in Search(filter)
                        group product by product.Category into categoryGroup
                        select categoryGroup;

            return query.ToList();
        }

        public Dictionary<string, object> GetStatistics(ProductFilter filter)
        {
            var products = Search(filter).ToList();

            return new Dictionary<string, object>
            {
                ["TotalProduse"] = products.Count,
                ["PrețMediu"] = products.Average(p => p.Price),
                ["PrețMinim"] = products.Min(p => p.Price),
                ["PrețMaxim"] = products.Max(p => p.Price),
                ["StocTotal"] = products.Sum(p => p.StockQuantity),
                ["ProduseDisponibile"] = products.Count(p => p.IsAvailable),
                ["CategoriiUnice"] = products.Select(p => p.Category).Distinct().Count(),
                ["CeleMaiNoiProduse"] = products.OrderByDescending(p => p.CreatedDate).Take(3).ToList()
            };
        }

        public IEnumerable<IProduct> GetTopProducts(int count, Func<IProduct, decimal> orderBy)
        {
            return _productRepository.GetAllProducts()
                .OrderByDescending(orderBy)
                .Take(count)
                .ToList();
        }

        private Func<IProduct, bool> BuildFilterPredicate(ProductFilter filter)
        {
            return p =>
            {
                if (filter == null) return true;

                bool result = true;

                if (!string.IsNullOrWhiteSpace(filter.Category))
                {
                    result = result && p.Category == filter.Category;
                }

                if (filter.MinPrice.HasValue)
                {
                    result = result && p.Price >= filter.MinPrice.Value;
                }

                if (filter.MaxPrice.HasValue)
                {
                    result = result && p.Price <= filter.MaxPrice.Value;
                }

                if (filter.IsAvailable.HasValue)
                {
                    result = result && p.IsAvailable == filter.IsAvailable.Value;
                }

                if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                {
                    result = result && p.Name.IndexOf(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (filter.MinStock.HasValue)
                {
                    result = result && p.StockQuantity >= filter.MinStock.Value;
                }

                if (filter.FromDate.HasValue)
                {
                    result = result && p.CreatedDate >= filter.FromDate.Value;
                }

                if (filter.ToDate.HasValue)
                {
                    result = result && p.CreatedDate <= filter.ToDate.Value;
                }

                return result;
            };
        }

        private IQueryable<IProduct> SortQuery(IQueryable<IProduct> query, ProductSortBy sortBy, SortOrder order)
        {
            switch (sortBy)
            {
                case ProductSortBy.Name:
                    return order == SortOrder.Ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name);
                case ProductSortBy.Price:
                    return order == SortOrder.Ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                case ProductSortBy.Category:
                    return order == SortOrder.Ascending ? query.OrderBy(p => p.Category) : query.OrderByDescending(p => p.Category);
                case ProductSortBy.StockQuantity:
                    return order == SortOrder.Ascending ? query.OrderBy(p => p.StockQuantity) : query.OrderByDescending(p => p.StockQuantity);
                case ProductSortBy.CreatedDate:
                    return order == SortOrder.Ascending ? query.OrderBy(p => p.CreatedDate) : query.OrderByDescending(p => p.CreatedDate);
                default:
                    return order == SortOrder.Ascending ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id);
            }
        }
    }
}
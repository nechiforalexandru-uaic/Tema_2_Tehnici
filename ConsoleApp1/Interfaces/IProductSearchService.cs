using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogSearch
{
    public interface IProductSearchService
    {
        IEnumerable<IProduct> Search(ProductFilter filter);
        IEnumerable<IProduct> SearchAndSort(ProductFilter filter, ProductSortBy sortBy, SortOrder order = SortOrder.Ascending);
        IEnumerable<IGrouping<string, IProduct>> GroupByCategory(ProductFilter filter);
        Dictionary<string, object> GetStatistics(ProductFilter filter);
        IEnumerable<IProduct> GetTopProducts(int count, Func<IProduct, decimal> orderBy);
    }
}
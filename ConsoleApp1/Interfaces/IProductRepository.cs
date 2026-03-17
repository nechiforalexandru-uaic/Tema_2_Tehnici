using System.Collections.Generic;

namespace ProductCatalogSearch
{
    public interface IProductRepository
    {
        IEnumerable<IProduct> GetAllProducts();
        IProduct GetProductById(int id);
    }
}
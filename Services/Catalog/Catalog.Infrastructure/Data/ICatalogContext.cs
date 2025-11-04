using Catalog.Infrastructure.Documents;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;
public interface ICatalogContext
{
    IMongoCollection<ProductDocument> Products { get; }
    IMongoCollection<ProductBrandDocument> Brands { get; }
    IMongoCollection<ProductTypeDocument> Types { get; }
}

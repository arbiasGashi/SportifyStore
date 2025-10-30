using Catalog.Application.Sorting;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;
    private readonly ISortStrategyFactory _sortStrategyFactory;

    private static readonly Collation _caseInsensitiveCollation =
        new("en", strength: CollationStrength.Secondary);

    public ProductRepository(ICatalogContext context, ISortStrategyFactory sortStrategyFactory)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(sortStrategyFactory, nameof(sortStrategyFactory));
        _context = context;
        _sortStrategyFactory = sortStrategyFactory;
    }

    public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<Product>.Filter;
        var filter = builder.Empty;

        if (!string.IsNullOrEmpty(catalogSpecParams.Search))
        {
            // Flexible substring search (case-insensitive)
            var re = new BsonRegularExpression(catalogSpecParams.Search, "i");
            filter &= builder.Regex(p => p.Name, re);
        }

        if (!string.IsNullOrEmpty(catalogSpecParams.BrandId))
        {
            filter &= builder.Eq(p => p.Brand.Id, catalogSpecParams.BrandId);
        }

        if (!string.IsNullOrEmpty(catalogSpecParams.TypeId))
        {
            filter &= builder.Eq(p => p.Type.Id, catalogSpecParams.TypeId);
        }

        // Count with collation (for equality parts)
        var countOptions = new CountOptions
        {
            Collation = _caseInsensitiveCollation
        };
        var totalItems = await _context.Products.CountDocumentsAsync(filter, countOptions);

        var data = await DataFilter(catalogSpecParams, filter);

        return new Pagination<Product>(
            catalogSpecParams.PageIndex,
            catalogSpecParams.PageSize,
            (int)totalItems,
            data
        );
    }

    private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<Product> filter)
    {
        var sortBuilder = Builders<Product>.Sort;
        var strategy = _sortStrategyFactory.GetStrategy(catalogSpecParams.Sort ?? "name");
        var sortDefinition = strategy.ApplySort(sortBuilder);

        var aggregateOptions = new AggregateOptions
        {
            Collation = _caseInsensitiveCollation
        };

        var query = _context.Products
            .Aggregate(aggregateOptions)
            .Match(filter)
            .Sort(sortDefinition)
            .Skip(catalogSpecParams.PageSize * (catalogSpecParams.PageIndex - 1))
            .Limit(catalogSpecParams.PageSize);

        return await query.ToListAsync();
    }

    public async Task<Product> GetProduct(string id)
    {
        return await _context
            .Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Brand.Name, brandName);
        var options = new FindOptions<Product>
        {
            Collation = _caseInsensitiveCollation
        };

        using var cursor = await _context.Products.FindAsync(filter, options);

        return await cursor.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
        var options = new FindOptions<Product>
        {
            Collation = _caseInsensitiveCollation
        };

        using var cursor = await _context.Products.FindAsync(filter, options);

        return await cursor.ToListAsync();
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _context
            .Products
            .InsertOneAsync(product);

        return product;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        var deletedProduct = await _context
            .Products
            .DeleteOneAsync(p => p.Id == id);

        return deletedProduct.IsAcknowledged && deletedProduct.DeletedCount > 0;
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updatedProduct = await _context
            .Products
            .ReplaceOneAsync(p => p.Id == product.Id, product);

        return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
    }
}

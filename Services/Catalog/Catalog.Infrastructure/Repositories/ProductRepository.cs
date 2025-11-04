using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Documents;
using Catalog.Infrastructure.Sorting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly ICatalogContext _context;
    private readonly MongoSortStrategyFactory<ProductDocument> _sortStrategyFactory;
    private readonly IMapper _mapper;

    private static readonly Collation _caseInsensitiveCollation =
        new("en", strength: CollationStrength.Secondary);

    public ProductRepository(ICatalogContext context, MongoSortStrategyFactory<ProductDocument> sortStrategyFactory, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));
        ArgumentNullException.ThrowIfNull(sortStrategyFactory, nameof(sortStrategyFactory));

        _context = context;
        _sortStrategyFactory = sortStrategyFactory;
        _mapper = mapper;
    }

    public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpecParams)
    {
        var builder = Builders<ProductDocument>.Filter;
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
        var mappedData = _mapper.Map<IReadOnlyList<Product>>(data);

        return new Pagination<Product>(
            catalogSpecParams.PageIndex,
            catalogSpecParams.PageSize,
            (int)totalItems,
            mappedData
        );
    }

    private async Task<IList<ProductDocument>> DataFilter(CatalogSpecParams catalogSpecParams, FilterDefinition<ProductDocument> filter)
    {
        var sortBuilder = Builders<ProductDocument>.Sort;
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
        var docs = await _context
            .Products
            .Find(p => p.Id == id)
            .FirstOrDefaultAsync();

        return _mapper.Map<Product>(docs);
    }

    public async Task<IEnumerable<Product>> GetProductsByBrand(string brandName)
    {
        var filter = Builders<ProductDocument>.Filter.Eq(p => p.Brand.Name, brandName);
        var options = new FindOptions<ProductDocument>
        {
            Collation = _caseInsensitiveCollation
        };

        using var cursor = await _context.Products.FindAsync(filter, options);
        var docs = await cursor.ToListAsync();

        return _mapper.Map<IEnumerable<Product>>(docs);
    }

    public async Task<IEnumerable<Product>> GetProductsByName(string name)
    {
        var filter = Builders<ProductDocument>.Filter.Eq(p => p.Name, name);
        var options = new FindOptions<ProductDocument>
        {
            Collation = _caseInsensitiveCollation
        };

        using var cursor = await _context.Products.FindAsync(filter, options);
        var docs = await cursor.ToListAsync();

        return _mapper.Map<IEnumerable<Product>>(docs);
    }

    public async Task<Product> CreateProduct(Product product)
    {
        var doc = _mapper.Map<ProductDocument>(product);

        await _context
            .Products
            .InsertOneAsync(doc);

        return _mapper.Map<Product>(doc);
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
        var doc = _mapper.Map<ProductDocument>(product);

        var updatedProduct = await _context
            .Products
            .ReplaceOneAsync(p => p.Id == doc.Id, doc);

        return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
    }
}

using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;
public class BrandRepository : IBrandRepository
{
    private readonly ICatalogContext _context;

    public BrandRepository(ICatalogContext context)
    {
        ArgumentNullException.ThrowIfNull(nameof(context));
        _context = context;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        return await _context
            .Brands
            .Find(b => true)
            .ToListAsync();
    }
}

using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;
public class BrandRepository : IBrandRepository
{
    private readonly ICatalogContext _context;
    private readonly IMapper _mapper;

    public BrandRepository(ICatalogContext context, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductBrand>> GetAllBrands()
    {
        var docs = await _context
            .Brands
            .Find(b => true)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ProductBrand>>(docs);
    }
}

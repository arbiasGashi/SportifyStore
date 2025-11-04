using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Repositories;
public class TypeRepository : ITypeRepository
{
    private readonly ICatalogContext _context;
    private readonly IMapper _mapper;

    public TypeRepository(ICatalogContext context, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductType>> GetAllTypes()
    {
        var docs = await _context
            .Types
            .Find(t => true)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ProductType>>(docs);
    }
}

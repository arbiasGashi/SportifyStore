using Catalog.Infrastructure.Documents;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Sorting;
public class MongoPriceAscSortStrategy : IMongoSortStrategy<ProductDocument>
{
    public string Key => "priceAsc";

    public SortDefinition<ProductDocument> ApplySort(SortDefinitionBuilder<ProductDocument> builder)
    {
        return builder.Ascending(p => p.Price);
    }
}

using Catalog.Infrastructure.Documents;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Sorting;
public class MongoPriceDescSortStrategy : IMongoSortStrategy<ProductDocument>
{
    public string Key => "priceDesc";

    public SortDefinition<ProductDocument> ApplySort(SortDefinitionBuilder<ProductDocument> builder)
    {
        return builder.Descending(p => p.Price);
    }
}

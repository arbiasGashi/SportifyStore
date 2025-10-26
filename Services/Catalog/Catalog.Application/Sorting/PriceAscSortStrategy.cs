using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Application.Sorting;
public class PriceAscSortStrategy : ISortStrategy
{
    public string Key => "priceAsc";

    public SortDefinition<Product> ApplySort(SortDefinitionBuilder<Product> builder)
    {
        return builder.Ascending(p => p.Price);
    }
}

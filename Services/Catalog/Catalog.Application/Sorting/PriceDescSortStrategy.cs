using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Application.Sorting;
public class PriceDescSortStrategy : ISortStrategy
{
    public string Key => "priceDesc";

    public SortDefinition<Product> ApplySort(SortDefinitionBuilder<Product> builder)
    {
        return builder.Descending(p => p.Price);
    }
}

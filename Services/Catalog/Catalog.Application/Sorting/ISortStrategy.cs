using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Application.Sorting;
public interface ISortStrategy
{
    string Key { get; } // e.g., "priceAsc", "priceDesc", "name"
    SortDefinition<Product> ApplySort(SortDefinitionBuilder<Product> builder);
}

using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Application.Sorting;
public class NameSortStrategy : ISortStrategy
{
    public string Key => "name";

    public SortDefinition<Product> ApplySort(SortDefinitionBuilder<Product> builder)
    {
        return builder.Ascending(p => p.Name);
    }
}

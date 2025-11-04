using MongoDB.Driver;

namespace Catalog.Infrastructure.Sorting;
public interface IMongoSortStrategy<T>
{
    string Key { get; }
    SortDefinition<T> ApplySort(SortDefinitionBuilder<T> builder);
}

namespace Catalog.Infrastructure.Sorting;
public class MongoSortStrategyFactory<T>
{
    private readonly IEnumerable<IMongoSortStrategy<T>> _strategies;

    public MongoSortStrategyFactory(IEnumerable<IMongoSortStrategy<T>> strategies)
    {
        _strategies = strategies;
    }

    public IMongoSortStrategy<T> GetStrategy(string sortOption)
    {
        if (string.IsNullOrWhiteSpace(sortOption))
        {
            sortOption = "name";
        }

        var strategy = _strategies.FirstOrDefault(
            s => s.Key.Equals(sortOption, StringComparison.OrdinalIgnoreCase));

        return strategy ?? _strategies.First(s => s.Key == "name");
    }
}

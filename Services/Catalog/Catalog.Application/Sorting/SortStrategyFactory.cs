namespace Catalog.Application.Sorting;
public class SortStrategyFactory : ISortStrategyFactory
{
    private readonly IEnumerable<ISortStrategy> _strategies;

    public SortStrategyFactory(IEnumerable<ISortStrategy> strategies)
    {
        _strategies = strategies;
    }

    public ISortStrategy GetStrategy(string sortOption)
    {
        if (string.IsNullOrWhiteSpace(sortOption))
        {
            sortOption = "name";
        }

        var strategy = _strategies.FirstOrDefault(s =>
            s.Key.Equals(sortOption, StringComparison.OrdinalIgnoreCase));

        return strategy ?? _strategies.First(s => s.Key == "name");
    }
}

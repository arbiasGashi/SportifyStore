namespace Catalog.Application.Sorting;
public interface ISortStrategyFactory
{
    ISortStrategy GetStrategy(string sortOption);
}

namespace Catalog.Core.Entities;
public class Product : BaseEntity
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string ImageFile { get; set; }
    public ProductBrand Brand { get; set; }
    public ProductType Type { get; set; }
    public decimal Price { get; set; }
}

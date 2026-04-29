namespace CoreStock.Domain.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Country { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}

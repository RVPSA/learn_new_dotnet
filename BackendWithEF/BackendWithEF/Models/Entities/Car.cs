namespace BackendWithEF.Models.Entities;

public class Car
{
    public Guid ID { get; set; }
    public required string Brand { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
}
namespace BackendWithEF.Models.Dtos;

public class AddCarDTO
{
    public required string Brand { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }
}
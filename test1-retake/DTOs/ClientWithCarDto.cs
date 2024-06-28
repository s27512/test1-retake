namespace test1_retake.DTOs;

public class ClientWithCarDto
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Address { get; set; }
    public List<CarDto> Rentals { get; set; } = new List<CarDto>();

}

public class CarDto
{
    public required string Vin { get; set; }
    public required string Color { get; set; }
    public required string Model { get; set; }
    public required DateTime DateFrom { get; set; }
    public required DateTime DateTo { get; set; }
    public required int TotalPrice { get; set; }
}
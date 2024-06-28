namespace test1_retake.DTOs;

public class CreateClientWithCarDto
{
    public ClientDto Client { get; set; }
    public int CarId { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
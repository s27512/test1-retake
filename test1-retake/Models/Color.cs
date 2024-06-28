namespace test1_retake.Models;

public class Color
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    
    public ICollection<Car> Cars { get; set; }
}
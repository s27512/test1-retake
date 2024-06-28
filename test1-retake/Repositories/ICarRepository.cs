using Microsoft.Data.SqlClient;
using test1_retake.Models;
using test1_retake.Services;

namespace test1_retake.Repositories;

public interface ICarRepository
{
    Task<Car?> GetCarByIdAsync(int carId);
}

public class CarRepository: ICarRepository
{
    private readonly string _connectionString;

    public CarRepository(IDbConnectionService connectionService)
    {
        _connectionString = connectionService.GetConnectionString;
    }

    public async Task<Car?> GetCarByIdAsync(int carId)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT * FROM cars WHERE ID = @carId";
        command.Parameters.AddWithValue("@carId", carId);
        
        await connection.OpenAsync();
        
        using (var dr = await command.ExecuteReaderAsync())
        {
            if (await dr.ReadAsync())
            {
                return new Car
                {
                    Id = (int)dr["ID"],
                    Name = dr["Name"].ToString(),
                    Seats = (int)dr["Seats"],
                    ColorId = (int)dr["ColorId"],
                    ModelId = (int)dr["ModelId"],
                    PricePerDay = (int)dr["PricePerDay"],
                    VIN = dr["VIN"].ToString()
                };
            }
        }
        return null;
    }
}
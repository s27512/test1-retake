using Microsoft.Data.SqlClient;
using test1_retake.DTOs;
using test1_retake.Models;
using test1_retake.Services;

namespace test1_retake.Repositories;

public interface IClientRepository
{
    Task<ClientWithCarDto> GetClientWithCars(int clientId);
    Task AddClientWithCar(Client client, CarRentals carRental);
}

public class ClientRepository: IClientRepository
{
    private readonly string _connectionString;

    public ClientRepository(IDbConnectionService connectionService)
    {
        _connectionString = connectionService.GetConnectionString;
    }

    public async Task<ClientWithCarDto> GetClientWithCars(int clientId)
    {
        ClientWithCarDto dto = null;
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = "SELECT c.ID, c.FirstName,c.LastName,c.Address," +
                              "car. FROM Clients c" +
                              "LEFT JOIN CarRentals cr ON c.ID = cr.ClientID" +
                              "LEFT JOIN Cars car ON cr.CarID = car.ID" +
                              "LEFT JOIN Models m ON car.ModelID = m.ID" +
                              "LEFT JOIN Colors col ON car.ColorID = col.ID WHERE ClientId = @ClientId";
        command.Parameters.AddWithValue("@ClientId", clientId);
        
        await connection.OpenAsync();
        
        using var clientReader = await command.ExecuteReaderAsync();
        while (await clientReader.ReadAsync())
        {
            if (dto is null)
            {
                dto = new ClientWithCarDto
                {
                    Id = clientReader.GetInt32(clientReader.GetOrdinal("ID")),
                    FirstName = clientReader.GetString(clientReader.GetOrdinal("FirstName")),
                    LastName = clientReader.GetString(clientReader.GetOrdinal("LastName")),
                    Address = clientReader.GetString(clientReader.GetOrdinal("Address"))
                };
            }
            var carDto = new CarDto
            {
                Vin = clientReader.GetString(clientReader.GetOrdinal("VIN")),
                Color = clientReader.GetString(clientReader.GetOrdinal("Color")),
                Model = clientReader.GetString(clientReader.GetOrdinal("Model")),
                DateFrom = clientReader.GetDateTime(clientReader.GetOrdinal("DateFrom")),
                DateTo = clientReader.GetDateTime(clientReader.GetOrdinal("DateTo")),
                TotalPrice = clientReader.GetInt32(clientReader.GetOrdinal("TotalPrice"))
            };
            dto.Rentals.Add(carDto);
        }

        await connection.CloseAsync();
        return dto;
    }

    public async Task AddClientWithCar(Client client, CarRentals rental)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        
        using var transaction = connection.BeginTransaction();

        try
        {
            var clientCommand =
                new SqlCommand(
                    "INSERT INTO Clients (FirstName, LastName, Address) VALUES (@FirstName, @LastName, @Address;",
                    connection, transaction);
            clientCommand.Parameters.AddWithValue("@FirstName", client.FirstName);
            clientCommand.Parameters.AddWithValue("@LastName", client.LastName);
            clientCommand.Parameters.AddWithValue("@Address", client.Address);

            var clientId = (int)await clientCommand.ExecuteScalarAsync();

            var rentalCommand =
                new SqlCommand(
                    "INSERT INTO CarRentals (ClientID, CarID, DateFrom, DateTo, TotalPrice) VALUES (@ClientID, @CarID, @DateFrom, @DateTo, @TotalPrice)",
                    connection, transaction);
            rentalCommand.Parameters.AddWithValue("@ClientID", clientId);
            rentalCommand.Parameters.AddWithValue("@CarID", rental.CarId);
            rentalCommand.Parameters.AddWithValue("@DateFrom", rental.DateFrom);
            rentalCommand.Parameters.AddWithValue("@DateTo", rental.DateTo);
            rentalCommand.Parameters.AddWithValue("@TotalPrice", rental.TotalPrice);

            await rentalCommand.ExecuteNonQueryAsync();
            transaction.Commit();
        }
        catch(Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}
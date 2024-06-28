using test1_retake.DTOs;
using test1_retake.Models;
using test1_retake.Repositories;

namespace test1_retake.Services;

public class ClientService: IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly ICarRepository _carRepository;

    public ClientService(IClientRepository clientRepository, ICarRepository carRepository)
    {
        _clientRepository = clientRepository;
        _carRepository = carRepository;
    }

    public async Task<ClientWithCarDto> GetClientWithCarsAsync(int clientId)
    {
        return await _clientRepository.GetClientWithCars(clientId);
    }

    public async Task AddClientWithCar(CreateClientWithCarDto requestDto)
    {
        var car = await _carRepository.GetCarByIdAsync(requestDto.CarId) ?? throw new Exception("car not found");
        var client = new Client
        {
            FirstName = requestDto.Client.FirstName,
            LastName = requestDto.Client.LastName,
            Address = requestDto.Client.Address
        };
        var carRental = new CarRentals
        {
            CarId = requestDto.CarId,
            DateFrom = requestDto.DateFrom,
            DateTo = requestDto.DateTo,
        };

        await _clientRepository.AddClientWithCar(client, carRental);
    }
}
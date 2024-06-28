using test1_retake.DTOs;

namespace test1_retake.Services;

public interface IClientService
{
    Task<ClientWithCarDto> GetClientWithCarsAsync(int clientId);
    Task AddClientWithCar(CreateClientWithCarDto requestDto);
}


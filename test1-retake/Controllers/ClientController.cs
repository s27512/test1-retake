using Microsoft.AspNetCore.Mvc;
using test1_retake.DTOs;
using test1_retake.Services;

namespace test1_retake.Controllers;

[ApiController]
[Route("api/clients")]
public class ClientController: ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetClientWithIdAsync(int clientId)
    {
        var dto = _clientService.GetClientWithCarsAsync(clientId);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> AddClientWithCar(CreateClientWithCarDto requestDto)
    {
        await _clientService.AddClientWithCar(requestDto);
        return Created();
    }
}
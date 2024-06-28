namespace test1_retake.Services;

public class DbConnectionService: IDbConnectionService
{
    private readonly IConfiguration _configuration;

    public DbConnectionService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString => _configuration["ConnectionStrings:DefaultConnection"] ?? throw new NullReferenceException();
}
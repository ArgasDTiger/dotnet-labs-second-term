namespace Blazor.Core.Services;

public sealed class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GetApiUrl()
    {
        return _configuration["ApiUrl"] ?? throw new Exception("ApiUrl is not set.");
    }
}
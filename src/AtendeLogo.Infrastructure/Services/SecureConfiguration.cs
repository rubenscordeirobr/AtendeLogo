using AtendeLogo.Application.Contracts.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AtendeLogo.Infrastructure.Services;

public class SecureConfiguration : ISecureConfiguration
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;

    public SecureConfiguration(
        IConfiguration configuration,
        IHostEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }

    public string GetPasswordSalt()
    {
        if (_hostEnvironment.IsDevelopment())
        {
            return "dev-salt";
        }

        var salt = _configuration["AppSettings:PasswordSalt"];  
        if (string.IsNullOrEmpty(salt))
        {
            salt = Environment.GetEnvironmentVariable("APP_PASSWORD_SALT");
        }
 
        if (string.IsNullOrEmpty(salt))
        {
            throw new Exception("Salt key is not configured properly.");
        }
        return salt;
    }
}

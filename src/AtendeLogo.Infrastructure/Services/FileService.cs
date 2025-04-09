using AtendeLogo.Shared.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.Infrastructure.Services;

public sealed class FileService : FileServiceBase
{

    public FileService(
        ILogger<FileService> logger,
        IHostEnvironment hostEnvironment)
        : base(hostEnvironment?.ContentRootPath ?? throw new ArgumentNullException(nameof(hostEnvironment)), logger)
    {

    }

}

using AtendeLogo.Shared.Services;
using Microsoft.Extensions.Logging;

namespace AtendeLogo.TestCommon.Mocks.Infrastructure;

public sealed class FileServiceMock : FileServiceBase, IDisposable
{

    public FileServiceMock(
        ILogger<FileServiceMock> logger)
        : base(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()), logger)
    {

    }
      
    public void Dispose()
    {
        try
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, true);
            }
        }
        catch
        {
            // Ignore exceptions during cleanup
        }
    }
}

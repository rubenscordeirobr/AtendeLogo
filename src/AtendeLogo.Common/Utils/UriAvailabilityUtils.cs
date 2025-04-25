namespace AtendeLogo.Common.Utils;

public static class UriAvailabilityUtils
{
    private static readonly HttpClient SharedHttpClient = new();
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(5);

    public static Task<string> GetFirstAvailableBaseUrlAsync(
        IEnumerable<string> baseUrls,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        return GetFirstAvailableBaseUrlAsync(
            baseUrls,
            BuildPingUri,
            timeout,
            cancellationToken);
    }

    public static async Task<string> GetFirstAvailableBaseUrlAsync(
        IEnumerable<string> baseUrls,
        Func<Uri, Uri> checkUriBuilder,
        TimeSpan? timeout = null,
        CancellationToken cancellationToken = default)
    {
        Guard.NotNull(checkUriBuilder);

        var checks = baseUrls
            .Select(address => CheckAsync(address, checkUriBuilder, timeout ?? DefaultTimeout, cancellationToken))
            .ToList();

        while (checks.Count > 0)
        {
            var finished = await Task.WhenAny(checks);
            checks.Remove(finished);

            var (address, available) = await finished;
            if (available)
            {
                return address;
            }
        }

        throw new InvalidOperationException("No reachable base URL found in the provided list.");
    }

    private static async Task<(string address, bool available)> CheckAsync(
        string address,
        Func<Uri, Uri> checkUriBuilder,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        if (!Uri.TryCreate(address, UriKind.Absolute, out var uri))
        {
            return (address, false);
        }
        
        var pingUri = checkUriBuilder(uri);
        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            var response = await SharedHttpClient.GetAsync(pingUri, cts.Token);
            return (address, response.IsSuccessStatusCode);
        }
        catch (HttpRequestException)
        {
            return (address, false);
        }
        catch (OperationCanceledException)
        {
            return (address, false);
        }
        catch(Exception)
        {
            return (address, false);
        }
    }
     
    private static Uri BuildPingUri(Uri baseUri)
    {
        var builder = new UriBuilder(baseUri)
        {
            Path = "ping",
            Query = $"ticks={DateTime.UtcNow.Ticks}"
        };

        return builder.Uri;
    }

}

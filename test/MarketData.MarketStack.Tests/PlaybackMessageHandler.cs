using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace MarketData.MarketStack.Tests;

public class PlaybackMessageHandler : DelegatingHandler
{
    private readonly string _testName;
    private readonly bool _record;

    private bool ShouldRecord => bool.TryParse(Environment.GetEnvironmentVariable("RECORD_REQUEST"), out var r) && r;

    public PlaybackMessageHandler(string testName)
        : base(new HttpClientHandler())
    {
        _testName = testName;
        _record = ShouldRecord && Debugger.IsAttached;
    }

    public HttpRequestMessage? Request { get; private set; }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Request = request;

        if (!Directory.Exists("TestData"))
            Directory.CreateDirectory("TestData");

        var hash = GetHash(request.RequestUri ?? throw new InvalidOperationException("RequestUri expected"));
        var filename = Path.Combine("TestData", $"{_testName}_{hash}.json");

        if (_record)
        {
            var recordedResponse = await base.SendAsync(request, cancellationToken);
            var recordedContent = await recordedResponse.Content.ReadAsStringAsync(cancellationToken);
            await File.WriteAllTextAsync(filename, recordedContent, cancellationToken);
            return recordedResponse;
        }

        if (!File.Exists(filename))
            throw new InvalidOperationException($"Unable to find test data file {filename}");

        var playbackContent = await File.ReadAllTextAsync(filename, cancellationToken);
        var playbackResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent(playbackContent),
        };

        return playbackResponse;
    }

    private static string GetHash(Uri uri)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(uri.ToString()));

        return BitConverter
            .ToString(hashBytes)
            .Replace("-", string.Empty)[..10];
    }
}
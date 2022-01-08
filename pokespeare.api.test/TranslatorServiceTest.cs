using AutoFixture;
using Microsoft.Extensions.Options;
using Pokespeare.Models;
using Pokespeare.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace pokespeare.api.test;

public class TranslatorServiceTest
{
    [Fact]
    public async Task TranslateTextTest()
    {
        var fixture = new Fixture();

        var configuration = Options.Create(new Configuration
        {
            TranslatorUrl = "http://foo/translate",
            TranslationBreakMinutes = 5,
            TranslationCacheMinutes = 5,
        });

        var translationResponse = fixture.Create<TranslationResponse>();

        var httpClientFactory = TestHelpers.CreateTestHttpClientFactory(HttpStatusCode.OK, translationResponse);
        var logger = TestHelpers.CreateTestLogger<TranslatorService>();
        var memoryCache = TestHelpers.CreateTestMemoryCache();

        var translatorService = new TranslatorService(configuration, httpClientFactory, logger, memoryCache);

        var result = await translatorService.TranslateAsync(fixture.Create<string>()).ConfigureAwait(false);

        Assert.True(result.Success);
        Assert.NotNull(result.Text);
        Assert.Null(result.ErrorMessage);
        Assert.Equal(translationResponse.Contents.Translated, result.Text);
    }

    [Fact]
    public async Task TranslateCircuitBreakerTest()
    {
        var fixture = new Fixture();

        var configuration = Options.Create(new Configuration
        {
            TranslatorUrl = "http://foo/translate",
            TranslationBreakMinutes = 4f / 60, // 4 seconds
            TranslationCacheMinutes = 5,
        });

        var translationResponse = fixture.Create<TranslationResponse>();
        var requestCount = 0;

        var httpClientFactory = TestHelpers.CreateTestHttpClientFactory(HttpStatusCode.TooManyRequests, translationResponse, () => requestCount++);
        var logger = TestHelpers.CreateTestLogger<TranslatorService>();
        var memoryCache = TestHelpers.CreateTestMemoryCache();

        var translatorService = new TranslatorService(configuration, httpClientFactory, logger, memoryCache);

        //Make sure the translation fails
        var result = await translatorService.TranslateAsync(fixture.Create<string>()).ConfigureAwait(false);

        Assert.False(result.Success);
        Assert.Null(result.Text);
        Assert.NotNull(result.ErrorMessage);

        //Make 5 more calls to the service
        for (var i = 0; i < 5; i++)
        {
            result = await translatorService.TranslateAsync(fixture.Create<string>()).ConfigureAwait(false);
        }

        //Check that no new API calls were made
        Assert.Equal(2, requestCount);

        //Wait for circuit to close (reset)
        await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);

        //Retry
        result = await translatorService.TranslateAsync(fixture.Create<string>()).ConfigureAwait(false);

        //Check that a new API call was made
        Assert.Equal(3, requestCount);
    }
}
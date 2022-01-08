using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace pokespeare.api.test;

internal static class TestHelpers
{
    public static ILogger<T> CreateTestLogger<T>() => NullLogger<T>.Instance;

    public static IMemoryCache CreateTestMemoryCache()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider.GetRequiredService<IMemoryCache>();
    }

    public static IHttpClientFactory CreateTestHttpClientFactory(HttpStatusCode httpStatus, TranslationResponse response, Action? callback = null)
    {
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var httpClientFactory = new Mock<IHttpClientFactory>();

        var sendMethodMock = mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatus,
                Content = new StringContent(JsonSerializer.Serialize(response), Encoding.UTF8, "application/json"),
            });

        if (callback != null)
        {
            sendMethodMock.Callback(callback);
        }

        var client = new HttpClient(mockHttpMessageHandler.Object);
        httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        return httpClientFactory.Object;
    }

    public static T? GetOkObjectResultValue<T>(this IResult result)
    {
        return (T?)Type.GetType("Microsoft.AspNetCore.Http.Result.OkObjectResult, Microsoft.AspNetCore.Http.Results")?
            .GetProperty("Value",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)?
            .GetValue(result);
    }

    public static int? GetOkObjectResultStatusCode(this IResult result)
    {
        return (int?)Type.GetType("Microsoft.AspNetCore.Http.Result.OkObjectResult, Microsoft.AspNetCore.Http.Results")?
            .GetProperty("StatusCode",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)?
            .GetValue(result);
    }

    public static int? GetNotFoundResultStatusCode(this IResult result)
    {
        return (int?)Type.GetType("Microsoft.AspNetCore.Http.Result.NotFoundObjectResult, Microsoft.AspNetCore.Http.Results")?
            .GetProperty("StatusCode",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)?
            .GetValue(result);
    }
}

internal record TranslationContent(string Translated, string Translation);

internal record TranslationResponse(TranslationContent Contents);
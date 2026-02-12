using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ProbabilityCalculator.Api.Middleware;
using ProbabilityCalculator.Api.Models;

namespace ProbabilityCalculator.Tests.Middleware;

public class GlobalExceptionMiddlewareTests
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger =
        NullLoggerFactory.Instance.CreateLogger<GlobalExceptionMiddleware>();

    [Fact]
    public async Task InvokeAsync_NoException_ReturnsNormally()
    {
        RequestDelegate next = _ => Task.CompletedTask;
        var middleware = new GlobalExceptionMiddleware(next, _logger);
        var context = new DefaultHttpContext();

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task InvokeAsync_UnhandledException_Returns500WithJsonBody()
    {
        RequestDelegate next = _ => throw new InvalidOperationException("Something broke");
        var middleware = new GlobalExceptionMiddleware(next, _logger);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.Should().Be("application/json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var error = JsonSerializer.Deserialize<ApiError>(body,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        error.Should().NotBeNull();
        error!.StatusCode.Should().Be(500);
        error.Message.Should().Be("An unexpected error occurred.");
        error.Detail.Should().Contain("Something broke");
    }

    [Fact]
    public async Task InvokeAsync_ArgumentException_Returns400()
    {
        RequestDelegate next = _ => throw new ArgumentException("Bad arg");
        var middleware = new GlobalExceptionMiddleware(next, _logger);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
    }
}

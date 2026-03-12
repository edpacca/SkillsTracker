using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SkillsTracker.Middleware;

namespace SkillsTracker.Tests.UnitTests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private static async Task<(HttpStatusCode StatusCode, ProblemDetails? Body)> InvokeAsync(Exception exception)
    {
        using var host = await new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.Configure(app =>
                {
                    app.UseMiddleware<ExceptionHandlingMiddleware>();
                    app.Run(_ => throw exception);
                });
            })
            .StartAsync();

        var response = await host.GetTestClient().GetAsync("/");

        var body = await JsonSerializer.DeserializeAsync<ProblemDetails>(
            await response.Content.ReadAsStreamAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return (response.StatusCode, body);
    }

    [Fact]
    public async Task ArgumentException_Returns400()
    {
        var (status, body) = await InvokeAsync(new ArgumentException("ID mismatch."));

        Assert.Equal(HttpStatusCode.BadRequest, status);
        Assert.Equal(400, body?.Status);
        Assert.Equal("ID mismatch.", body?.Detail);
    }

    [Fact]
    public async Task KeyNotFoundException_Returns404()
    {
        var (status, body) = await InvokeAsync(new KeyNotFoundException("Resource not found."));

        Assert.Equal(HttpStatusCode.NotFound, status);
        Assert.Equal(404, body?.Status);
        Assert.Equal("Resource not found.", body?.Detail);
    }

    [Fact]
    public async Task DbUpdateConcurrencyException_Returns409()
    {
        var (status, body) = await InvokeAsync(new DbUpdateConcurrencyException());

        Assert.Equal(HttpStatusCode.Conflict, status);
        Assert.Equal(409, body?.Status);
        Assert.Equal("A concurrency conflict occurred.", body?.Detail);
    }

    [Fact]
    public async Task DbUpdateException_Returns409()
    {
        var (status, body) = await InvokeAsync(new DbUpdateException());

        Assert.Equal(HttpStatusCode.Conflict, status);
        Assert.Equal(409, body?.Status);
        Assert.Equal("The operation failed due to a database constraint.", body?.Detail);
    }

    [Fact]
    public async Task UnhandledException_Returns500()
    {
        var (status, body) = await InvokeAsync(new Exception("Something went wrong."));

        Assert.Equal(HttpStatusCode.InternalServerError, status);
        Assert.Equal(500, body?.Status);
        Assert.Equal("An unexpected error occurred.", body?.Detail);
    }

    [Fact]
    public async Task NoException_PassesThrough()
    {
        using var host = await new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.Configure(app =>
                {
                    app.UseMiddleware<ExceptionHandlingMiddleware>();
                    app.Run(async ctx =>
                    {
                        ctx.Response.StatusCode = 200;
                        await ctx.Response.WriteAsync("ok");
                    });
                });
            })
            .StartAsync();

        var response = await host.GetTestClient().GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

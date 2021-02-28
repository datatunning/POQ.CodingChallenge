// <copyright file="SwaggerTests.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace POQ.CodingChallenge.API.UnitTests
{
    public class SwaggerTests
    {
        private readonly HttpClient _client;

        public SwaggerTests()
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            var testServer = new TestServer(builder);
            _client = testServer.CreateClient();
        }

        [Fact]
        public async Task Swagger_OnInvoke_ReturnsHealthy()
        {
            var response = await _client.GetAsync(Startup.SwaggerEndpoint);
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
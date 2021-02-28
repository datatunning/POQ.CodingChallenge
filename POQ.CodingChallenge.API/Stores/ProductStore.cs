// <copyright file="ProductStore.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using POQ.CodingChallenge.API.Models;

namespace POQ.CodingChallenge.API.Stores
{
    public class ProductStore : IProductStore
    {
        private const string MockyUrl = @"http://www.mocky.io/v2/5e307edf3200005d00858b49";
        private readonly ILogger<ProductStore> _logger;

        public ProductStore(ILogger<ProductStore> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<IList<MockyProduct>> RealAll(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Retrieving (mocky) products");
                using var httpClient = new HttpClient();
                var response = await httpClient.GetFromJsonAsync<MockyResponse>(MockyUrl, cancellationToken);

                // ReSharper disable once MethodHasAsyncOverload
                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));

                return response?.products;
            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.StatusCode == HttpStatusCode.NoContent)
                {
                    _logger.LogWarning("Mocky did not returned any products.");
                    return new List<MockyProduct>();
                }

                _logger.LogError(
                    $"An error occurred while retrieving the mocky product. StatusCode: {httpEx.StatusCode}, Message: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unhandled error occurred while retrieving the mocky products. {ex.Message}");
                throw;
            }
        }
    }
}
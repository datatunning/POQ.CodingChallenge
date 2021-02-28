// <copyright file="ProductStore.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using POQ.CodingChallenge.API.Models;

namespace POQ.CodingChallenge.API.Stores
{
    public class ProductStore : IProductStore
    {
        private const string MockyUrl = @"http://www.mocky.io/v2/5e307edf3200005d00858b49";
        private IList<MockyProduct> _products;

        /// <inheritdoc />
        public async Task<IList<MockyProduct>> RealAll(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_products != null)
                {
                    Console.WriteLine("Getting (cached) products");
                    return _products;
                }

                Console.WriteLine("Retrieving (mocky) products");
                using var httpClient = new HttpClient();
                var response = await httpClient.GetFromJsonAsync<MockyResponse>(MockyUrl, cancellationToken);
                
                // ReSharper disable once MethodHasAsyncOverload
                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));

                _products = response?.products;
                return _products;
            }
            catch (HttpRequestException httpEx)
            {
                if (httpEx.StatusCode == HttpStatusCode.NoContent)
                {
                    Console.WriteLine("Mocky did not returned any products.");
                    _products = new List<MockyProduct>();
                    return _products;
                }

                Console.WriteLine($"An error occurred while retrieving the mocky product. StatusCode: {httpEx.StatusCode}, Message: {httpEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unhandled error occurred while retrieving the mocky products. {ex.Message}");
                throw;
            }
        }
    }
}
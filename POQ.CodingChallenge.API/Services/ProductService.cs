// <copyright file="ProductService.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using POQ.CodingChallenge.API.Endpoints;
using POQ.CodingChallenge.API.Models;
using POQ.CodingChallenge.API.Stores;

namespace POQ.CodingChallenge.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductStore _productStore;

        /// <summary>Initializes a new instance of the <see cref="ProductService" /> class.</summary>
        /// <param name="productStore">The product store.</param>
        public ProductService(IProductStore productStore)
        {
            Console.WriteLine(
                $"Initializing ProductService {(productStore == null ? "without" : "with")} ProductStore");
            _productStore = productStore;
        }

        /// <inheritdoc />
        public async Task<ProductFilterResponse> Filter(ProductFilterRequest request,
            CancellationToken cancellationToken = default)
        {
            // ReSharper disable once MethodHasAsyncOverload
            Console.WriteLine($"Received product filter request : {JsonConvert.SerializeObject(request, Formatting.Indented)}");

            var products = await _productStore.RealAll(cancellationToken);
            var priceRange = GetProductsPriceRange(products);
            var sizes = GetProductsSizes(products);
            var keywords = GetProductsKeywords(products);

            var filteredProducts = ApplyFilter(products, request);

            return new ProductFilterResponse(filteredProducts, priceRange, sizes, keywords);
        }

        /// <inheritdoc />
        public (int, int) GetProductsPriceRange(IEnumerable<MockyProduct> products) => (products?.Min(p => p.price) ?? 0, products?.Max(p => p.price) ?? 0);

        /// <inheritdoc />
        public string[] GetProductsSizes(IEnumerable<MockyProduct> products) => products.SelectMany(p => p.sizes).Distinct().ToArray();

        /// <inheritdoc />
        public string[] GetProductsKeywords(IEnumerable<MockyProduct> products)
        {
            return products
                .SelectMany(p => GetProductDescriptionWords(p.description))
                .GroupBy(s => s)
                .Select(g => new {
                    KeyField = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(wg => wg.Count)
                .ThenBy(og => og.KeyField)
                .Skip(5)
                .Take(10)
                .Select(r => r.KeyField)
                .ToArray();
        }

        private static IEnumerable<string> GetProductDescriptionWords(string description) => description.Split(new[] {' ', '.'}, StringSplitOptions.RemoveEmptyEntries);

        private IEnumerable<MockyProduct> ApplyFilter(IEnumerable<MockyProduct> products, ProductFilterRequest request)
        {
            var filteredProducts = request?.maxprice switch
            {
                <= 0 when string.IsNullOrWhiteSpace(request?.size) => products,
                > 0 when string.IsNullOrWhiteSpace(request?.size) => products.Where(p => p.price <= request?.maxprice).AsParallel(),
                <= 0 when !string.IsNullOrWhiteSpace(request?.size) => products.Where(p => p.sizes.Contains(request?.size)).AsParallel(),
                _ => products.Where(p => p.price <= request?.maxprice && p.sizes.Contains(request?.size)).AsParallel()
            };

            return ApplyHighlights(filteredProducts.ToList(), request?.hightlight);
        }

        private IEnumerable<MockyProduct> ApplyHighlights(List<MockyProduct> products, string highLight)
        {
            var keywords = highLight.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (!(keywords?.Count > 0)) return products;

            products.ForEach(p =>
            {
                keywords.ForEach(hWord => p.description = new StringBuilder(p.description)
                    .Replace(hWord, $"<em>{hWord}</em>")
                    .ToString()
                );

            });

            return products;
        }
    }
}
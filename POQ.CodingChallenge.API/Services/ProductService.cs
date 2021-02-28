// <copyright file="ProductService.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using POQ.CodingChallenge.API.Endpoints;
using POQ.CodingChallenge.API.Models;
using POQ.CodingChallenge.API.Stores;

namespace POQ.CodingChallenge.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger<IProductService> _logger;
        private readonly IMemoryCache _cache;
        private readonly IProductStore _productStore;
        public static readonly char[] CommonSeparators = {' ', '.', ';', ','};

        /// <summary>Initializes a new instance of the <see cref="ProductService" /> class.</summary>
        /// <param name="productStore">The product store.</param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        public ProductService(IProductStore productStore, IMemoryCache cache, ILogger<IProductService> logger)
        {
            _logger?.LogInformation(
                $"Initializing ProductService {(productStore == null ? "without" : "with")} ProductStore");
            _productStore = productStore;
            _cache = cache;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<ProductFilterResponse> FilterAsync(ProductFilterRequest request,
            CancellationToken cancellationToken = default)
        {
            // ReSharper disable once MethodHasAsyncOverload
            _logger?.LogInformation(
                $"Received product filter request : {JsonConvert.SerializeObject(request, Formatting.Indented)}");

            var products = await GetProductsAsync(cancellationToken);
            var priceRange = GetPriceRange(products);
            var sizes = GetSizes(products);
            var keywords = GetKeywords(products);

            var filteredProducts = ApplyFilter(products, request);

            return new ProductFilterResponse(filteredProducts, priceRange, sizes, keywords);
        }

        /// <summary>Gets the products.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        private async Task<IList<MockyProduct>> GetProductsAsync(CancellationToken cancellationToken)
        {
            _cache.TryGetValue("Products", out IList<MockyProduct> products);
            if (products != null) return products;

            products = await _productStore.RealAll(cancellationToken);
            _cache.Set("Products", products);
            return products;
        }

        /// <summary>Gets the price range.</summary>
        /// <param name="products">The products.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        private (int min, int max) GetPriceRange(IList<MockyProduct> products)
        {
            if (products == null) return (0, 0);
            _cache.TryGetValue("ProductPriceRange", out (int min, int max) priceRange);
            if (priceRange.max > 0 && priceRange.max > 0) return priceRange;

            priceRange = (products?.Min(p => p?.price) ?? 0, products?.Max(p => p?.price) ?? 0);
            _cache.Set("ProductPriceRange", priceRange);
            return priceRange;
        }

        /// <summary>Gets the sizes.</summary>
        /// <param name="products">The products.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        private string[] GetSizes(IList<MockyProduct> products)
        {
            if (products == null) return Array.Empty<string>();

            _cache.TryGetValue("ProductSizes", out string[] productSize);
            if (productSize != null) return productSize;

            productSize = products?.SelectMany(p => p.sizes)?.Distinct()?.ToArray();
            _cache.Set("ProductSizes", productSize);
            return productSize;
        }

        /// <summary>Gets the keywords.</summary>
        /// <param name="products">The products.</param>
        /// <returns>
        ///   List the 10 most used keywords except the top 5.
        /// </returns>
        private string[] GetKeywords(IList<MockyProduct> products)
        {
            if (products == null) return Array.Empty<string>();

            _cache.TryGetValue("ProductKeywords", out string[] productKeywords);
            if (productKeywords != null) return productKeywords;

            productKeywords = products
                .SelectMany(p => GetDescriptionWords(p.description, CommonSeparators))
                .GroupBy(s => s)
                .Select(g => new
                {
                    KeyField = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(wg => wg.Count)
                .ThenBy(og => og.KeyField)
                .Skip(5)
                .Take(10)
                .Select(r => r.KeyField)
                .OrderBy(s => s)
                .ToArray();
            _cache.Set("ProductKeywords", productKeywords);
            return productKeywords;
        }

        /// <summary>Gets the description words.</summary>
        /// <param name="description">The description.</param>
        /// <param name="separators">The separators.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        private IEnumerable<string> GetDescriptionWords(string description, params char[] separators) =>
            description.Split(separators, StringSplitOptions.RemoveEmptyEntries);

        /// <summary>
        ///     Applies the filter to the product list
        /// </summary>
        /// <param name="products">The products.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        private IEnumerable<MockyProduct> ApplyFilter(IEnumerable<MockyProduct> products, ProductFilterRequest request)
        {
            var filteredProducts = request.maxprice switch
            {
                null when string.IsNullOrWhiteSpace(request.size) => products,
                
                <= 0 when string.IsNullOrWhiteSpace(request.size) => products,
                
                not null when string.IsNullOrWhiteSpace(request.size) => products.Where(p => p.price <= request.maxprice)
                    .AsParallel(),
                
                null when !string.IsNullOrWhiteSpace(request.size) => products
                    .Where(p => p.sizes.Contains(request.size)).AsParallel(),
                
                <= 0 when !string.IsNullOrWhiteSpace(request.size) => products
                    .Where(p => p.sizes.Contains(request.size)).AsParallel(),
                
                _ => products.Where(p => p.price <= request.maxprice && p.sizes.Contains(request.size)).AsParallel()
            };

            return ApplyHighlights(filteredProducts.ToList(), request?.hightlight);
        }

        /// <summary>
        ///     Applies the highlights to the product description.
        /// </summary>
        /// <param name="products">The products.</param>
        /// <param name="highLights">The high lights.</param>
        /// <returns></returns>
        private IEnumerable<MockyProduct> ApplyHighlights(List<MockyProduct> products, string highLights)
        {
            if (string.IsNullOrWhiteSpace(highLights)) return products;

            var keywords = highLights.Split(CommonSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (keywords.Count <= 0) return products;

            return products.Select(p =>
            {
                var np = new MockyProduct()
                {
                    price = p.price,
                    sizes = p.sizes,
                    title = p.title,
                    description = p.description
                };

                keywords.ForEach(hWord => np.description = new StringBuilder(np.description)
                    .Replace(hWord, $"<em>{hWord}</em>")
                    .ToString()
                );
                return np;
            });
        }
    }
}
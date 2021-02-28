// <copyright file="IProductService.cs" company="Bruno DUVAL">
// Copyright (c) Bruno DUVAL.</copyright>

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using POQ.CodingChallenge.API.Endpoints;
using POQ.CodingChallenge.API.Models;

namespace POQ.CodingChallenge.API.Services
{
    /// <summary>
    /// Service feature for the Product
    /// </summary>
    public interface IProductService
    {
        /// <summary>Filters the specified request.</summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public Task<ProductFilterResponse> Filter(ProductFilterRequest request, CancellationToken cancellationToken);

        /// <summary>Gets the products price range.</summary>
        /// <param name="products">The products.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public (int, int) GetProductsPriceRange(IEnumerable<MockyProduct> products);

        /// <summary>Gets the products sizes.</summary>
        /// <param name="products">The products.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public string[] GetProductsSizes(IEnumerable<MockyProduct> products);

        /// <summary>Gets the products keywords.</summary>
        /// <param name="products">The products.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public string[] GetProductsKeywords(IEnumerable<MockyProduct> products);
    }
}